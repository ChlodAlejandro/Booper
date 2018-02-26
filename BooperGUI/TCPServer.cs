using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooperGUI
{
    class TCPServer
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private Dictionary<TcpClient, String> connections = new Dictionary<TcpClient, String>();
        private TextBox logbox;
        private List<String> serverlog = new List<String>();
        private Dictionary<String, int> RecentSends = new Dictionary<String, int>();
        private Dictionary<String, int> MediumRecentSends = new Dictionary<String, int>();
        private Dictionary<String, int> MinuteSends = new Dictionary<String, int>();
        private Dictionary<String, List<Violation>> violations = new Dictionary<String, List<Violation>>();
        private void log(String message)
        {
            logbox.AppendText(Environment.NewLine);
            logbox.AppendText("[SERVER] " + message);
        }
        private void threadLog(String message)
        {
            serverlog.Add(message);
        }

        async Task appendServerLog()
        {
            while (true)
            {
                if (serverlog.Count != 0)
                {
                    List<String> toAppend = serverlog;
                    foreach (String e in toAppend)
                    {
                        log(e);
                        serverlog.Remove(e);
                    }
                }
                await Task.Delay(200);
            }
        }
        Stopwatch spamTick1 = new Stopwatch();
        Stopwatch spamTick2 = new Stopwatch();
        Stopwatch spamTick3 = new Stopwatch();
        async Task antiSpam1()
        {
            while (true)
            {
                foreach(String node in RecentSends.Keys)
                {
                    if (RecentSends[node] >= 10)
                    {
                        tempban(node, "Spam: Type 1 - More than 10 messages in 5 seconds", "AntiSpam", 10);
                    }
                }
                if (spamTick1.ElapsedMilliseconds / 1000 >= 5)
                {
                    spamTick1.Restart();
                    RecentSends.Clear();
                }
                await Task.Delay(50);
            }
        }
        async Task antiSpam2()
        {
            while (true)
            {
                foreach (String node in MediumRecentSends.Keys)
                {
                    if (MediumRecentSends[node] >= 50)
                    {
                        tempban(node, "Spam: Type 2 - More than 50 messages in 30 seconds", "AntiSpam", 10);
                    }
                }
                if (spamTick2.ElapsedMilliseconds / 1000 >= 30)
                {
                    spamTick2.Restart();
                    MediumRecentSends.Clear();
                }
                await Task.Delay(50);
            }
        }
        async Task antiSpam3()
        {
            while (true)
            {
                foreach (String node in MinuteSends.Keys)
                {
                    if (MinuteSends[node] >= 10)
                    {
                        tempban(node, "Spam: Type 3 - More than 100 messages in 1 minute", "AntiSpam", 10);
                    }
                }
                if (spamTick3.ElapsedMilliseconds / 1000 >= 60)
                {
                    spamTick3.Restart();
                    MinuteSends.Clear();
                }
                await Task.Delay(50);
            }
        }
        async Task multiviolations()
        {
            while (true)
            {
                foreach (String node in violations.Keys)
                {
                    bool ban = false;
                    int violated = 0;
                    foreach (Violation v in violations[node]) {
                        if (v.IsActive())
                        {
                            ban = true;
                            violated++;
                        }
                    }
                    if (ban == false && violated == 5)
                    {
                        tempban(node, "5 violations in one day. Banned for 1 day.", "BooperServer", 3600);
                    }
                }
                await Task.Delay(50);
            }
        }

        public void tempban(String callsign, String reason, String cause, long seconds)
        {
            List<Violation> newViolationsList = new List<Violation>();
            newViolationsList.Add(new Violation(callsign, reason, cause, seconds));
            if (violations.ContainsKey(callsign))
            {
                violations[callsign] = newViolationsList;
            }
            else
            {
                violations.Add(callsign, newViolationsList);
            }
            TcpClient client = new TcpClient();
            foreach (TcpClient clients in connections.Keys)
            {
                if (connections[clients] == callsign)
                {
                    client = clients;
                }
            }
            reply(client, "Banned by " + banned(client, 2) + ": " + banned(client, 1) + " - Ends on " + banned(client, 3));
        }

        public void ban(String callsign, String reason, String cause)
        {
            List<Violation> newViolationsList = violations[callsign];
            newViolationsList.Add(new Violation(callsign, reason, cause, 315328464000));
            if (violations.ContainsKey(callsign))
            {
                violations[callsign] = newViolationsList;
            }
            else
            {
                violations.Add(callsign, newViolationsList);
            }
            TcpClient client = new TcpClient();
            foreach (TcpClient clients in connections.Keys)
            {
                if (connections[clients] == callsign)
                {
                    client = clients;
                }
            }
            reply(client, "Banned by " + banned(client, 2) + ": " + banned(client, 1) + " - Ends on " + banned(client, 3));
        }

        public TCPServer(TextBox syslog)
        {
            #pragma warning disable CS4014
            antiSpam1();
            antiSpam2();
            antiSpam3();
            logbox = syslog;
            log("Server instantiated.");
            connections.Clear();
            this.tcpListener = new TcpListener(IPAddress.Any, 3333);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            log("Starting port listener...");
            this.listenThread.Start();
            log("Port listener started. Listening for connections...");
            #pragma warning disable CS4014
            appendServerLog();
        }

        private void ListenForClients()
        {
            try
            {
                tcpListener.Start();
            } catch (SocketException e)
            {
                if (e.Message == "Only one usage of each socket address(protocol / network address / port) is normally permitted")
                {
                    threadLog("A server is already running! Close all programs using port '3333' and restart this program.");
                }
            }
            

            while (true)
            {
                threadLog("Awaiting connection...");
                TcpClient client = new TcpClient();
                try
                {
                    client = this.tcpListener.AcceptTcpClient();
                } catch
                {}
                
                
                //create a thread to handle communication
                //with connected client
                threadLog("Starting new communication thread...");
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
                threadLog("New client communication thread started.");
            }

            
        }

        private void HandleClientComm(Object clienta)
        {
            TcpClient client = (TcpClient)clienta;
            NetworkStream clientStream = null;
            try
            {
                clientStream = client.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();

                byte[] message = new byte[4096];
                int bytesRead;

                bool authok = false;
                while (authok == false)
                {
                    try
                    {
                        bytesRead = clientStream.Read(message, 0, 4096);
                        String response = encoder.GetString(message, 0, bytesRead);
                        if (response.Contains("}[--ENV_AUTHENTICATE--]{: "))
                        {
                            String[] auth = response.Replace("}[--ENV_AUTHENTICATE--]{: ", "").Replace("cs: ", "").Split('|');
                            authok = true;
                            reply(client, "--auth--ok--");
                            connections.Add(client, auth[1]);
                            broadcast(" " + auth[1] + " has connected.");
                        }
                        else
                        {
                            threadLog("Client has not yet authenticated.");
                            reply(client, "--auth-notauth--");
                        }
                    }
                    catch
                    {
                        threadLog("Error in authentication!");
                    }
                }

                while (authok == true)
                {
                    bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }

                    //message has successfully been received
                    String callsign;
                    connections.TryGetValue(client, out callsign);
                    String response = encoder.GetString(message, 0, bytesRead);
                    threadLog("[" + callsign + "]" + "Message received:");
                    if (response.Contains("--ENV"))
                    {
                        if (response.Contains("--ENV-callsign: "))
                        {
                            threadLog(client.GetHashCode() + " requested callsign change.");
                            String oldsign;
                            connections.TryGetValue(client, out oldsign);
                            String newCS = response.Substring(16, response.Length - 16);
                            threadLog(client.GetHashCode() + "'s new callsign is " + newCS + " (old: " + oldsign + ")");
                            connections.Remove(client);
                            connections.Add(client, newCS);
                            reply(client, "--ENV-callsign-changed");

                        }
                        if (response.Contains("--ENV-DISconnEct"))
                        {
                            reply(client, "--ENV__DISCONNECT--(): User has disconnected");
                            connections.Remove(client);
                            client.Close();
                        }
                        if (response.Contains("--ENV-hide"))
                        {
                            broadcast(callsign + " hid their Booper.");
                        }
                    }
                    else
                    {
                        if (!banned(client)) 
                        {
                            threadLog("[" + callsign + "] " + response);
                            broadcast("[" + callsign + "] " + response);
                        } else
                        {
                            reply(client, "Banned by " + banned(client, 2) + ": " + banned(client, 1) + " - Ends on " + banned(client, 3));
                        }
                        bool banOn = false;
                        if (banOn)
                        {
                            if (!RecentSends.ContainsKey(callsign))
                            {
                                RecentSends.Add(callsign, 1);
                            }
                            else
                            {
                                RecentSends[callsign]++;
                            }
                            if (!MediumRecentSends.ContainsKey(callsign))
                            {
                                MediumRecentSends.Add(callsign, 1);
                            }
                            else
                            {
                                MediumRecentSends[callsign]++;
                            }
                            if (!MinuteSends.ContainsKey(callsign))
                            {
                                MinuteSends.Add(callsign, 1);
                            }
                            else
                            {
                                MinuteSends[callsign]++;
                            }
                        }
                        
                    }
                }

                threadLog(client.GetHashCode() + " disconnected.");
                connections.Remove(client);
                client.Close();
            }
            catch { }
        }

        private bool banned(TcpClient client)
        {
            if (violations.ContainsKey(connections[client]))
            {
                bool activity = false;
                foreach(Violation v in violations[connections[client]])
                {
                    if (v.IsActive())
                    {
                        activity = true;
                    }
                }
                return activity;
            } else
            {
                return false;
            }
        }
        private String banned(TcpClient client, int e)
        {
            if (violations.ContainsKey(connections[client]))
            {
                Violation currentViolation = new Violation();
                foreach (Violation v in violations[connections[client]])
                {
                    if (v.IsActive())
                    {
                        currentViolation = v;
                    }
                }
                switch(e)
                {
                    case 0:
                        return currentViolation.Callsign;
                    case 1:
                        return currentViolation.Reason;
                    case 2:
                        return currentViolation.Cause;
                    case 3:
                        DateTime end = new DateTime(1970, 1, 1, 0, 0, 0).Add(new TimeSpan(0, 0, Int32.Parse((currentViolation.UnixEnd - currentViolation.UnixStart).ToString())));
                        return end.Month + "/" + end.Date + "/" + end.Year + " " + end.Hour + ":" + end.Minute + ":" + end.Second;
                    default:
                        return "Invalid index.";
                }
            }
            else
            {
                return "No active violations.";
            }
        }


        private static void reply(TcpClient client, String message)
        {
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(message);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private void broadcast(String message)
        {
            foreach (TcpClient client in connections.Keys)
            {
                DateTime now = DateTime.Now;
                String timenow = "[" + now.Month + "/" + now.Day + "/" + now.Year + " " + now.Hour + ":" + now.Minute + ":" + now.Second + "]";
                NetworkStream clientStream = client.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(timenow + message);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

            }
        }

        public void close()
        {
            this.tcpListener.Stop();
            List<TcpClient> tobeDisconnected = new List<TcpClient>();
            foreach (TcpClient node in connections.Keys)
            {
                
                NetworkStream clientStream = node.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes("--ENV__DISCONNECT--(): Server closed");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                String callsign;
                connections.TryGetValue(node, out callsign);
                threadLog("Removed " + callsign + " (" + callsign + ")");
                tobeDisconnected.Add(node);
                
            }
            foreach (TcpClient node in tobeDisconnected)
            {
                connections.Remove(node);
            }
            tobeDisconnected = null;
            tcpListener = null;
            listenThread.Abort();
        }
        public void forceclose()
        {
            this.tcpListener.Stop();
            List<TcpClient> tobeDisconnected = new List<TcpClient>();
            foreach (TcpClient node in connections.Keys)
            {

                NetworkStream clientStream = node.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes("--ENV__DISCONNECT--(): Server closed");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                String callsign;
                connections.TryGetValue(node, out callsign);
                tobeDisconnected.Add(node);

            }
            foreach (TcpClient node in tobeDisconnected)
            {
                connections.Remove(node);
            }
            tobeDisconnected = null;
            tcpListener = null;
            listenThread.Abort();
        }
    }
}
