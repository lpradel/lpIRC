namespace lpIRC
{
    partial class fMain
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
            this.fMainMenu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fMainMenuBtnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.fMainMenuBtnNewCon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.fMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // fMainMenu
            // 
            this.fMainMenu.BackColor = System.Drawing.SystemColors.MenuBar;
            this.fMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.toolStripMenuItem3});
            this.fMainMenu.Location = new System.Drawing.Point(0, 0);
            this.fMainMenu.MdiWindowListItem = this.toolStripMenuItem4;
            this.fMainMenu.Name = "fMainMenu";
            this.fMainMenu.Size = new System.Drawing.Size(868, 24);
            this.fMainMenu.TabIndex = 0;
            this.fMainMenu.Text = "fMainMenu";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fMainMenuBtnClose});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // fMainMenuBtnClose
            // 
            this.fMainMenuBtnClose.Name = "fMainMenuBtnClose";
            this.fMainMenuBtnClose.Size = new System.Drawing.Size(152, 22);
            this.fMainMenuBtnClose.Text = "Exit";
            this.fMainMenuBtnClose.Click += new System.EventHandler(this.fMainMenuBtnClose_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fMainMenuBtnNewCon});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(81, 20);
            this.toolStripMenuItem2.Text = "&Connection";
            // 
            // fMainMenuBtnNewCon
            // 
            this.fMainMenuBtnNewCon.Name = "fMainMenuBtnNewCon";
            this.fMainMenuBtnNewCon.Size = new System.Drawing.Size(152, 22);
            this.fMainMenuBtnNewCon.Text = "&Connect...";
            this.fMainMenuBtnNewCon.Click += new System.EventHandler(this.fMainMenuBtnNewCon_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(63, 20);
            this.toolStripMenuItem4.Text = "&Window";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItem3.Text = "&Help";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 415);
            this.Controls.Add(this.fMainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.fMainMenu;
            this.Name = "fMain";
            this.Text = "lpIRC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.fMainMenu.ResumeLayout(false);
            this.fMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip fMainMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fMainMenuBtnClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem fMainMenuBtnNewCon;
    }
}

