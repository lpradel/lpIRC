using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace lpIRC
{
    /// <summary>
    /// Handles everything regarding an IRCConnection including channels
    /// and nicks, messages, ...
    /// </summary>
    public class IRCManager
    {
        #region "Private members"
        private IRCConnection connection;
        private IRCServer server;
        private IRCLogInDetails logininfo;
        private Hashtable channels = new Hashtable();
        private Hashtable privatechats = new Hashtable();

        // Event-Handler for Events from IRCConnection
        private EventHandler connectionMade;
        private EventHandler connectionClosed;
        private IRCConnection.MessageEventHandler connectionGotMessage;
        private IRCConnection.MessageEventHandler connectionSentMessage;
        #endregion

        #region "Channel Events"
        public class ChannelEventArgs : EventArgs
        {
            private IRCChannel channel;
            public ChannelEventArgs(IRCChannel pChannel)
            {
                channel = pChannel;
            }

            public IRCChannel Channel
            {
                get { return channel; }
                set { channel = value; }
            }
        }

        public delegate void ChannelEventHandler(object sender, ChannelEventArgs e);

        public event ChannelEventHandler JoinedChannel;
        public event ChannelEventHandler LeftChannel;
        public event ChannelEventHandler KickedFromChannel;
        public event IRCConnection.MessageEventHandler GotPrivateMessage;
        #endregion

        #region "IRCConnection-Event-Handlers"
        private void ConnectionMade(object sender, EventArgs e)
        {
            connection.LogIn(logininfo.Username, logininfo.Realname, logininfo.Nicknames[0].ToString());
        }

        private void ConnectionClosed(object sender, EventArgs e)
        {
            // todo: fill
        }

        private void ConnectionGotMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            // process depending on message type
            switch (msg.MessageTypeID.ToUpper())
            {
                case IRCConstants.MSG_JOIN:
                    {
                        if (string.Compare(msg.SenderNode.Nickname, connection.LocalIRCNode.Nickname, true) == 0)
                        {   // check if WE sent the message
                            if (msg.Parameters.Count <= 0)
                                return;

                            string channelName = msg.Parameters[0].ToString().ToLower();    // extract name of the channel
                            /*
                            string channelName = msg.Body.ToLower();    // extract name of the channel
                             * */

                            // if we already are in the channel, we can ignore the message
                            if (channels.ContainsKey(channelName))
                                return;

                            // otherwise, we join the channel and add it to our channels
                            IRCChannel channel = JoinChannel(channelName);
                            if (JoinedChannel != null)
                                JoinedChannel(this, new ChannelEventArgs(channel));
                        }
                        break;
                    }

                case IRCConstants.MSG_PART:
                    {
                        if (string.Compare(msg.SenderNode.Nickname, connection.LocalIRCNode.Nickname, true) == 0)
                        {   // check if WE sent the message
                            string channelName = msg.Parameters[0].ToString().ToLower();    // extract name of the channel

                            // if it's not for this channel, we can ignore
                            if (!channels.ContainsKey(channelName))
                                return;

                            // otherwise, retrieve channel and leave it
                            IRCChannel channel = channels[channelName] as IRCChannel;
                            channel.Dispose();
                            channels.Remove(channelName);
                            if (LeftChannel != null)
                                LeftChannel(this, new ChannelEventArgs(channel));
                        }
                        break;
                    }

                case IRCConstants.MSG_KICK:
                    {
                        if (    (msg.Parameters.Count > 1) &&
                                (string.Compare(msg.Parameters[1].ToString(), connection.Nickname, true) == 0))
                        {
                            string channelName = msg.Parameters[0].ToString().ToLower();    // extract name of the channel

                            // otherwise, retrieve channel and leave it
                            IRCChannel channel = channels[channelName] as IRCChannel;
                            channel.Dispose();
                            channels.Remove(channelName);
                            if (KickedFromChannel != null)
                                KickedFromChannel(this, new ChannelEventArgs(channel));
                        }
                        break;
                    }

                case IRCConstants.MSG_PRIVMSG:
                    {
                        if (    (msg.Parameters.Count > 0) &&   // syntax
                                (string.Compare(msg.Parameters[0].ToString(), connection.Nickname, true) == 0) &&   // PM to user
                                (GotPrivateMessage != null) &&  // trigger neccessary
                                (!privatechats.ContainsKey(msg.SenderNode.Nickname.ToLower()))) // PM-Chat does not exist yet
                        {
                            GotPrivateMessage(this, new IRCConnection.MessageEventArgs(msg));
                        }
                        break;
                    }

                case IRCConstants.MSG_403:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // if channel exists, retrieve and leave
                        IRCChannel channel = channels[msg.Parameters[1].ToString()] as IRCChannel;
                        if (channel != null)
                            channel.Dispose();
                        channels.Remove(msg.Parameters[1].ToString());
                        break;
                    }

                case IRCConstants.MSG_473:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // if channel exists, retrieve and leave
                        IRCChannel channel = channels[msg.Parameters[1].ToString()] as IRCChannel;
                        if (channel != null)
                        {
                            channel.Dispose();
                            channels.Remove(channel.ChannelName.ToLower());
                        }
                        break;
                    }

                case IRCConstants.MSG_475:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // if channel exists, retrieve and leave
                        IRCChannel channel = channels[msg.Parameters[1].ToString()] as IRCChannel;
                        if (channel != null)
                        {
                            channel.Dispose();
                            channels.Remove(channel.ChannelName.ToLower());
                        }
                        break;
                    }
            }
        }

        private void ConnectionSentMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            // process depending on message type
            switch (msg.MessageTypeID.ToUpper())
            {
                case IRCConstants.MSG_JOIN:
                    {
                        if (msg.Parameters.Count <= 0)
                            return;

                        string channelName = msg.Parameters[0].ToString().ToLower();    // extract name of the channel
                        // if we already are in channel, we can ignore
                        if (channels.ContainsKey(channelName))
                            return;

                        // join channel and notify listeners
                        IRCChannel channel = JoinChannel(channelName);
                        if (JoinedChannel != null)
                            JoinedChannel(this, new ChannelEventArgs(channel));
                        break;
                    }

                case IRCConstants.MSG_PART:
                    {
                        if (msg.Parameters.Count <= 0)
                            return;

                        string channelName = msg.Parameters[0].ToString().ToLower();    // extract name of the channel
                        // if we are not in the concerned channel, we can ignore
                        if (!channels.ContainsKey(channelName))
                            return;

                        // retrieve channel and leave
                        IRCChannel channel = channels[channelName] as IRCChannel;
                        channel.Dispose();
                        channels.Remove(channelName);
                        // notify listeners, if neccessary
                        if (LeftChannel != null)
                            LeftChannel(this, new ChannelEventArgs(channel));
                        break;
                    }
            }
        }
        #endregion

        #region "Constructor"
        public IRCManager(IRCConnection pConnection, IRCServer pServer, IRCLogInDetails pLogIn)
        {
            connection = pConnection;
            server = pServer;
            logininfo = pLogIn;

            // plug into IRCConnection's events
            connectionMade = new EventHandler(ConnectionMade);
            connection.Connected += connectionMade;
            connectionClosed = new EventHandler(ConnectionClosed);
            connection.ConnectionClosed += connectionClosed;
            connectionGotMessage = new IRCConnection.MessageEventHandler(ConnectionGotMessage);
            connection.GotMessage += connectionGotMessage;
            connectionSentMessage = new IRCConnection.MessageEventHandler(ConnectionSentMessage);
        }
        #endregion

        #region "Public methods"
        /// <summary>
        /// Removes all event handlers.
        /// </summary>
        public void Dispose()
        {
            connection.Connected -= connectionMade;
            connection.ConnectionClosed -= connectionClosed;
            connection.GotMessage -= connectionGotMessage;
            connection.SentMessage -= connectionGotMessage;
        }

        /// <summary>
        /// Initiates connection using the given Server- and Login-Info.
        /// </summary>
        public void Connect()
        {
            connection.Connect(server);
        }

        public bool IsInChannel(string pChannelname)
        {
            return channels.ContainsValue(pChannelname.ToLower());
        }

        public IRCChannel JoinChannel(string pChannelname)
        {
            // create channel-object and return result
            IRCChannel channel = new IRCChannel(Connection, pChannelname);
            channels.Add(pChannelname.ToLower(), channel);
            return channel;
        }

        public override string ToString()
        {
            return connection.ToString();
        }
        #endregion

        #region "Getters"
        public IRCConnection Connection
        {
            get { return connection; }
        }

        public IRCServer Server
        {
            get { return server; }
        }

        public Hashtable JoinedChannels
        {
            get { return channels; }
        }
        #endregion
    }
}
