using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lpIRC
{
    public partial class fChannel : Form
    {
        #region "Private members"
        private IRCManager manager;
        private IRCConnection connection;
        private IRCChannel channel;
        private bool isJoined = true;

        // Event-Handlers
        private IRCChannel.UserStatusChangeEventHandler channelUserStatusChanged;
        private IRCConnection.MessageEventHandler channelGotMessage;
        private IRCConnection.MessageEventHandler channelSentMessage;
        private IRCConnection.MessageEventHandler channelTopicChanged;
        private IRCConnection.MessageEventHandler channelModesChanged;
        private IRCConnection.MessageEventHandler channelNamesChanged;
        private IRCConnection.MessageEventHandler channelLocalUserLeft;

        private IRCManager.ChannelEventHandler managerKickedFromChannel;
        #endregion

        #region "Constructor"
        public fChannel(IRCChannel pChannel, IRCManager pManager)
        {
            InitializeComponent();

            // plug into Manager from KickedFromChannel
            manager = pManager;
            managerKickedFromChannel = new IRCManager.ChannelEventHandler(ManagerKickedFromChannel);
            pManager.KickedFromChannel += managerKickedFromChannel;

            // setup
            SetChannel(pChannel);
        }
        #endregion

        #region "fChannel-Form-Event-Handlers"
        private void fChannelSendBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // retrieve text
                String text = fChannelSendBox.Text;

                // clear box
                fChannelSendBox.Clear();

                // process text
                if (text.StartsWith("/"))
                    connection.SendMessage(text.Substring(1));
                else
                    channel.SendMessageToChannel(text);

                e.Handled = true;   // no annoying sound
            }
        }

        private void fChannel_FormClosed(object sender, FormClosedEventArgs e)
        {
            // perform cleanup
            DisposeChannelEventHandlers();
            if (isJoined)
            {
                channel.Part();
                channel.Dispose();
            }
        }
        #endregion

        #region "IRCChannel-Event-Handlers"
        private void ChannelUserStatusChanged(object sender, IRCChannel.UserStatusChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ChannelUserStatusChanged(sender, e); }));
                return;
            }

            // retrieve user in question from list and update
            int index = fChannelUsers.Items.IndexOf(e.NewUser);
            if (index >= 0)
                fChannelUsers.Items[index] = e.NewUser;

            // printing message
            if (e.Quit)
            {
                string name = e.NewUser.NicknameInChannel;
                Log(name + " QUIT: " + e.Message.Body, Color.DarkRed);
            }
            else if (e.Left)
            {
                string name = e.NewUser.NicknameInChannel;
                Log(name + " hat den Channel verlassen.", Color.DarkRed);
            }
            else if (e.Message.MessageTypeID.Equals(IRCConstants.MSG_NICK, StringComparison.OrdinalIgnoreCase))
            {
                Log(e.Message.SenderNode.Nickname + " heißt jetzt " + e.Message.Body + ".", Color.DarkRed);
            }
            else
            {   // everything else
                Log(e.Message.ToString(), Color.DarkRed);
            }
            
            // process depending on status change
            if (e.Left || e.Quit || e.Kicked)
                fChannelUsers.Items.Remove(e.NewUser);
            else if (e.Joined)
                fChannelUsers.Items.Add(e.NewUser);        
        }

        private void ChannelGotMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            if (e.Message.IsSenderLocal)
            {
                Log(e.Message.ToShortString(), Color.DarkGreen);
            }
            else
            {
                Log(e.Message.ToShortString(), Color.Black);
            }
        }

        private void ChannelSentMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            Log(e.Message.ToShortString(), Color.Green);
        }

        private void ChannelTopicChanged(object sender, IRCConnection.MessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ChannelTopicChanged(sender, e); }));
                return;
            }

            Log(e.Message.ToString(), Color.DarkRed);

            // set window title
            this.Text = channel.ChannelName + " - " + channel.Topic;
        }

        private void ChannelModesChanged(object sender, IRCConnection.MessageEventArgs e)
        {
            Log(e.Message.ToString(), Color.DarkRed);
        }

        private void ChannelNamesChanged(object sender, IRCConnection.MessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ChannelNamesChanged(sender, e); }));
                return;
            }

            Log(e.Message.ToString(), Color.DarkRed);

            // update user-list
            fChannelUsers.Items.Clear();
            foreach (DictionaryEntry d in channel.Users)
            {
                fChannelUsers.Items.Add(d.Value);
            }
        }

        private void ChannelLocalUserLeft(object sender, IRCConnection.MessageEventArgs e)
        {
            isJoined = false;   // we left
            this.Close();       // cleanup
        }
        #endregion

        #region "IRCManager-Event-Handlers"
        private void ManagerKickedFromChannel(object sender, IRCManager.ChannelEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { ManagerKickedFromChannel(sender, e); }));
                return;
            }

            if (e.Channel.ChannelName.ToLower().Equals(channel.ChannelName.ToLower()))
            {   // we were kicked from the channel
                // perform cleanup
                DisposeChannelEventHandlers();
                if (isJoined)
                {
                    channel.Dispose();
                }
                isJoined = false;
                this.Close();
            }
        }
        #endregion

        #region "Public methods"
        public void DisposeChannelEventHandlers()
        {
            // unplug event-handlers from IRCChannel
            channel.UserStatusChanged -= channelUserStatusChanged;
            channel.GotMessage -= channelGotMessage;
            channel.SentMessage -= channelGotMessage;
            channel.TopicChanged -= channelTopicChanged;
            channel.ModesChanged -= channelModesChanged;
            channel.NamesChanged -= channelNamesChanged;
            channel.LocalUserLeftChannel -= channelLocalUserLeft;

            // unplug event-handlers from IRCManager
            manager.KickedFromChannel -= managerKickedFromChannel;
        }
        #endregion

        #region "Getters"
        public string GetChannelName()
        {
            return channel.ChannelName;
        }
        #endregion

        #region "Private methods"
        private void SetChannel(IRCChannel pChannel)
        {
            this.channel = pChannel;
            this.connection = channel.Connection;

            // setup user list
            fChannelUsers.Items.Clear();
            foreach (DictionaryEntry d in channel.Users)
            {
                fChannelUsers.Items.Add(d.Value);
            }

            // set window title
            this.Text = channel.ChannelName + " - " + channel.Topic;

            // setup event-handlers
            channelUserStatusChanged = new IRCChannel.UserStatusChangeEventHandler(ChannelUserStatusChanged);
            channelGotMessage = new IRCConnection.MessageEventHandler(ChannelGotMessage);
            channelSentMessage = new IRCConnection.MessageEventHandler(ChannelSentMessage);
            channelTopicChanged = new IRCConnection.MessageEventHandler(ChannelTopicChanged);
            channelModesChanged = new IRCConnection.MessageEventHandler(ChannelModesChanged);
            channelNamesChanged = new IRCConnection.MessageEventHandler(ChannelNamesChanged);
            channelLocalUserLeft = new IRCConnection.MessageEventHandler(ChannelLocalUserLeft);

            // plug-in to channel via event-handlers
            channel.UserStatusChanged += channelUserStatusChanged;
            channel.GotMessage += channelGotMessage;
            channel.SentMessage += channelGotMessage;
            channel.TopicChanged += channelTopicChanged;
            channel.ModesChanged += channelModesChanged;
            channel.NamesChanged += channelNamesChanged;
            channel.LocalUserLeftChannel += channelLocalUserLeft;

        }

        private void Log(string pText, System.Drawing.Color pColor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate { Log(pText, pColor); }));
                return;
            }

            // output text
            fChannelChatBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + pText + "\n", pColor);
            // autoscroll
            fChannelChatBox.Select(fChannelChatBox.Text.Length, 0);
            fChannelChatBox.ScrollToCaret();
        }
        #endregion
    }
}
