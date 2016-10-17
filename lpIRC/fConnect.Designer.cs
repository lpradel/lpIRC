namespace lpIRC
{
    partial class fConnect
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
            this.fConnectBtnConnect = new System.Windows.Forms.Button();
            this.fConnectLblAddress = new System.Windows.Forms.Label();
            this.fConnectLblPort = new System.Windows.Forms.Label();
            this.fConnectBoxAddress = new System.Windows.Forms.TextBox();
            this.fConnectBoxPort = new System.Windows.Forms.TextBox();
            this.fConnectGrpServer = new System.Windows.Forms.GroupBox();
            this.fConnectGrpLogin = new System.Windows.Forms.GroupBox();
            this.fConnectLblHint = new System.Windows.Forms.Label();
            this.fConnectBoxNicknames = new System.Windows.Forms.TextBox();
            this.fConnectLblNicks = new System.Windows.Forms.Label();
            this.fConnectLblUserName = new System.Windows.Forms.Label();
            this.fConnectBoxRealName = new System.Windows.Forms.TextBox();
            this.fConnectBoxUserName = new System.Windows.Forms.TextBox();
            this.fConnectLblRealName = new System.Windows.Forms.Label();
            this.fConnectCheckBoxData = new System.Windows.Forms.CheckBox();
            this.fConnectGrpServer.SuspendLayout();
            this.fConnectGrpLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // fConnectBtnConnect
            // 
            this.fConnectBtnConnect.Location = new System.Drawing.Point(154, 271);
            this.fConnectBtnConnect.Name = "fConnectBtnConnect";
            this.fConnectBtnConnect.Size = new System.Drawing.Size(140, 28);
            this.fConnectBtnConnect.TabIndex = 0;
            this.fConnectBtnConnect.Text = "&Connect";
            this.fConnectBtnConnect.UseVisualStyleBackColor = true;
            this.fConnectBtnConnect.Click += new System.EventHandler(this.fConnectBtnConnect_Click);
            // 
            // fConnectLblAddress
            // 
            this.fConnectLblAddress.AutoSize = true;
            this.fConnectLblAddress.Location = new System.Drawing.Point(6, 25);
            this.fConnectLblAddress.Name = "fConnectLblAddress";
            this.fConnectLblAddress.Size = new System.Drawing.Size(81, 13);
            this.fConnectLblAddress.TabIndex = 1;
            this.fConnectLblAddress.Text = "Server address:";
            // 
            // fConnectLblPort
            // 
            this.fConnectLblPort.AutoSize = true;
            this.fConnectLblPort.Location = new System.Drawing.Point(17, 54);
            this.fConnectLblPort.Name = "fConnectLblPort";
            this.fConnectLblPort.Size = new System.Drawing.Size(67, 13);
            this.fConnectLblPort.TabIndex = 2;
            this.fConnectLblPort.Text = "Port number:";
            // 
            // fConnectBoxAddress
            // 
            this.fConnectBoxAddress.Location = new System.Drawing.Point(94, 22);
            this.fConnectBoxAddress.Name = "fConnectBoxAddress";
            this.fConnectBoxAddress.Size = new System.Drawing.Size(173, 20);
            this.fConnectBoxAddress.TabIndex = 3;
            // 
            // fConnectBoxPort
            // 
            this.fConnectBoxPort.Location = new System.Drawing.Point(94, 51);
            this.fConnectBoxPort.Name = "fConnectBoxPort";
            this.fConnectBoxPort.Size = new System.Drawing.Size(173, 20);
            this.fConnectBoxPort.TabIndex = 4;
            this.fConnectBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fConnectBoxPort_KeyPress);
            // 
            // fConnectGrpServer
            // 
            this.fConnectGrpServer.Controls.Add(this.fConnectLblAddress);
            this.fConnectGrpServer.Controls.Add(this.fConnectBoxPort);
            this.fConnectGrpServer.Controls.Add(this.fConnectLblPort);
            this.fConnectGrpServer.Controls.Add(this.fConnectBoxAddress);
            this.fConnectGrpServer.Location = new System.Drawing.Point(12, 12);
            this.fConnectGrpServer.Name = "fConnectGrpServer";
            this.fConnectGrpServer.Size = new System.Drawing.Size(281, 88);
            this.fConnectGrpServer.TabIndex = 5;
            this.fConnectGrpServer.TabStop = false;
            this.fConnectGrpServer.Text = "IRC-Server";
            // 
            // fConnectGrpLogin
            // 
            this.fConnectGrpLogin.Controls.Add(this.fConnectLblHint);
            this.fConnectGrpLogin.Controls.Add(this.fConnectBoxNicknames);
            this.fConnectGrpLogin.Controls.Add(this.fConnectLblNicks);
            this.fConnectGrpLogin.Controls.Add(this.fConnectLblUserName);
            this.fConnectGrpLogin.Controls.Add(this.fConnectBoxRealName);
            this.fConnectGrpLogin.Controls.Add(this.fConnectBoxUserName);
            this.fConnectGrpLogin.Controls.Add(this.fConnectLblRealName);
            this.fConnectGrpLogin.Location = new System.Drawing.Point(12, 112);
            this.fConnectGrpLogin.Name = "fConnectGrpLogin";
            this.fConnectGrpLogin.Size = new System.Drawing.Size(280, 153);
            this.fConnectGrpLogin.TabIndex = 6;
            this.fConnectGrpLogin.TabStop = false;
            this.fConnectGrpLogin.Text = "Login Credentials";
            // 
            // fConnectLblHint
            // 
            this.fConnectLblHint.AutoSize = true;
            this.fConnectLblHint.Location = new System.Drawing.Point(25, 125);
            this.fConnectLblHint.Name = "fConnectLblHint";
            this.fConnectLblHint.Size = new System.Drawing.Size(191, 13);
            this.fConnectLblHint.TabIndex = 11;
            this.fConnectLblHint.Text = "Hint: Separate nicknames with spaces.";
            // 
            // fConnectBoxNicknames
            // 
            this.fConnectBoxNicknames.Location = new System.Drawing.Point(94, 85);
            this.fConnectBoxNicknames.Name = "fConnectBoxNicknames";
            this.fConnectBoxNicknames.Size = new System.Drawing.Size(173, 20);
            this.fConnectBoxNicknames.TabIndex = 10;
            // 
            // fConnectLblNicks
            // 
            this.fConnectLblNicks.AutoSize = true;
            this.fConnectLblNicks.Location = new System.Drawing.Point(26, 88);
            this.fConnectLblNicks.Name = "fConnectLblNicks";
            this.fConnectLblNicks.Size = new System.Drawing.Size(63, 13);
            this.fConnectLblNicks.TabIndex = 9;
            this.fConnectLblNicks.Text = "Nicknames:";
            // 
            // fConnectLblUserName
            // 
            this.fConnectLblUserName.AutoSize = true;
            this.fConnectLblUserName.Location = new System.Drawing.Point(25, 30);
            this.fConnectLblUserName.Name = "fConnectLblUserName";
            this.fConnectLblUserName.Size = new System.Drawing.Size(58, 13);
            this.fConnectLblUserName.TabIndex = 5;
            this.fConnectLblUserName.Text = "Username:";
            // 
            // fConnectBoxRealName
            // 
            this.fConnectBoxRealName.Location = new System.Drawing.Point(94, 56);
            this.fConnectBoxRealName.Name = "fConnectBoxRealName";
            this.fConnectBoxRealName.Size = new System.Drawing.Size(173, 20);
            this.fConnectBoxRealName.TabIndex = 8;
            // 
            // fConnectBoxUserName
            // 
            this.fConnectBoxUserName.Location = new System.Drawing.Point(94, 27);
            this.fConnectBoxUserName.Name = "fConnectBoxUserName";
            this.fConnectBoxUserName.Size = new System.Drawing.Size(173, 20);
            this.fConnectBoxUserName.TabIndex = 7;
            // 
            // fConnectLblRealName
            // 
            this.fConnectLblRealName.AutoSize = true;
            this.fConnectLblRealName.Location = new System.Drawing.Point(17, 59);
            this.fConnectLblRealName.Name = "fConnectLblRealName";
            this.fConnectLblRealName.Size = new System.Drawing.Size(61, 13);
            this.fConnectLblRealName.TabIndex = 6;
            this.fConnectLblRealName.Text = "Real name:";
            // 
            // fConnectCheckBoxData
            // 
            this.fConnectCheckBoxData.AutoSize = true;
            this.fConnectCheckBoxData.Location = new System.Drawing.Point(12, 278);
            this.fConnectCheckBoxData.Name = "fConnectCheckBoxData";
            this.fConnectCheckBoxData.Size = new System.Drawing.Size(75, 17);
            this.fConnectCheckBoxData.TabIndex = 7;
            this.fConnectCheckBoxData.Text = "Save data";
            this.fConnectCheckBoxData.UseVisualStyleBackColor = true;
            // 
            // fConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 311);
            this.Controls.Add(this.fConnectCheckBoxData);
            this.Controls.Add(this.fConnectGrpLogin);
            this.Controls.Add(this.fConnectGrpServer);
            this.Controls.Add(this.fConnectBtnConnect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fConnect";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fConnect";
            this.Load += new System.EventHandler(this.fConnect_Load);
            this.fConnectGrpServer.ResumeLayout(false);
            this.fConnectGrpServer.PerformLayout();
            this.fConnectGrpLogin.ResumeLayout(false);
            this.fConnectGrpLogin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button fConnectBtnConnect;
        private System.Windows.Forms.Label fConnectLblAddress;
        private System.Windows.Forms.Label fConnectLblPort;
        private System.Windows.Forms.TextBox fConnectBoxAddress;
        private System.Windows.Forms.TextBox fConnectBoxPort;
        private System.Windows.Forms.GroupBox fConnectGrpServer;
        private System.Windows.Forms.GroupBox fConnectGrpLogin;
        private System.Windows.Forms.Label fConnectLblUserName;
        private System.Windows.Forms.TextBox fConnectBoxRealName;
        private System.Windows.Forms.TextBox fConnectBoxUserName;
        private System.Windows.Forms.Label fConnectLblRealName;
        private System.Windows.Forms.TextBox fConnectBoxNicknames;
        private System.Windows.Forms.Label fConnectLblNicks;
        private System.Windows.Forms.Label fConnectLblHint;
        private System.Windows.Forms.CheckBox fConnectCheckBoxData;
    }
}