using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace lpIRC
{
    /// <summary>
    /// Holds all relevant data regarding login to an IRC-server.
    /// </summary>
    public class IRCLogInDetails
    {
        #region "Private members"
        private string username;
        private string realname;
        private ArrayList nicknames = new ArrayList();
        #endregion

        #region "Getter"
        public string Username
        {
            get { return username; }
        }

        public string Realname
        {
            get { return realname; }
        }

        public ArrayList Nicknames
        {
            get { return nicknames; }
        }
        #endregion

        /// <summary>
        /// Constructor for all relevant login-data.
        /// </summary>
        /// <param name="pUsername">Desired Username for Server-Login.</param>
        /// <param name="pRealname">Desired realname for Server-Login.</param>
        /// <param name="pNicknames">List of nicknames, separated by spaces.</param>
        public IRCLogInDetails(string pUsername, string pRealname, string pNicknames)
        {
            username = pUsername;
            realname = pRealname;

            // get nicknames, split by spaces
            string[] temp = pNicknames.Split(" ".ToCharArray());
            foreach (string s in temp)
                nicknames.Add(s);
        }

        public IRCLogInDetails(string pUsername, string pRealname, ArrayList pNicknames)
        {
            username = pUsername;
            realname = pRealname;

            foreach (string n in pNicknames)
                nicknames.Add(n);
        }
    }
}
