using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace lpIRC
{
    public partial class fIRC : Form
    {
        #region "Private members"
        IRCConnection connection = new IRCConnection();
        IRCManager manager;
        IRCServer server;
        IRCLogInDetails login;
        #endregion

        #region "Constructor"
        public fIRC(IRCServer pServer, IRCLogInDetails pLogin)
        {
            InitializeComponent();

            // set window-title
            this.Text = pServer.Address;

            // plug-in to connection via event-handlers
            connection.Connected += new EventHandler(ConnectionConnected);
            connection.ConnectionClosed += new EventHandler(ConnectionDisconnected);
            connection.GotMessage += new IRCConnection.MessageEventHandler(ConnectionGotMessage);
            connection.SentMessage += new IRCConnection.MessageEventHandler(ConnectionSentMessage);
            connection.NickChanged += new EventHandler(ConnectionNickChanged);
            connection.ConnectionSocketError += new ClientSocket.ExceptionEventHandler(ConnectionError);

            // setup
            server = pServer;
            login = pLogin;

            // build IRCManager
            manager = new IRCManager(connection, server, login);

            // plug-in to manager via event-handlers
            manager.JoinedChannel += new IRCManager.ChannelEventHandler(ManagerJoinedChannel);
            manager.LeftChannel += new IRCManager.ChannelEventHandler(ManagerLeftChannel);
            manager.KickedFromChannel += new IRCManager.ChannelEventHandler(ManagerKickedFromChannel);
            manager.GotPrivateMessage += new IRCConnection.MessageEventHandler(ManagerGotPrivateMessage);

            // connect to server
            manager.Connect();
        }
        #endregion

        #region "fIRC-Form-Event-Handlers"
        private void fIRCSendBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // retrieve text
                String text = fIRCSendBox.Text;

                // clear box
                fIRCSendBox.Clear();

                // process text
                if (text.StartsWith("/"))
                    connection.SendMessage(text.Substring(1));
                else
                    connection.SendMessage(text);

                e.Handled = true;   // no annoying sound
            }
        }
        #endregion

        #region "IRCConnection-Event-Handlers"
        private void ConnectionConnected(object sender, EventArgs e)
        {
            Log("Verbunden mit " + server.Address + "...", Color.DarkRed);
        }

        private void ConnectionDisconnected(object sender, EventArgs e)
        {
            Log("Verbindung zum Server getrennt!", Color.DarkRed);
        }

        private void ConnectionGotMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            Log(msg.ToString(), Color.Blue);
        }

        private void ConnectionSentMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            // TODO: fix
            Log(msg.ToString(), Color.DarkGreen);
        }

        private void ConnectionNickChanged(object sender, EventArgs e)
        {
            Log("Neuer Nickname ist: " + manager.Connection.Nickname, Color.DarkRed);
        }

        private void ConnectionError(object sender, ClientSocket.ExceptionEventArgs e)
        {
            Log(e.CatchedException.Message, Color.Red);
        }
        #endregion

        #region "IRCManager-Event-Handlers"
        private void ManagerJoinedChannel(object sender, IRCManager.ChannelEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ManagerJoinedChannel(sender, e); }));
                return;
            }

            // setup channel-frame
            fChannel wndChannel = new fChannel(e.Channel, manager);
            Form mdiParent = this.MdiParent;
            wndChannel.MdiParent = mdiParent;
            wndChannel.Show();

            Log("Du hast den Channel" + e.Channel.ChannelName + " betreten!", Color.DarkRed);

            // update channels-list
            if (!fIRCChannels.Items.Contains(e.Channel))
            {
                fIRCChannels.Items.Add(e.Channel);
            }
            // todo: fix
            // e.Channel.TopicChanged += new IRCConnection.MessageEventHandler(ChannelTopicChanged);
        }

        private void ManagerLeftChannel(object sender, IRCManager.ChannelEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ManagerLeftChannel(sender, e); }));
                return;
            }

            fIRCChannels.Items.Remove(e.Channel);
            Log("Du hast den Channel " + e.Channel.ChannelName + " verlassen!", Color.DarkRed);
        }

        private void ManagerKickedFromChannel(object sender, IRCManager.ChannelEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ManagerKickedFromChannel(sender, e); }));
                return;
            }

            fIRCChannels.Items.Remove(e.Channel);
            Log("Du wurdest aus dem Channel " + e.Channel.ChannelName + " gekickt!", Color.DarkRed);
        }

        private void ManagerGotPrivateMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            // todo: fill..
        }
        #endregion

        #region "Public methods and members"
        public Boolean IsConnected()
        {
            return connection.IsConnected();
        }
        #endregion

        #region "Private methods"
        private void Log(string pText, System.Drawing.Color pColor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { Log(pText, pColor); }));
                return;
            }

            // output text
            fIRCChatBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + pText + "\n", pColor);
            // autoscroll
            fIRCChatBox.Select(fIRCChatBox.Text.Length, 0);
            fIRCChatBox.ScrollToCaret();
        }
        #endregion
    }
}
