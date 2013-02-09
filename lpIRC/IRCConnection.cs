using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;

namespace lpIRC
{
    public class IRCConnection
    {
        #region "Private members"
        private ClientSocket socket = null;
        private IRCServer server = null;
        private IRCNode node = new IRCNode("");
        private string nick = "";
        private bool loggedIn = false;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IRCConnection()
        {
            Init();
        }

        /// <summary>
        /// Sets up a socket and the necessary handlers.
        /// </summary>
        public void Init()
        {
            socket = new ClientSocket();    // create a socket
            socket.ConnectSuccess += new EventHandler(SocketConnectedHandler);
            socket.Closed += new EventHandler(SocketClosedHandler);
            socket.ConnectionException += new ClientSocket.ExceptionEventHandler(SocketErrorHandler);
            socket.SendException += new ClientSocket.ExceptionEventHandler(SocketErrorHandler);
            socket.GeneralException += new ClientSocket.ExceptionEventHandler(SocketErrorHandler);
            socket.DataArrival += new EventHandler(DataArrivalHandler);
        }

        #region "Public methods"
        /// <summary>
        /// Attempts a connection to pServer. If it fails,
        /// the Exception-Event will be raised.
        /// </summary>
        /// <param name="pServer">The server to connect to.</param>
        public void Connect(IRCServer pServer)
        {
            server = pServer;
            // attempt Connection
            Socket.Connect(Server.Address, int.Parse(Server.Port));
        }

        /// <summary>
        /// Attempts to login to server with given parameters.
        /// </summary>
        /// <param name="pUserName"></param>
        /// <param name="pRealName"></param>
        /// <param name="pNickname"></param>
        public void LogIn(string pUserName, string pRealName, string pNickname)
        {
            node.Username = pUserName;
            node.Realname = pRealName;
            node.Nickname = pNickname;
            node.Hostname = socket.LocalHost;

            // send LogIn-Commands
            SendMessage("USER " + pUserName + " \"" + socket.LocalHost + "\" \"" + server.Address + "\" :" + pRealName);
            SendMessage("NICK :" + pNickname);
        }

        /// <summary>
        /// Attempts to change nickname to pNickname.
        /// </summary>
        /// <param name="pNickname">The new nickname.</param>
        public void ChangeNickname(string pNickname)
        {
            SendMessage("NICK " + pNickname);
        }

        /// <summary>
        /// Sends pMsg to the IRCServer, iff there is a connection.
        /// </summary>
        /// <param name="pMsg">The message or command to be sent to server.</param>
        public void SendMessage(string pMsg)
        {
            if (pMsg == "" || pMsg == null)
                return;

            // compose send
            IRCMessage msg = IRCMessage.Parse(this.LocalIRCNode.ToString() + " " + pMsg);
            msg.IsSenderLocal = true;   // we are sending

            CancelMessageEventArgs e = new CancelMessageEventArgs(msg);
            if (SendingMessage != null)
            {
                SendingMessage(this, e);    // transmission flag
            }
            if (!e.Cancel)
            {   // all good, no errors
                HandleMessage(msg);
                socket.SendTextAppendCrlf(pMsg);    // send on network-level
            }

        }
        #endregion

        #region "Delegated events"
        public class MessageEventArgs : EventArgs
        {
            protected IRCMessage msg = null;

            public MessageEventArgs(IRCMessage pMessage) { msg = pMessage; }
            public IRCMessage Message { get { return msg; } set { msg = value; } }
        }

        public class CancelMessageEventArgs : MessageEventArgs
        {
            private bool cancel = false;

            public CancelMessageEventArgs(IRCMessage message) : base(message) { }
            public bool Cancel { get { return cancel; } set { cancel = value; } }
        }

        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        public delegate void CancelMessageEventHandler(object sender, CancelMessageEventArgs e);
        public event MessageEventHandler GotMessage;
        public event MessageEventHandler SentMessage;
        public event CancelMessageEventHandler SendingMessage;
        public event ClientSocket.ExceptionEventHandler ConnectionSocketError;
        public event EventHandler Connected;
        public event EventHandler ConnectionClosed;
        public event EventHandler NickChanged;
        #endregion

