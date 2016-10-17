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
    public partial class fConnect : Form
    {
        #region "Private members"
        private IRCServer server;
        private IRCLogInDetails login;
        #endregion

        public fConnect()
        {
            InitializeComponent();

            // set window-title
            this.Text = "Connect...";

            // check if a profile exists
            if (Options.OptionsExist())
            {
                IRCServer server = Options.LoadServer();
                IRCLogInDetails login = Options.LoadLogIn();

                // check for errors
                if (server == null || login == null)
                {   // create new profile with default values
                    fConnectBoxAddress.Text = "irc.quakenet.org";
                    fConnectBoxPort.Text = "6667";
                    fConnectBoxUserName.Text = "test-username";
                    fConnectBoxRealName.Text = "test-realname";
                    fConnectBoxNicknames.Text = "test-nick1 test-nick2";
                    fConnectCheckBoxData.Checked = true;
                }
                else
                {   // load old profile
                    fConnectBoxAddress.Text = server.Address;
                    fConnectBoxPort.Text = server.Port;
                    fConnectBoxUserName.Text = login.Username;
                    fConnectBoxRealName.Text = login.Realname;

                    string nicks = "";
                    foreach (string n in login.Nicknames)
                        nicks = nicks + n + " ";
                    fConnectBoxNicknames.Text = nicks;

                    fConnectCheckBoxData.Checked = true;
                }
            }
        }

        private void fConnectBtnConnect_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { fConnectBtnConnect_Click(sender, e); }));
                return;
            }

            // Get input
            string address = fConnectBoxAddress.Text;
            string port = fConnectBoxPort.Text;
            string username = fConnectBoxUserName.Text;
            string realname = fConnectBoxRealName.Text;
            string nicknames = fConnectBoxNicknames.Text;

            // check input
            if (address == null || address.Trim() == "")
            {
                ErrorBox("Invalid server address!");
                return;
            }
            if (port == null || port.Trim() == "")
            {
                ErrorBox("Invalid port number!");
                return;
            }
            if (username == null || username.Trim() == "")
            {
                ErrorBox("Invalid username!");
                return;
            }
            if (realname == null || realname.Trim() == "")
            {
                ErrorBox("Invalid real name!");
                return;
            }
            if (nicknames == null || nicknames.Trim() == "")
            {
                ErrorBox("Invalid nickname!");
                return;
            }

            // here all input should be fine
            
            // build server
            server = new IRCServer(address, port);

            // build login
            login = new IRCLogInDetails(username, realname, nicknames);

            // save data, if checkbox is checked
            if (fConnectCheckBoxData.Checked)
            {
                Options.SaveOptions(server, login);
            }

            // hide me
            this.Hide();

            // setup IRC-form
            fIRC wndIRC = new fIRC(server, login);
            wndIRC.MdiParent = this.MdiParent;
            wndIRC.Show();

            // clean-up
            this.Close();
        }

        private void fConnectBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {   // make sure, only numbers in port-input
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch != 13)
            {
                e.Handled = true;
            }
        }

        private void ErrorBox(string text)
        {
            MessageBox.Show(text,
                            "Fehler!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1);
        }

        private void fConnect_Load(object sender, EventArgs e)
        {

        }

    }
}
