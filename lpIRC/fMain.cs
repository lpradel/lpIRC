using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lpIRC
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();

            // set window-title
            this.Text = "lpIRC - " + Version();
        }

        private void fMainMenuBtnClose_Click(object sender, EventArgs e)
        {
            // TODO: close connection and perform cleanup
            // ...
            this.Dispose();
        }

        private void fMainMenuBtnNewCon_Click(object sender, EventArgs e)
        {
            // open connect-form
            fConnect wndConnect = new fConnect();
            wndConnect.MdiParent = this;
            wndConnect.Show();

            /*
            // build server
            IRCServer server = new IRCServer("irc.quakenet.org", "6667");

            // build login
            IRCLogInDetails login = new IRCLogInDetails("test-username", "test-realname", "test-nick1 test-nick2");

            // setup IRC-form
            fIRC wndIRC = new fIRC(server, login);
            wndIRC.MdiParent = this;
            wndIRC.Show();
             * */
        }

        /// <summary>
        /// Returns the version number of current assembly.
        /// </summary>
        /// <returns>The version number of the current assembly.</returns>
        private string Version()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
