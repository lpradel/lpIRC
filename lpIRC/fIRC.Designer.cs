namespace lpIRC
{
    partial class fIRC
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
            this.fIRCChatBox = new System.Windows.Forms.RichTextBox();
            this.fIRCChannels = new System.Windows.Forms.ListBox();
            this.fIRCSendBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // fIRCChatBox
            // 
            this.fIRCChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fIRCChatBox.Location = new System.Drawing.Point(0, 0);
            this.fIRCChatBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.fIRCChatBox.Name = "fIRCChatBox";
            this.fIRCChatBox.ReadOnly = true;
            this.fIRCChatBox.Size = new System.Drawing.Size(526, 324);
            this.fIRCChatBox.TabIndex = 0;
            this.fIRCChatBox.Text = "";
            // 
            // fIRCChannels
            // 
            this.fIRCChannels.Dock = System.Windows.Forms.DockStyle.Right;
            this.fIRCChannels.FormattingEnabled = true;
            this.fIRCChannels.Location = new System.Drawing.Point(526, 0);
            this.fIRCChannels.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.fIRCChannels.Name = "fIRCChannels";
            this.fIRCChannels.Size = new System.Drawing.Size(131, 324);
            this.fIRCChannels.Sorted = true;
            this.fIRCChannels.TabIndex = 1;
            // 
            // fIRCSendBox
            // 
            this.fIRCSendBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fIRCSendBox.Location = new System.Drawing.Point(0, 324);
            this.fIRCSendBox.Name = "fIRCSendBox";
            this.fIRCSendBox.Size = new System.Drawing.Size(657, 20);
            this.fIRCSendBox.TabIndex = 2;
            this.fIRCSendBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fIRCSendBox_KeyPress);
            // 
            // fIRC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 344);
            this.Controls.Add(this.fIRCChatBox);
            this.Controls.Add(this.fIRCChannels);
            this.Controls.Add(this.fIRCSendBox);
            this.Name = "fIRC";
            this.Text = "fIRC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox fIRCChatBox;
        private System.Windows.Forms.ListBox fIRCChannels;
        private System.Windows.Forms.TextBox fIRCSendBox;
    }
}