namespace BooperGUI
{
    partial class BooperGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BooperGUI));
            this.groupChatArea = new System.Windows.Forms.GroupBox();
            this.textChat = new System.Windows.Forms.TextBox();
            this.textSendBox = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupConn = new System.Windows.Forms.GroupBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.connStatus = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.lblCallsign = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblHostname = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupChatArea.SuspendLayout();
            this.groupConn.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupChatArea
            // 
            this.groupChatArea.Controls.Add(this.textChat);
            this.groupChatArea.Location = new System.Drawing.Point(12, 12);
            this.groupChatArea.Name = "groupChatArea";
            this.groupChatArea.Size = new System.Drawing.Size(601, 447);
            this.groupChatArea.TabIndex = 0;
            this.groupChatArea.TabStop = false;
            this.groupChatArea.Text = "Chat";
            // 
            // textChat
            // 
            this.textChat.Location = new System.Drawing.Point(7, 20);
            this.textChat.Multiline = true;
            this.textChat.Name = "textChat";
            this.textChat.ReadOnly = true;
            this.textChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textChat.Size = new System.Drawing.Size(588, 421);
            this.textChat.TabIndex = 0;
            // 
            // textSendBox
            // 
            this.textSendBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textSendBox.Location = new System.Drawing.Point(12, 463);
            this.textSendBox.Multiline = true;
            this.textSendBox.Name = "textSendBox";
            this.textSendBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textSendBox.Size = new System.Drawing.Size(729, 36);
            this.textSendBox.TabIndex = 1;
            this.textSendBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatbox_KeyDown);
            this.textSendBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.chatbox_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(747, 463);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(52, 36);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // groupConn
            // 
            this.groupConn.Controls.Add(this.btnServer);
            this.groupConn.Controls.Add(this.connStatus);
            this.groupConn.Controls.Add(this.lblPassword);
            this.groupConn.Controls.Add(this.txtPassword);
            this.groupConn.Controls.Add(this.btnDisconnect);
            this.groupConn.Controls.Add(this.btnConnect);
            this.groupConn.Controls.Add(this.txtCallsign);
            this.groupConn.Controls.Add(this.lblCallsign);
            this.groupConn.Controls.Add(this.txtPort);
            this.groupConn.Controls.Add(this.lblPort);
            this.groupConn.Controls.Add(this.lblHostname);
            this.groupConn.Controls.Add(this.txtHost);
            this.groupConn.Location = new System.Drawing.Point(620, 13);
            this.groupConn.Name = "groupConn";
            this.groupConn.Size = new System.Drawing.Size(179, 444);
            this.groupConn.TabIndex = 3;
            this.groupConn.TabStop = false;
            this.groupConn.Text = "Connection";
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(6, 415);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(167, 23);
            this.btnServer.TabIndex = 11;
            this.btnServer.Text = "Start Server";
            this.btnServer.UseVisualStyleBackColor = true;
            // 
            // connStatus
            // 
            this.connStatus.AutoSize = true;
            this.connStatus.Location = new System.Drawing.Point(53, 247);
            this.connStatus.Name = "connStatus";
            this.connStatus.Size = new System.Drawing.Size(81, 13);
            this.connStatus.TabIndex = 10;
            this.connStatus.Text = "Not connected.";
            this.connStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(7, 150);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(10, 166);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(163, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.Text = "PasswordsDisabled";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(10, 221);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(163, 23);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 192);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(163, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // txtCallsign
            // 
            this.txtCallsign.Location = new System.Drawing.Point(10, 123);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(163, 20);
            this.txtCallsign.TabIndex = 5;
            // 
            // lblCallsign
            // 
            this.lblCallsign.AutoSize = true;
            this.lblCallsign.Location = new System.Drawing.Point(7, 107);
            this.lblCallsign.Name = "lblCallsign";
            this.lblCallsign.Size = new System.Drawing.Size(43, 13);
            this.lblCallsign.TabIndex = 4;
            this.lblCallsign.Text = "Callsign";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(10, 79);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(163, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "3333";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(7, 63);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Location = new System.Drawing.Point(7, 19);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(55, 13);
            this.lblHostname.TabIndex = 1;
            this.lblHostname.Text = "Hostname";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(10, 35);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(163, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.Text = "127.0.0.1";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Double click to reopen BooperGUI";
            this.notifyIcon.BalloonTipTitle = "BooperGUI";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "BooperGUI";
            // 
            // BooperGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 505);
            this.Controls.Add(this.groupConn);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textSendBox);
            this.Controls.Add(this.groupChatArea);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "BooperGUI";
            this.Text = "Booper";
            this.groupChatArea.ResumeLayout(false);
            this.groupChatArea.PerformLayout();
            this.groupConn.ResumeLayout(false);
            this.groupConn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupChatArea;
        private System.Windows.Forms.TextBox textChat;
        private System.Windows.Forms.TextBox textSendBox;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox groupConn;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.Label lblCallsign;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label connStatus;
        private System.Windows.Forms.Button btnServer;
        public System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

