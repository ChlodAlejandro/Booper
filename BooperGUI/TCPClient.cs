using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooperGUI
{
    class TCPClient
    {
        TextBox logBox;
        void log(String message)
        {
            logBox.AppendText(Environment.NewLine);
            logBox.AppendText(message);
        }
        void log(String message, int a)
        {
            logBox.AppendText(Environment.NewLine);
            logBox.AppendText(message);
            logBox.AppendText(Environment.NewLine);
        }
        void log(String message, bool b)
        {
            logBox.AppendText(message);
        }
        static String callsign = "";
        public TcpClient client = new TcpClient();
        public TCPClient(String hostname, int intport, String acallsign, String password, Label status, TextBox chatbox, Button connect, Button disconnect)
        {
            logBox = chatbox;
            log("Starting BooperClient v0.1");
            callsign = acallsign;
            log("Creating connection...");
            log("Authenticating...");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(hostname), intport);
            try
            {
                client.Connect(serverEndPoint);
            } catch (SocketException e)
            {
                BooperGUI.connStatusText = "Connection failed. " + e.Message;
            }
            String response = WaitReplyFromServer(client, "}[--ENV_AUTHENTICATE--]{: " + password + "|cs: " + callsign);
            
            bool authok = false;
            switch (response)
            {
                case "--auth--ok--":
                    log("Successful authentication.");
                    authok = true;
                    break;
                case "--auth--passw--":
                    log("Wrong password.");
                    BooperGUI.connectedButton = true;
                    client.Close();
                    break;
                case "--ENV-noserver":
                    log("There is no server on that hostname");
                    BooperGUI.connectedButton = true;
                    client.Close();
                    break;
                default:
                    log("Error in authentication!");
                    BooperGUI.connectedButton = true;
                    client.Close();
                    break;
            }
            if (authok == false)
            {
                log("Auth failed. TCPClient shutting down.");
                BooperGUI.connectedButton = true;
                BooperGUI.connStatusText = "Authentication failed.";
                
                return;
            } else
            {
                log("Connected successfully.", 1);
                BooperGUI.connectedButton = false;
                BooperGUI.disconnectedButton = true;
                BooperGUI.connStatusText = "Connected.";
            }
        }

        internal void SendDataToServer(TcpClient client, String ASCIIdata)
        {
            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(ASCIIdata);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        internal String WaitReplyFromServer(TcpClient client, String ASCIIdata)
        {
            try
            {
                NetworkStream clientStream = client.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(ASCIIdata);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                if (!ASCIIdata.Contains("--ENV"))
                {
                    log("Sent data. Awaiting reply...");
                }

                byte[] message = new byte[4096];
                int bytesRead = 0;
                //TODO data
                bytesRead = clientStream.Read(message, 0, 4096);
                String response = encoder.GetString(message, 0, bytesRead);
                return response;
            } catch
            {
                return ("--ENV-noserver");
            }
        }

        public void disconnect()
        {
            SendDataToServer(client, "--ENV-DISconnEct");
        }

        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
        public TcpClient getClient()
        {
            return client;
        }
    }
}
