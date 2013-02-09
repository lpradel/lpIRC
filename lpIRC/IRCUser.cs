using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace lpIRC
{
    /// <summary>
    /// Holds all relevant information on a specific user in a channel.
    /// </summary>
    public class IRCUser
    {
        #region "Private members"
        private string nickname;
        private IRCChannel channel;
        private bool isOperator;
        private bool hasVoice;
        private IRCNode node;
        #endregion

        #region "Constructors"
        public IRCUser(string pNickname, IRCChannel pChannel, bool pOperator, bool pVoice)
        {
            nickname = pNickname;
            channel = pChannel;
            isOperator = pOperator;
            hasVoice = pVoice;
            node = null;
        }

        public IRCUser(string pNickname, IRCChannel pChannel)
        {
            nickname = pNickname;
            channel = pChannel;

            // evaluate nickname
            if (pNickname.Substring(0, 1) == "@")
                isOperator = true;
            else
                isOperator = false;
            if (pNickname.Substring(0, 1) == "+")
                hasVoice = true;
            else
                hasVoice = false;

            // remove +/@ if necessarry
            if (isOperator || hasVoice)
                nickname = pNickname.Substring(1);

            node = null;
        }

        public IRCUser(IRCChannel pChannel, IRCNode pNode)
        {
            nickname = pNode.Nickname;
            channel = pChannel;
            isOperator = false;
            hasVoice = false;
            node = pNode;
        }
        #endregion

        #region "Getter/Setter"
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public string NicknameInChannel
        {
            get
            {
                if (IsOperator)
                    return ("@" + Nickname);
                if (HasVoice)
                    return ("+" + Nickname);
                return Nickname;
            }
        }

        public IRCChannel Channel
        {
            get { return channel; }
        }

        public bool IsOperator
        {
            get { return isOperator; }
            set { isOperator = value; }
        }

        public bool HasVoice
        {
            get { return hasVoice; }
            set { hasVoice = value; }
        }

        public IRCNode Node
        {
            get { return node; }
        }

        public override string ToString()
        {
            return NicknameInChannel;
        }
        #endregion
    }
}
