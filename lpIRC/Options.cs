using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace lpIRC
{
    public class Options
    {
        #region "Private members"
        private const string FILE_NAME = "lpIRC.xml";
        private const string ROOT_ELEMENT = "lpIRC";
        #endregion

        #region "Constructor"
        public Options()
        {
        }
        #endregion

        #region "Public methods"
        public static void SaveOptions(IRCServer pServer, IRCLogInDetails pLogIn)
        {
            // setup settings
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(FILE_NAME, settings))
            {
                // setup writer
                writer.WriteStartDocument();
                writer.WriteStartElement(ROOT_ELEMENT);

                // IRCServer-Info
                writer.WriteStartElement("IRCServer");
                writer.WriteElementString("Address", pServer.Address);
                writer.WriteElementString("Port", pServer.Port);
                writer.WriteEndElement();

                // LogIn-Details
                writer.WriteStartElement("IRCLogin");
                writer.WriteElementString("Username", pLogIn.Username);
                writer.WriteElementString("Realname", pLogIn.Realname);
                foreach (string n in pLogIn.Nicknames)
                {
                    writer.WriteElementString("Nickname", n);
                }
                writer.WriteEndElement();

                // finish
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        public static IRCLogInDetails LoadLogIn()
        {
            // make sure, file exists
            if (!OptionsExist())
                return null;

            string username = "";
            string realname = "";
            ArrayList nicknames = new ArrayList();

            // read from file
            using (XmlReader reader = XmlReader.Create(FILE_NAME))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case ROOT_ELEMENT:
                                break;

                            case "Username":
                                if (reader.Read())
                                    username = reader.Value;
                                else
                                    return null;
                                break;

                            case "Realname":
                                if (reader.Read())
                                    realname = reader.Value;
                                else
                                    return null;
                                break;

                            case "Nickname":
                                if (reader.Read())
                                    nicknames.Add(reader.Value);
                                else
                                    return null;
                                break;
                        }
                    }
                }
            }

            return (new IRCLogInDetails(username, realname, nicknames));
        }

        public static IRCServer LoadServer()
        {
            // make sure, file exists
            if (!OptionsExist())
                return null;

            string address = "";
            string port = "";

            // read from file
            using (XmlReader reader = XmlReader.Create(FILE_NAME))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case ROOT_ELEMENT:
                                break;

                            case "Address":
                                if (reader.Read())
                                    address = reader.Value;
                                else
                                    return null;
                                break;

                            case "Port":
                                if (reader.Read())
                                    port = reader.Value;
                                else
                                    return null;
                                break;
                        }
                    }
                }
            }

            return (new IRCServer(address, port));
        }

        public static bool OptionsExist()
        {
            return (System.IO.File.Exists(FILE_NAME));
        }
        #endregion
    }
}
