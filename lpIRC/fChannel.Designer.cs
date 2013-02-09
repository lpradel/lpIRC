namespace lpIRC
{
    partial class fChannel
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
            this.fChannelChatBox = new System.Windows.Forms.RichTextBox();
            this.fChannelUsers = new System.Windows.Forms.ListBox();
            this.fChannelSendBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // fChannelChatBox
            // 
            this.fChannelChatBox.BackColor = System.Drawing.SystemColors.Control;
            this.fChannelChatBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fChannelChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fChannelChatBox.Location = new System.Drawing.Point(0, 0);
            this.fChannelChatBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.fChannelChatBox.Name = "fChannelChatBox";
            this.fChannelChatBox.ReadOnly = true;
            this.fChannelChatBox.Size = new System.Drawing.Size(518, 304);
            this.fChannelChatBox.TabIndex = 3;
            this.fChannelChatBox.Text = "";
            // 
            // fChannelUsers
            // 
            this.fChannelUsers.Dock = System.Windows.Forms.DockStyle.Right;
            this.fChannelUsers.FormattingEnabled = true;
            this.fChannelUsers.Location = new System.Drawing.Point(518, 0);
            this.fChannelUsers.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.fChannelUsers.Name = "fChannelUsers";
            this.fChannelUsers.Size = new System.Drawing.Size(131, 304);
            this.fChannelUsers.Sorted = true;
            this.fChannelUsers.TabIndex = 4;
            // 
            // fChannelSendBox
            // 
            this.fChannelSendBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fChannelSendBox.Location = new System.Drawing.Point(0, 304);
            this.fChannelSendBox.Name = "fChannelSendBox";
            this.fChannelSendBox.Size = new System.Drawing.Size(649, 20);
            this.fChannelSendBox.TabIndex = 5;
            this.fChannelSendBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fChannelSendBox_KeyPress);
            // 
            // fChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 324);
            this.Controls.Add(this.fChannelChatBox);
            this.Controls.Add(this.fChannelUsers);
            this.Controls.Add(this.fChannelSendBox);
            this.Name = "fChannel";
            this.Text = "fChannel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fChannel_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox fChannelChatBox;
        private System.Windows.Forms.ListBox fChannelUsers;
        private System.Windows.Forms.TextBox fChannelSendBox;
    }
}