using BooperGUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooperGUI
{
    public partial class BooperGUI : Form
    {
        
        private TCPServer serv = null;
        public TextBox chat;

        TCPClient client = null;

        internal TCPServer Serv { get => serv; set => serv = value; }
        public List<String> toBeLogged = new List<String>();
        public static Boolean connectedButton = true;
        public static Boolean disconnectedButton = false;
        public static String connStatusText = "Not connected.";
        private void BooperGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!serv.closed)
            {
                DialogResult result = MessageBox.Show("Press the button below to stop the server.", "Exiting BooperGUI", MessageBoxButtons.OK);
                if (!(client == null))
                {
                    if (!client.disconnected)
                    {
                        client.disconnect();
                    }
                }
                serv.forceclose();
            }
        }
        public void log(String message)
        {
            try
            {
                textChat.AppendText(message);
                textChat.AppendText(Environment.NewLine);
            }
            catch { }
            
        }
        public void log(String message, bool a)
        {
            try
            {
                textChat.AppendText(message);
            }
            catch { }

        }
        async void threadLog()
        {
            while(true)
            {
                if (toBeLogged.Count != 0)
                {
                    List<String> toAppend = toBeLogged;
                    foreach (String e in toAppend.ToList())
                    {
                        if (e.Contains("\n"))
                        {
                            foreach (String a in e.Replace("\n", "»").Split('»')) {
                                if (a != " " && a != "" && a != "\n" && a != "\\n" && a != "»")
                                {
                                    log(a + "\n", true);
                                }
                            }
                        } else
                        {
                            log(e + "\n", true);
                        }
                        toBeLogged.Remove(e);
                    }
                }
                
                await Task.Delay(200);
            }
        }
        bool hostboxEnabled = true;
        bool callsignboxEnabled = true;
        bool portboxEnabled = true;
        bool passwordboxEnabled = false;
        async void textCheck()
        {
            while (true)
            {
                txtHost.Enabled = hostboxEnabled;
                txtCallsign.Enabled = callsignboxEnabled;
                txtPort.Enabled = portboxEnabled;
                txtPassword.Enabled = passwordboxEnabled;
                await Task.Delay(200);
            }
        }

        async void buttonCheck()
        {
            while (true)
            {
                Button connectButton = btnConnect;
                Button disconnectButton = btnDisconnect;
                if (connectedButton)
                {
                    connectButton.Enabled = true;
                } else
                {
                    connectButton.Enabled = false;
                }
                if (disconnectedButton)
                {
                    disconnectButton.Enabled = true;
                }
                else
                {
                    disconnectButton.Enabled = false;
                }
                connStatus.Text = connStatusText;
                await Task.Delay(200);
            }
        }
        private void resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon.Visible = true;
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                notifyIcon.Visible = false;
            }
        }
        private void reshow(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }
        public BooperGUI()
        {
            InitializeComponent();
            buttonCheck();
            textCheck();
            chat = textChat;
            chat.Clear();
            log("BooperGUI version v0.0.1 by Chlod Aidan Alejandro");
            Button sendButton = btnSend;
            sendButton.Click += btsend;
            Button connect = btnConnect;
            connect.Click += ConnectServerAsync;
            Button disconnect = btnDisconnect;
            disconnect.Click += DisconnectServer;
            Button server = btnServer;
            server.Click += HandleServer;
            FormClosing += BooperGUI_FormClosing;
            bool escaping = false;
            Stopwatch escapeStopwatch = new Stopwatch();
            Resize += resize;
            notifyIcon.DoubleClick += reshow;
            textSendBox.KeyDown += chatbox_KeyDown;
            textSendBox.KeyUp += chatbox_KeyUp;
            void escape(Object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (escaping)
                    {
                        if (escapeStopwatch.ElapsedMilliseconds >= 2000)
                        {
                            escaping = false;
                            escapeStopwatch.Reset();
                        }
                        else
                        {
                            notifyIcon.Visible = true;
                            Hide();
                            if (client != null)
                            {
                                SendDataToServer(client.getClient(), "--ENV-hide");
                            }
                            escaping = false;
                        }
                    }
                    else
                    {
                        escaping = true;
                        log("ESCAPING!");
                        escapeStopwatch.Start();
                    }
                }
                e.Handled = true;
            }

            KeyDown += escape;
            async void ConnectServerAsync(Object sender, EventArgs e)
            {
                connectedButton = false;
                String hostname = txtHost.Text;
                String port = txtPort.Text;
                String callsign = txtCallsign.Text;
                String password = TCPClient.getHashSha256(txtPassword.Text);
                Label status = connStatus;
                if (hostname == "")
                {
                    log("Hostname field is empty!");
                    connectedButton = true;
                    return;
                }
                IPAddress ip;
                if (!IPAddress.TryParse(hostname, out ip))
                {
                    log("Hostname is not a valid IP Address!");
                    connectedButton = true;
                    return;
                }
                int intport;
                if (!Int32.TryParse(port, out intport))
                {
                    log("Port is not a valid port.");
                    connectedButton = true;
                    return;
                }
                if (intport < 0 || intport > Int16.MaxValue)
                {
                    log("Port is out of range. (1-65535)");
                    connectedButton = true;
                    return;
                }
                if (new Regex("/[a-zA-Z0-9-_]{5,32}/g").IsMatch(callsign) && callsign.Count() > 4 && callsign.Count() < 33)
                {
                    log("Callsign invalid (a-z, A-Z, 0-9, -, _, 5 to 32 characters)");
                    connectedButton = true;
                    return;
                }

                status.Text = "Connecting...";

                txtPassword.Clear();

                client = new TCPClient(hostname, intport, callsign, password, status, chat, btnConnect, btnDisconnect);
                DateTime time = new DateTime();
                long connecttime = time.Ticks;
                while (status.Text != "Connected.")
                { 
                    await Task.Delay(200);
                    if (time.Ticks - connecttime > 100000000)
                    {
                        status.Text = "Connection timed out.";
                        connectedButton = true;
                        client = null;
                        return;
                    }
                }
                hostboxEnabled = false;
                callsignboxEnabled = false;
                portboxEnabled = false;
                passwordboxEnabled = false;
                Thread listenThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                listenThread.Start(client.getClient());
                threadLog();
            }
        }
        public void btsend(Object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(chat.Text))
            {
                sendMessage();
                textSendBox.Text = "";
            }
        }

        public void DisconnectServer(Object sender, EventArgs e)
        {
            client.disconnect();
            hostboxEnabled = true;
            callsignboxEnabled = true;
            portboxEnabled = true;
            //passwordboxEnabled = true;
        }

        void sendMessage()
        {
            if (client != null)
            {
                if (!textSendBox.Text.Contains("--ENV"))
                {
                    client.SendDataToServer(client.client, textSendBox.Text);
                    textSendBox.Text = "";
                }
                else
                {
                    String command = textSendBox.Text;
                    if (serv != null)
                    {
                        /*if (command.Contains("--ENV-tempban: "))
                        {
                            command.Replace("--ENV-tempban: ", "");
                            String[] banargs = command.Split('\\');
                            if (banargs.Count() == 3)
                                serv.tempban(banargs[0], banargs[1], txtCallsign.Text, Int64.Parse(banargs[2]));
                            else
                                log("Invalid command arguments");
                        }
                        if (command.Contains("--ENV-ban: "))
                        {
                            command.Replace("--ENV-ban: ", "");
                            String[] banargs = command.Split('\\');
                            if (banargs.Count() == 2)
                                serv.ban(banargs[0], banargs[1], txtCallsign.Text);
                            else
                                log("Invalid command arguments");
                        }*/
                        if (command.Contains("--ENV-close"))
                        {
                            HandleServer("Manual trigger.", new EventArgs());
                        }
                        if (command.Contains("--ENV-help"))
                        {
                            log("BooperServer Administrator Commands:");
                            //log("--ENV-tempban: <callsign>\\<reason>\\<time in seconds> - temporarily ban someone");
                            //log("--ENV-ban: <callsign>\\<reason> - permanently ban someone");
                            log("--ENV-close - close the server");
                        }
                    }
                    else
                    {
                        log("Command not run - not hosting server.");
                    }
                }
            }
            else
            {
                log("\n[!] Client not initialized. Connect to a server using the connection menu.");
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient) client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
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
                ASCIIEncoding encoder = new ASCIIEncoding();
                String callsign;
                String response = encoder.GetString(message, 0, bytesRead);
                if (response.Contains("--ENV")) 
                {
                    if (response.Contains("--ENV__DISCONNECT--(): "))
                    {
                        String disconnectMessage = response.Replace("--ENV__DISCONNECT--(): ", "");
                        toBeLogged.Add("Disconnected. Message: " + disconnectMessage);
                        connStatusText = "Not connected.";
                        disconnectedButton = false;
                        connectedButton = true;
                        tcpClient.Close();
                    }
                }
                toBeLogged.Add("\n" + response);
            }
            toBeLogged.Add("Server disconnected.");
            tcpClient.Close();
        }

        static void SendDataToServer(TcpClient client, String ASCIIdata)
        {
            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(ASCIIdata);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        void HandleServer(Object sender, EventArgs a)
        {
            if (btnServer.Text == "Start Server")
            {
                serv = new TCPServer(chat);
                btnServer.Text = "Stop Server";
            } else
            {
                serv.close();
                serv.closed = true;
                btnServer.Text = "Start Server";
            }
        }

        bool keyshift = false;
        private void chatbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift)
            {
                keyshift = true;
            }
            if ((e.KeyCode == Keys.Enter) && keyshift)
            {
                textSendBox.AppendText("lolkys");
                textSendBox.AppendText(Environment.NewLine);
            } else if (e.KeyCode == Keys.Enter)
            {
                sendMessage();
            }
        }

        private void chatbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift)
            {
                keyshift = false;
            }
        }
    }
}
