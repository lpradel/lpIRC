using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace lpIRC
{
    public class IRCMessage
    {
        #region "Message-Construction (static!)"
        public static IRCMessage Parse(string pRawMessage)
        {
            IRCMessage msg = new IRCMessage();
            
            // parse
            msg.rawText = pRawMessage;
            msg.body = GetBody(pRawMessage);
            msg.header = GetHeader(pRawMessage);
            msg.messageType = GetMessageType(msg.Header);
            msg.parameters = GetParameters(msg.Header);
            msg.sender = GetSender(msg.Header);
            msg.senderNode = msg.sender == "" ? null : new IRCNode(msg.sender);

            if (msg.Sender == "")   // no sender
                msg.messageType = msg.Header;

            return msg;
        }
        #endregion

        #region "Parsing"
        private static string GetHeader(string message)
        {
            string[] tmp = message.Split(":".ToCharArray(), 3);
            if (message.StartsWith(":") && tmp.Length >= 2)
                return tmp[1].Trim();
            if (!message.StartsWith(":") && tmp.Length >= 1)
                return tmp[0].Trim();
            return "";
        }

        private static string GetBody(string message)
        {
            string[] tmp;
            if (message.StartsWith(":"))
            {
                tmp = message.Split(":".ToCharArray(), 3);
                if (tmp.Length > 2)
                    return tmp[2].Trim();
            }
            else
            {
                tmp = message.Split(":".ToCharArray(), 2);
                if (tmp.Length > 1)
                    return tmp[1].Trim();
            }
            return "";
        }

        private static string GetMessageType(string messageHeader)
        {
            string[] tmp = messageHeader.Split(' ');
            if (tmp.Length > 1)
                return tmp[1].Trim();
            else
                return "";
        }

        private static string GetSender(string messageHeader)
        {
            string[] tmp = messageHeader.Split(' ');
            if (tmp.Length >= 2)
            {
                return tmp[0].Trim();
            }
            else
                return "";
        }

        private static ArrayList GetParameters(string messageHeader)
        {
            string[] parameters = messageHeader.Split(' ');
            ArrayList al = new ArrayList();
            if (parameters.Length > 2)
            {
                for (int i = 2; i < parameters.Length; i++)
                    if (parameters[i] != "")
                        al.Add(parameters[i].Trim());
            }
            return al;
        }
        #endregion

        #region "Private members"
        private bool senderIsLocalUser;
        private string header;
        private string body;
        private string sender;
        private string messageType;
        private string rawText;
        private ArrayList parameters = null;
        private IRCNode senderNode = null;
        #endregion

        #region "Getter"
        public string Header { get { return header; } }
        public string Body { get { return body; } }
        public string MessageTypeID { get { return messageType.ToUpper(); } }
        public string RawMessage { get { return rawText; } }
        public string Sender { get { return sender; } }
        public ArrayList Parameters { get { return parameters; } }
        public IRCNode SenderNode { get { return senderNode; } }
        public bool IsSenderLocal
        {
            get { return senderIsLocalUser; }
            set { senderIsLocalUser = value; }
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            sb.Append(SenderNode == null ? "SERVER" : SenderNode.ToString());
            sb.Append("> ");
            sb.Append(MessageTypeID);

            foreach (string parameter in parameters)
            {
                sb.Append(" ");
                sb.Append(parameter);
            }

            sb.Append(" : ");
            sb.Append(body);
            return sb.ToString();
        }

        public string ToShortString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            sb.Append(SenderNode == null ? "SERVER" : SenderNode.ToString());
            sb.Append("> ");

            // skip parameters, msgtype etc
            sb.Append(body);

            return sb.ToString();
        }

        /// <summary>
        /// Default constructor, create IRCMessage using the static method!
        /// </summary>
        protected IRCMessage()
        {

        }

        public IRCMessage(String pMsg)
        {
            rawText = pMsg;
        }
    }
}
