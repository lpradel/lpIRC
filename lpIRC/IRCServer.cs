using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lpIRC
{
    public class IRCServer
    {
        #region "Private members"
        private string name;
        private string address;
        private string port;
        private string password;
        #endregion

        #region "Constructors"
        public IRCServer(string pName, string pAddress, string pPort, string pPassword)
        {
            name = pName;
            address = pAddress;
            port = pPort;
            password = pPassword;
        }

        public IRCServer(string pAddress, string pPort)
        {
            address = pAddress;
            port = pPort;
        }
        #endregion

        public override string ToString()
        {
            return this.Name + " ( " + this.Address + ":" + this.Port + ")";
        }

        #region "Getter"
        public string Name { get { return name; } }
        public string Address { get { return address; } }
        public string Password { get { return password; } }
        public string Port { get { return port; } }
        #endregion

    }
}
