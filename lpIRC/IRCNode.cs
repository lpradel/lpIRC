using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace lpIRC
{
    public class IRCNode
    {
        #region "Static members"
		private static string GetWord(string pText, string pSeparator,int pIndex, out string pRemaining)
		{
			string[] temp = pText.Split(pSeparator.ToCharArray());
			if(temp.Length<pIndex)
			{
				pRemaining="";
				return "";
			}
			else
			{
				pRemaining="";
				for(int i=1;i<temp.Length;i++)
				{					
					pRemaining+=temp[i];
				}
				return temp[pIndex-1];
			}
		}		
		#endregion

		#region "Private members"
		private string nickname;
		private string realName;
		private string username;
		private string hostName;
		private string server;
		private bool isServer;
		private bool isIdentified;
		private ArrayList channelsIn;
		#endregion

		#region "Constructors"				
		public IRCNode(string NodeInfo)
		{			
			if(NodeInfo!="")
			{
				if(NodeInfo.Substring(0,1)==":")
					NodeInfo=NodeInfo.Substring(1);        			
				if(NodeInfo.IndexOf("@")==-1)
				{
					hostName=NodeInfo.Trim();
					nickname=NodeInfo.Trim();
					server=NodeInfo.Trim();				
					isServer=true;				
				}            	
				else
				{
					nickname=GetWord(NodeInfo,"!",1,out username).Trim();
					username=GetWord(username,"@",1,out hostName).Trim();
					hostName=hostName.Trim();
					if(Username!="")
					{
						if(username.Substring(0,1)=="~")
						{
							username=username.Substring(1);
							isIdentified=false;				
						}
						else
							isIdentified=true;							
					}
					channelsIn=new ArrayList();
					server="";
					realName="";			
				}
			}			
		}
		#endregion

		#region "Getters"
		public string Nickname { get { return	nickname;} set { nickname=value; } }
		public string Hostname { get { return hostName;} set { hostName=value; } }
		public string Realname { get	{return			realName;} set{realName=value;}}
		public string Username { get	{return			username;} set{username=value;}}
		public string ServerOn { get	{return			server;} set{server=value;}}
		public bool	 IsIdentified {get	{return			isIdentified;} set{isIdentified=value;}}
		public bool	IsServer {get	{return			isServer;} set{isServer =value;}}
		#endregion		

		public override string ToString()
		{
			return nickname+"!"+username +"@" + hostName;
		}
	}

}
