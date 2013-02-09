using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace lpIRC
{
    /// <summary>
    /// Holds all info includings users on an IRCChannel.
    /// </summary>
    public class IRCChannel
    {
        #region "Private members"
        private string channelName;
        private string topic;
        private Hashtable users = new Hashtable();
        private Hashtable channelModes = new Hashtable();
        private IRCConnection connection;

        // IRCConnection-Event-Handlers
        private IRCConnection.MessageEventHandler connectionGotMessage;
        private IRCConnection.MessageEventHandler connectionSentMessage;
        #endregion

        #region "Channel-Events"
        /// <summary>
        /// Used to capsulate info on the change of a user's status.
        /// </summary>
        public class UserStatusChangeEventArgs : EventArgs
        {
            private IRCUser newUser;
            private IRCMessage message;
            private bool joined, left, kicked, quit;

            public UserStatusChangeEventArgs(IRCUser pUser, IRCMessage pMsg, bool pJoined, bool pLeft, bool pKicked, bool pQuit)
            {
                newUser = pUser;
                message = pMsg;
                joined = pJoined;
                left = pLeft;
                kicked = pKicked;
                quit = pQuit;
            }

            #region "Getter"
            public IRCUser NewUser { get { return newUser; } }
            public IRCMessage Message { get { return message; } }
            public bool Joined { get { return joined; } }
            public bool Left { get { return left; } }
            public bool Kicked { get { return kicked; } }
            public bool Quit { get { return quit; } }
            #endregion
        }

        public delegate void UserStatusChangeEventHandler(object sender, UserStatusChangeEventArgs e);

        // Channel-Events
        public event IRCConnection.MessageEventHandler NamesChanged; // triggered when user-list changes
        public event IRCConnection.MessageEventHandler TopicChanged; // triggered when channel-topic changes
        public event UserStatusChangeEventHandler UserStatusChanged; // triggered when a user's status changes

        public event IRCConnection.MessageEventHandler GotMessage;
        public event IRCConnection.MessageEventHandler SentMessage;
        public event IRCConnection.MessageEventHandler ModesChanged;
        public event IRCConnection.MessageEventHandler LocalUserLeftChannel;
        #endregion

        #region "Constructor"
        public IRCChannel(IRCConnection pConnection, string pChannelName)
        {
            connection = pConnection;
            channelName = pChannelName;

            // plug into connection's events
            connectionGotMessage = new IRCConnection.MessageEventHandler(ConnectionGotMessage);
            connection.GotMessage += connectionGotMessage;
            connectionSentMessage = new IRCConnection.MessageEventHandler(ConnectionSentMessage);
            connection.SentMessage += connectionSentMessage;
        }
        #endregion

        #region "Private IRCUserStatus-Event-Handlers"
        private void UserJoined(IRCNode pUserNode, IRCMessage pMessage)
        {
            // check if we joined ourselves
            if (pUserNode.Nickname.ToLower() == connection.Nickname.ToLower())
                return;

            // check if already in channel-user-list
            if (IsUserInChannel(pUserNode.Nickname))
                return;

            // add as new user in channel
            IRCUser newUser = new IRCUser(this, pUserNode);
            users.Add(pUserNode.Nickname, newUser);
            if (UserStatusChanged != null)  // trigger event if necessary
                UserStatusChanged(this, new UserStatusChangeEventArgs(newUser, pMessage, true, false, false, false));
        }

        private void UserLeft(IRCNode pUserNode, IRCMessage pMessage)
        {
            if (!IsUserInChannel(pUserNode.Nickname))
                return;

            // Get user from user-table
            IRCUser user = (IRCUser)users[pUserNode.Nickname.ToLower()];

            // trigger event if necessary
            if (UserStatusChanged != null)
                UserStatusChanged(this, new UserStatusChangeEventArgs(user, pMessage, false, true, false, false));

            // remove user from channel
            users.Remove(pUserNode.Nickname.ToLower());
        }

        private void UserKicked(IRCUser pUser, IRCMessage pMessage)
        {
            if (!IsUserInChannel(pUser.Nickname))
                return;

            // trigger event if necessary
            if (users.ContainsValue(pUser.Nickname.ToLower()))
            {
                if (UserStatusChanged != null)
                    UserStatusChanged(this, new UserStatusChangeEventArgs(pUser, pMessage, false, false, true, false));
            }

            // remove user from channel
            users.Remove(pUser.Nickname.ToLower());
        }

        private void UserQuit(IRCNode pUserNode, IRCMessage pMessage)
        {
             // Get user from user-table
            IRCUser user = (IRCUser)users[pUserNode.Nickname.ToLower()];

            // trigger event if necessary
            if (UserStatusChanged != null)
                UserStatusChanged(this, new UserStatusChangeEventArgs(user, pMessage, false, false, false, true));

            // remove user from channel
            users.Remove(pUserNode.Nickname.ToLower());
        }

        private void UserVoice(string pNickName, bool pVoiced, IRCMessage pMessage)
        {
            if (!IsUserInChannel(pNickName))
                return;

            // Get user from user-table
            IRCUser user = (IRCUser)users[pNickName.ToLower()];

            // update
            user.HasVoice = pVoiced;

            // trigger event if necessary
            if (UserStatusChanged != null)
                UserStatusChanged(this, new UserStatusChangeEventArgs(user, pMessage, false, false, false, false));
        }

        private void UserOperator(string pNickName, bool pOperator, IRCMessage pMessage)
        {
            if (!IsUserInChannel(pNickName))
                return;

            // Get user from user-table
            IRCUser user = (IRCUser)users[pNickName.ToLower()];

            // update
            user.IsOperator = pOperator;

            // trigger event if necessary
            if (UserStatusChanged != null)
                UserStatusChanged(this, new UserStatusChangeEventArgs(user, pMessage, false, false, false, false));
        }

        private void UserNickChange(string pOldNickName, string pNewNickName, IRCMessage pMessage)
        {
            if (!IsUserInChannel(pOldNickName))
                return;

            // Get user from user-table
            IRCUser user = (IRCUser)users[pOldNickName.ToLower()];

            // update
            user.Nickname = pNewNickName;
            users.Remove(pOldNickName.ToLower());
            users.Add(pNewNickName.ToLower(), user);

            // trigger event if necessary
            if (UserStatusChanged != null)
                UserStatusChanged(this, new UserStatusChangeEventArgs(user, pMessage, false, false, false, false));
        }
        #endregion

        #region "Private IRCConnection-Events-Handlers"
        private void ConnectionSentMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            switch (msg.MessageTypeID.ToUpper())
            {
                case IRCConstants.MSG_PART:
                    if ((msg.Parameters.Count > 0) && (MatchesChannel(msg.Parameters[0].ToString())))
                    {
                        if (LocalUserLeftChannel != null)
                            LocalUserLeftChannel(this, new IRCConnection.MessageEventArgs(msg));
                    }
                    else if (string.Compare(msg.Body, ChannelName, true) == 0)
                    {
                        if (LocalUserLeftChannel != null)
                            LocalUserLeftChannel(this, new IRCConnection.MessageEventArgs(msg));
                    }
                    break;

                case IRCConstants.MSG_PRIVMSG:
                    if ((msg.Parameters.Count > 0) && (MatchesChannel(msg.Parameters[0].ToString())))
                    {
                        if (SentMessage != null)
                            SentMessage(this, e);
                    }
                    break;
            }
        }

        private void ConnectionGotMessage(object sender, IRCConnection.MessageEventArgs e)
        {
            IRCMessage msg = e.Message;

            switch (msg.MessageTypeID.ToUpper())
            {
                case IRCConstants.MSG_JOIN:
                    {
                        if (msg.Body.ToLower() == "" || msg.IsSenderLocal)
                            return;

                        // if the msg-body aka name of channel in JOIN <channame>
                        // matches this channel's name, trigger user joined channel
                        // event
                        string channame = msg.Body;
                        if (MatchesChannel(channame))
                        {
                            UserJoined(msg.SenderNode, msg);
                        }
                        break;
                    }

                case IRCConstants.MSG_PART:
                    {
                        if (msg.Parameters.Count <= 0 || msg.IsSenderLocal)
                            return;

                        // check if part parameter <channelname> is this channel's name
                        if (MatchesChannel(msg.Parameters[0].ToString()) || (string.Compare(msg.Body, ChannelName, true) == 0))
                        {
                            UserLeft(msg.SenderNode, msg);
                        }
                        break;
                    }

                case IRCConstants.MSG_QUIT:
                    {
                        // check if channel matches or if user was in channel-user-list
                        if (IsUserInChannel(msg.SenderNode.Nickname) || (string.Compare(msg.Body, ChannelName, true) == 0))
                        {
                            UserQuit(msg.SenderNode, msg);
                        }
                        break;
                    }

                case IRCConstants.MSG_KICK:
                    {
                        if (    msg.Parameters.Count > 1 && // necessary for correct KICK-command
                                MatchesChannel(msg.Parameters[0].ToString()) && // msg for this channel
                                users.ContainsValue(msg.Parameters[1].ToString())) // user is in channel
                        {
                            IRCUser u = (IRCUser)users[msg.Parameters[1].ToString().ToLower()];
                            UserKicked(u, msg);
                            // notify listeners if necessary
                            if (GotMessage != null)
                                GotMessage(this, e);
                        }
                        break;
                    }

                case IRCConstants.MSG_332:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // change topic if this channel is concerned
                        string chan = msg.Parameters[1].ToString();
                        if (MatchesChannel(chan))
                        {
                            Topic = msg.Body;
                            if (TopicChanged != null)
                                TopicChanged(this, new IRCConnection.MessageEventArgs(msg));
                        }
                        break;
                    }

                case IRCConstants.MSG_333:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // trigger GotMessage, if 333 was for this channel
                        string chan = msg.Parameters[1].ToString();
                        if (MatchesChannel(chan))
                        {
                            if (GotMessage != null)
                                GotMessage(this, new IRCConnection.MessageEventArgs(msg));
                        }
                        break;
                    }

                case IRCConstants.MSG_353:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // perform names-update for given users
                        string chan = msg.Parameters[2].ToString();
                        if (MatchesChannel(chan))
                            UpdateNames(msg.Body);
                        break;
                    }

                case IRCConstants.MSG_366:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // perform names-update for given users
                        string chan = msg.Parameters[1].ToString();
                        if (MatchesChannel(chan))
                        {
                            if (NamesChanged != null)
                                NamesChanged(this, new IRCConnection.MessageEventArgs(msg));
                        }
                        break;
                    }

                case IRCConstants.MSG_NICK:
                    {
                        UserNickChange(msg.SenderNode.Nickname, msg.Body, msg);
                        break;
                    }

                case IRCConstants.MSG_TOPIC:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        // if concerns this channel, update Topic
                        string chan = msg.Parameters[0].ToString();
                        if (MatchesChannel(chan))
                        {
                            Topic = msg.Body;
                            // inform listeners if necessary
                            if (TopicChanged != null)
                                TopicChanged(this, new IRCConnection.MessageEventArgs(msg));
                        }
                        break;
                    }

                case IRCConstants.MSG_PRIVMSG:
                    {
                        if (msg.Parameters.Count <= 0)
                            return;

                        // if for this channel, trigger GotMessage for listeners
                        string chan = msg.Parameters[0].ToString();
                        if (MatchesChannel(chan))
                        {
                            if (GotMessage != null)
                                GotMessage(this, e);
                        }
                        break;
                    }

                case IRCConstants.MSG_MODE:
                    {
                        if (msg.Parameters.Count <= 1)
                            return;

                        string chan = msg.Parameters[0].ToString();

                        if (!MatchesChannel(chan))
                            return;

                        string mode = msg.Parameters[1].ToString();
						bool positive = true;
						char firstmode = mode[1];
                        switch (firstmode)
                        {
                            case 'o':
                                {
                                    string nick = msg.Parameters[2].ToString();
                                    UserOperator(nick, true, msg);
                                    break;
                                }

                            case 'v':
                                {
                                    string nick = msg.Parameters[2].ToString();
                                    UserVoice(nick, true, msg);
                                    break;
                                }

                            default:
                                {
                                    for (int i = 0; i < mode.Length; i++)
                                    {
                                        if (mode[i] == '+')
                                            positive = true;
                                        else if (mode[i] == '-')
                                            positive = false;
                                        else
                                        {
                                            if (positive && !channelModes.ContainsKey(mode[i]))
                                                channelModes.Add(mode[i], null);
                                            else
                                                channelModes.Remove(mode[i]);
                                        }
                                    }
                                    string key = "", limit = "";
                                    if (msg.Parameters.Count > 2)
                                    {
                                        foreach (DictionaryEntry d in channelModes)
                                        {
                                            char m = (char)d.Key;
                                            switch (m)
                                            {
                                                case 'k':
                                                    {
                                                        key = msg.Parameters.Count > 3 ? msg.Parameters[3].ToString() : msg.Parameters[2].ToString();

                                                        break;
                                                    }
                                                case 'l':
                                                    {
                                                        limit = msg.Parameters[2].ToString();
                                                        break;
                                                    }
                                            }
                                        }
                                        if (key != "")
                                            channelModes['k'] = key;
                                        if (limit != "")
                                            channelModes['l'] = limit;
                                    }
								}
								if(ModesChanged!=null)
									ModesChanged(this,new IRCConnection.MessageEventArgs(msg));
								break;		
                                
                        }


                        break;
                    }
            }
        }
        #endregion

        #region "Public methods"
        public void SendMessageToChannel(string pMsg)
        {
            connection.SendMessage(IRCConstants.MSG_PRIVMSG + " " + ChannelName + " : " + pMsg);
        }

        public bool MatchesChannel(string pChannelName)
        {
            return (string.Compare(pChannelName, ChannelName, true) == 0);
        }

        public void Dispose()
        {
            // unplug from connection
            connection.GotMessage -= connectionGotMessage;
            connection.SentMessage -= connectionSentMessage;
        }

        public void UpdateNames(string pNames)
        {
            string[] names = pNames.Split(' '); // seperated by a space
            foreach (string s in names)
            {
                IRCUser user = new IRCUser(s, this);
                if (!IsUserInChannel(user.Nickname))
                {
                    users.Add(user.Nickname.ToLower(), user);
                }
            }
        }

        public void Part()
        {
            connection.SendMessage(IRCConstants.MSG_PART + " " + channelName);
        }

        public static bool IsChannelName(string pName)
        {
            string s = pName.Substring(0, 1);
            return (s == "#");  // ChannelNames start with '#'
        }

        public bool IsUserInChannel(string pNickname)
        {
            return (users.ContainsKey(pNickname.ToLower()));
        }

        public override string ToString()
        {
            return channelName;
        }
        #endregion

        #region "Getters"
        public string ChannelName { get { return channelName; } }
        public string Topic { get { return topic; } set { topic = value; } }
        public Hashtable Users { get { return users; } }
        public Hashtable ChannelModes { get { return channelModes; } }
        public IRCConnection Connection { get { return connection; } }
        #endregion
    }
}