        #region "Socket Event-Handlers
        /// <summary>
        /// Triggered if connection was established.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketConnectedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(Socket.RemoteHost, "Connection to server established.");
            if (Connected != null)
                Connected(this, new EventArgs());
        }

        /// <summary>
        /// Triggered if connection is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketClosedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(Socket.RemoteHost, "Connection closed.");
            if (ConnectionClosed != null)
                ConnectionClosed(this, new EventArgs());
            Init();
        }

        /// <summary>
        /// Triggered when a socket exception is raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketErrorHandler(object sender, ClientSocket.ExceptionEventArgs e)
        {
            Debug.WriteLine(e.CatchedException.Message, "SocketError.");
            if (ConnectionSocketError != null)
                ConnectionSocketError(sender, e);
            else
                throw (e.CatchedException);
            Init();

        }

        /// <summary>
        /// Triggered when the current nickname was changed.
        /// </summary>
        /// <param name="nick">The new nickname</param>
        private void NicknameChanged(string pNick)
        {
            nick = pNick;
            node.Nickname = nick;
            if (NickChanged != null)
            {
                NickChanged(this, new EventArgs());
            }
            Debug.WriteLine(nick, "Nickname Changed!");
        }


        /// <summary>
        /// Triggered when there is incoming traffic on the socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataArrivalHandler(object sender, EventArgs e)
        {
            string incoming = Socket.GetArrivedText();

            // parse line-by-line
            string temp = "";
            string[] lines = incoming.Split('\n');  // split by line
            foreach (string line in lines)
            {   // create IRCMessage object from raw message text
                if (line != "\r" && line != "")
                {
                    if (line.Substring(line.Length - 1, 1) == "\r")
                        temp = line.Substring(0, line.Length - 1);
                    else
                        temp = line;
                    temp.Trim();
                    IRCMessage msg = IRCMessage.Parse(temp);

                    HandleMessage(msg); // process message
                }
            }
        }
        #endregion

        /// <summary>
        /// Processes the given message, depending on who sent it and the
        /// contents
        /// </summary>
        /// <param name="pMsg">The incoming/outgoing message.</param>
        private void HandleMessage(IRCMessage pMsg)
        {

            if (pMsg.IsSenderLocal) // check the sender
            {
                if (SentMessage != null)
                {
                    SentMessage(this, new MessageEventArgs(pMsg));
                }
            }
            else
            {
                if (GotMessage != null)
                {
                    GotMessage(this, new MessageEventArgs(pMsg));
                }
            }

            switch (pMsg.MessageTypeID.ToUpper())
            {
                case IRCConstants.MSG_PING: //Server ping , respond to remain alive
                    {
                        SendMessage("PONG :" + pMsg.Body);
                        Debug.WriteLine(pMsg.Body, "Server Ping ponged");
                        break;
                    }

                case IRCConstants.MSG_NICK: //Nick change command from user
                    {
                        if (pMsg.IsSenderLocal)
                        {
                            if (pMsg.Body != "")
                                NicknameChanged(pMsg.Body);
                            else if (pMsg.Parameters.Count > 0)
                                NicknameChanged(pMsg.Parameters[0].ToString());
                        }
                        break;
                    }

                case IRCConstants.MSG_433: //Nickname is in use, try sending other nicks
                    {
                        Debug.WriteLine(pMsg.Body, "Nickname in use!");
                        if (!loggedIn)
                        {   // TODO: fix!
                            /*
                            if (_Nicks.Count > 0)
                            {
                                SendCommand("NICK " + _Nicks[0].ToString());
                                _Nicks.RemoveAt(0);
                            }
                            else
                            {
                                NicknameChanged("*");
                            }
                             * */
                        }
                        break;
                    }

                case IRCConstants.MSG_001: //Login success
                    {
                        loggedIn = true;
                        Debug.WriteLine(socket.RemoteHost, "Logged In to IRC Server.");
                        break;
                    }
            }
        }

        #region "Getters"
        public ClientSocket Socket { get { return socket; } }
        public IRCServer Server { get { return server; } }
        public IRCNode LocalIRCNode { get { return node; } }
        public string Nickname { get { return nick; } }
        public bool IsConnected() { return Socket.Connected; }
        #endregion
    }
}