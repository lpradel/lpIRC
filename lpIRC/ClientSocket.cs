/* ServerSocket class by sahand(sstiger3000@yahoo.com)
 * I spent a lot of time finding and 
 * solving the bugs and as far as i know
 * this code is bug free.
 * These classes are very easy to use
 * but make sure you take a look at those
 * sample files before you start using or 
 * testing them. */

/***********NOTE************
 * This socket is not suitable for sending large amounts of data
 * since the send method is not asynchronous.
 * I tried to make it such but it didn't work well enough
 * so in this version it's like that.
 * (Remember that i am using this for an IRC client and in IRC 
 * the biggest package can be 256 bytes , so no problem!) */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace lpIRC
{
    /// <summary>
    /// Provides an easy way of event driven socket programming.
    /// </summary>
    public class ClientSocket
    {
        #region "Private variables"
        private bool _connected;
        private System.Threading.Thread _thread = null;
        private System.Threading.Thread _connectThread = null;
        //private System.Threading.Thread			_sendThread=null;
        private System.Net.Sockets.Socket _socket = null;
        private byte[] _data = null;
        private System.Collections.Hashtable _clientdata = null;
        private DateTime _connectdatetime;
        private IPEndPoint _remoteEndPoint;
        private IPEndPoint _localEndPoint;
        private TextEncodingTypes _encodingType = TextEncodingTypes.Ascii;
        #endregion

        /// <summary>
        /// Enumeration type used with Send and Get text methods
        /// </summary>
        public enum TextEncodingTypes
        {
            Ascii,
            Unicode,
            Utf7,
            Utf8
        }
        #region "Properties"
        /// <summary>
        /// The System.Net.Sockets.Socket instance used in this object
        /// </summary>
        public Socket Client
        {
            get
            {
                return _socket;
            }
        }

        public DateTime ConnectDateTime
        {
            get
            {
                return _connectdatetime;
            }
        }
        /// <summary>
        /// Remotehost to which the socket is connected to or was connected to
        /// </summary>
        public string RemoteHost
        {
            get
            {
                try
                {
                    if (_socket == null)
                        return "";
                    if (_socket.RemoteEndPoint == null)
                        return "";
                    IPEndPoint ep;
                    ep = (IPEndPoint)_socket.RemoteEndPoint;
                    return ep.Address.ToString();
                }
                catch (SocketException e)
                {
                    if (SendException != null)
                    {
                        SendException(this, new ExceptionEventArgs(e));
                        return "";
                    }
                    else
                    {
                        throw e;
                    }
                }
                catch (ObjectDisposedException)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Remote port to which the socket is connected to or was connected to
        /// </summary>
        public int RemotePort
        {
            get
            {
                try
                {
                    if (_socket == null)
                        return 0;
                    if (_socket.RemoteEndPoint == null)
                        return 0;
                    IPEndPoint ep;
                    ep = (IPEndPoint)_socket.RemoteEndPoint;
                    return ep.Port;
                }
                catch (ObjectDisposedException)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating wheter the socket is connected or not
        /// Always returns truth!
        /// </summary>
        public bool Connected
        {
            get
            {
                return _connected;
            }
        }

        /// <summary>
        /// Local address to which the socket is bind to
        /// </summary>
        public string LocalHost
        {
            get
            {
                try
                {
                    if (_socket == null)
                        return Dns.GetHostName();
                    if (_socket.LocalEndPoint == null)
                        return Dns.GetHostName();
                    IPEndPoint ep;
                    ep = (IPEndPoint)_socket.LocalEndPoint;
                    return ep.Address.ToString();
                }
                catch (ObjectDisposedException)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Local port to which the socket is bind to
        /// </summary>
        public int LocalPort
        {
            get
            {
                try
                {
                    if (_socket == null)
                        return 0;
                    if (_socket.LocalEndPoint == null)
                        return 0;
                    IPEndPoint ep;
                    ep = (IPEndPoint)_socket.LocalEndPoint;
                    return ep.Port;
                }
                catch (ObjectDisposedException)
                {
                    return 0;
                }
            }
        }
        #endregion

        public class ExceptionEventArgs : EventArgs
        {
            private Exception e;
            public ExceptionEventArgs(Exception ex)
            {
                e = ex;
            }
            public Exception CatchedException
            {
                get { return e; }
                set { e = value; }
            }
        }

        public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

        #region "Events"
        /// <summary>
        /// Fires when data is ready to be read
        /// </summary>
        public event EventHandler DataArrival;
        /// <summary>
        /// Fires when a connection is successfully made 
        /// </summary>
        public event EventHandler ConnectSuccess;
        /// <summary>
        /// Fires when the socket is closed for any reason
        /// Including client request, server close or any error
        /// </summary>
        public event EventHandler Closed;

        public event ExceptionEventHandler ConnectionException;
        public event ExceptionEventHandler SendException;
        public event ExceptionEventHandler GeneralException;
        #endregion

        #region "Constructor and destrructor"
        /// <summary>
        /// Builds an instance of ClientSocket class
        /// </summary>
        /// <param name="socket">You can pass a connected socket as this parameter.</param>
        public ClientSocket(Socket socket)
        {
            _thread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckSocket));
            _thread.IsBackground = true;
            _clientdata = new System.Collections.Hashtable();
            _socket = socket;
            if (_socket.Connected)
            {
                _connectdatetime = DateTime.Now;
                _connected = true;
                _thread.Start();
            }
        }

        /// <summary>
        /// Creates a new instance of ClientSocket class with encoding algorithm set ASCII
        /// </summary>
        public ClientSocket()
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of ClientSocket class and sets the encoding algorithm specified.
        /// </summary>
        /// <param name="enc">The encoding algorithm used when manipulating text</param>
        public ClientSocket(TextEncodingTypes enc)
        {
            _encodingType = enc;
            Initialize();
        }

        ~ClientSocket()
        {
            _connected = false;
            _socket.Close();
        }
        #endregion

        /// <summary>
        /// This private method is used to check the socket for events.
        /// </summary>
        private void CheckSocket()
        {
            while (_connected)
            {
                if (_socket.Poll(-1, SelectMode.SelectRead))
                {
                    if (_socket.Connected)
                    {
                        byte[] temp;
                        byte[] buffer = null;
                        try
                        {
                            buffer = new byte[_socket.Available];
                        }
                        catch (ObjectDisposedException)
                        {
                            _connected = false;
                            Close();
                            break;
                        }
                        catch (Exception e)
                        {
                            if (GeneralException != null)
                                GeneralException(this, new ExceptionEventArgs(e));
                            else
                                throw e;
                        }

                        int bytesread = 0;

                        try
                        {
                            bytesread = _socket.Receive(buffer);
                        }
                        catch (ObjectDisposedException)
                        {
                            _connected = false;
                            Close();
                        }
                        catch (Exception e)
                        {
                            if (GeneralException != null)
                                GeneralException(this, new ExceptionEventArgs(e));
                            else
                                throw e;
                        }

                        if (_data != null)
                        {
                            temp = new byte[_data.Length];
                            _data.CopyTo(temp, 0);
                            _data = new byte[temp.Length + bytesread];
                            temp.CopyTo(_data, 0);
                            buffer.CopyTo(_data, temp.Length - 1);
                        }
                        else
                        {
                            _data = new byte[buffer.Length];
                            buffer.CopyTo(_data, 0);
                        }
                        if (bytesread > 0)
                        {
                            if (DataArrival != null)
                                DataArrival(this, new EventArgs());
                        }
                        else
                            Close();
                    }
                    else
                        Close();
                }
                else
                    Close();
            }
        }

        /// <summary>
        /// Returns arrived data and cleans the buffer.
        /// </summary>
        /// <returns>Arrived data in an array of bytes format</returns>
        public byte[] GetArrivedData()
        {
            return GetArrivedData(true);
        }

        /// <summary>
        /// Returns arrived data.
        /// </summary>
        /// <param name="ClearData">Pass true to clear the buffer , false to leave the old data in buffer</param>
        /// <returns>Arrived data in an array of bytes format</returns>
        public byte[] GetArrivedData(bool ClearData)
        {
            byte[] temp = new byte[_data.Length];
            _data.CopyTo(temp, 0);
            if (ClearData)
            {
                _data = null;
            }
            return temp;
        }

        /// <summary>
        /// Returns the arrived text and clears the buffer.
        /// To set the encoding algorithm , use the 
        /// Encoding propery.
        /// </summary>
        /// <returns>Arrived data in text format</returns>
        public string GetArrivedText()
        {
            return GetArrivedText(true, _encodingType);
        }

        /// <summary>
        /// Returns the arrived data in text format using ASCII encoding
        /// </summary>
        /// <param name="ClearData">Pass false to clean the arrived data buffer, either pass false</param>
        /// <returns>Arrived data in text format</returns>
        public string GetArrivedText(bool ClearData)
        {
            return GetArrivedText(ClearData, _encodingType);
        }

        /// <summary>
        /// Converts the arrived data to the appropriate text format using the encoding type and returns the text.
        /// </summary>
        /// <param name="ClearData">Pass false to clean the arrived data buffer, either pass false</param>
        /// <param name="Encodingtype">Value indicating the text encoding type used.</param>
        /// <returns></returns>
        private string GetArrivedText(bool ClearData, TextEncodingTypes Encodingtype)
        {
            byte[] buffer = GetArrivedData(ClearData);
            string result;
            switch (Encodingtype)
            {
                default:
                    {
                        result = System.Text.Encoding.ASCII.GetString(buffer);
                        break;
                    }
                case TextEncodingTypes.Unicode:
                    {
                        result = System.Text.Encoding.Unicode.GetString(buffer);
                        break;
                    }
                case TextEncodingTypes.Utf7:
                    {
                        result = System.Text.Encoding.UTF7.GetString(buffer);
                        break;
                    }
                case TextEncodingTypes.Utf8:
                    {
                        result = System.Text.Encoding.UTF8.GetString(buffer);
                        break;
                    }
            }
            return result;
        }

        /// <summary>
        /// Connects to a remote device.
        /// If the socket is connected when calling this method, it will first close the connection.
        /// </summary>
        /// <param name="Remotehost">Remote device hostname. This value can be an IP address or a DNS name</param>
        /// <param name="Remoteport">Port number to connect to.</param>
        public void Connect(string Remotehost, int Remoteport)
        {
            IPEndPoint lep, rep = null;
            lep = new IPEndPoint(IPAddress.Any, 0);
            bool success = true;
            try
            {
                //@deprecated
                //rep = new IPEndPoint(Dns.Resolve(Remotehost).AddressList[0], Remoteport);
                rep = new IPEndPoint(Dns.GetHostEntry(Remotehost).AddressList[0], Remoteport);
            }
            catch (Exception e)
            {
                success = false;
                if (ConnectionException != null)
                    ConnectionException(this, new ExceptionEventArgs(e));
                else
                    throw e;
            }
            if (success)
                Connect(lep, rep);
        }

        /// <summary>
        /// Connects to the remotehost using any available local port and local address.
        /// If the socket is connected when calling this method, it will first close the connection.
        /// </summary>
        /// <param name="RemoteEndPoint">Remotehost to connect to.</param>
        public void Connect(IPEndPoint RemoteEndPoint)
        {
            IPEndPoint lep;
            lep = new IPEndPoint(IPAddress.Any, 0);
            Connect(lep, RemoteEndPoint);
        }

        /// <summary>
        /// Used by the connection thread to try to connect asynchronously.
        /// </summary>
        private void TryToConnect()
        {
            bool success = true;
            try
            {
                _socket.Bind(_localEndPoint);
                _socket.Connect(_remoteEndPoint);
            }
            catch (Exception e)
            {
                success = false;
                if (ConnectionException != null)
                    ConnectionException(this, new ExceptionEventArgs(e));
                else
                    throw e;
            }
            if (success)
            {
                _connected = true;
                if (ConnectSuccess != null)
                    ConnectSuccess(this, new EventArgs());
                _connectdatetime = DateTime.Now;
                _thread.Start();
            }
        }

        /// <summary>
        /// Binds to the local endpoint and connects to the specified remote endpoint.
        /// If the socket is connected when calling this method, it will first close the connection.
        /// </summary>
        /// <param name="LocalEndPoint">Local endpoint, the socket will be bind to this endpoint</param>
        /// <param name="RemoteEndPoint">Remote endpoint to connect to</param>
        public void Connect(IPEndPoint LocalEndPoint, IPEndPoint RemoteEndPoint)
        {
            if (Connected)
                Close(false);
            Initialize();
            _localEndPoint = LocalEndPoint;
            _remoteEndPoint = RemoteEndPoint;
            _connectThread = new Thread(new ThreadStart(TryToConnect));
            _connectThread.Name = "ConnectThread";
            _connectThread.IsBackground = true;
            _connectThread.Start();
        }

        /// <summary>
        /// Sends an array of bytes to the remote computer.
        /// To send text use the SendText method.
        /// </summary>
        /// <param name="Data">An array of bytes to send to remote computer</param>
        public void SendData(byte[] Data)
        {
            try
            {
                _socket.Send(Data);
            }
            catch (SocketException e)
            {
                if (SendException != null)
                    SendException(this, new ExceptionEventArgs(e));
                else
                    throw e;
            }
        }

        /// <summary>
        /// Sends the specified text + a CrLf(Crriage return/Line feed) to the remote computer using ASCII encoding.
        /// </summary>
        /// <param name="Text"></param>
        public void SendTextAppendCrlf(string Text)
        {
            SendText(Text + "\r\n");
        }

        /// <summary>
        /// Sends the specified text to the remote computer using ASCII encoding.
        /// </summary>
        /// <param name="Text">Text to send</param>
        public void SendText(string Text)
        {
            SendText(Text, _encodingType);
        }

        /// <summary>
        /// Sends the specified text to the remote computer.
        /// </summary>
        /// <param name="Text">Text to send</param>
        /// <param name="EncodingType">Encoding type used to send the data</param>
        private void SendText(string Text, TextEncodingTypes EncodingType)
        {
            byte[] buffer = new byte[Text.Length];
            switch (EncodingType)
            {
                case TextEncodingTypes.Ascii:
                    {
                        buffer = System.Text.Encoding.ASCII.GetBytes(Text);
                        break;
                    }
                case TextEncodingTypes.Unicode:
                    {
                        buffer = System.Text.Encoding.Unicode.GetBytes(Text);
                        break;
                    }
                case TextEncodingTypes.Utf7:
                    {
                        buffer = System.Text.Encoding.UTF7.GetBytes(Text);
                        break;
                    }
                case TextEncodingTypes.Utf8:
                    {
                        buffer = System.Text.Encoding.UTF8.GetBytes(Text);
                        break;
                    }
            }
            SendData(buffer);
        }

        /// <summary>
        /// Closes the connection and stops all threads
        /// </summary>
        public void Close(bool RaiseEvent)
        {
            if (_connected && Closed != null && RaiseEvent)
                Closed(this, new EventArgs());
            _connected = false;
            _socket.Close();
            Initialize();
            ClearBuffer();
        }
        /// <summary>
        /// Closes the connection and stops all threads, raises Closed event
        /// </summary>
        public void Close()
        {
            if (_connected && Closed != null)
            {
                Closed(this, new EventArgs());
            }
            _connected = false;
            _socket.Close();
            ClearBuffer();
            Initialize();
        }

        /// <summary>
        /// Clears the arrived data buffer.
        /// </summary>
        public void ClearBuffer()
        {
            //GC would do the rest			
            _data = null;
        }
        /// <summary>
        /// Returns a hashtable of session variables.
        /// </summary>
        public System.Collections.Hashtable SessionVariables
        {
            get
            {
                return _clientdata;
            }
        }

        /// <summary>
        /// Sets the text encoding algorithm used for sending and getting text.
        /// </summary>
        public TextEncodingTypes Encoding
        {
            get { return _encodingType; }
            set
            {
                _encodingType = value;
            }
        }

        private void Initialize()
        {
            _thread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckSocket));
            _thread.IsBackground = true;
            _thread.Name = "Client Socket Thread";
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientdata = new System.Collections.Hashtable();
        }

        public override string ToString()
        {
            if (RemoteHost == "")
                return "<not connected>";
            else
                return RemoteHost + ":" + RemotePort.ToString();
        }
    }
}

