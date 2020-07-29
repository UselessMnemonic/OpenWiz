using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace OpenWiz
{
    /// <summary>
    /// A socket-like class that wraps a standard UDP Socket to handle
    /// Wiz communication.
    /// </summary>
    ///
    public class WizSocket : IDisposable
    {
        public const int PORT_PILOT = 38900;
        public const int PORT_DISCOVER = 38899;
        private const int BUFFER_SIZE = 256;

        private readonly Socket socket;
        private readonly Dictionary<string, byte[]> bufferMap;

        /// <summary>
        /// Initializes a new UDP/IP Datagram socket for Wiz lights.
        /// </summary>
        ///
        public WizSocket()
        {
            bufferMap = new Dictionary<string, byte[]>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// Associates the OpenWiz.WizSocket as a server
        /// </summary>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been disposed.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// A caller in the call stack does not have the required permissions.
        /// </exception>
        /// 
        public void Bind()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT_PILOT));
        }

        /// <summary>
        /// Releases all resources used by the current instance of the OpenWiz.WizSocket
        /// class.
        /// </summary>
        /// 
        public void Dispose()
        {
            socket.Dispose();
        }

        /// <summary>
        /// Sends data to a connected OpenWiz.WizSocket.
        /// </summary>
        /// <param name="s">A OpenWiz.WizState containing the data to be sent.</param>
        /// <param name="handle">The handle to the remote light.</param>
        /// <exception cref="System.ArgumentNullException">
        /// s or handle are null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been disposed.
        /// </exception>
        /// <returns>the number of bytes sent</returns>
        /// 
        public int SendTo(WizState s, WizHandle handle)
        {
            if (s == null) throw new ArgumentNullException("s cannot be null");
            if (handle == null) throw new ArgumentNullException("handle cannot be null");

            byte[] bytes = s.ToUTF8();
            return socket.SendTo(bytes, new IPEndPoint(handle.Ip, PORT_DISCOVER));
        }
        
        /// <summary>
        /// Receives data from a bound OpenWiz.WizSocket.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// handle is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been disposed.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// A caller in the call stack does not have the required permissions.
        /// </exception>
        /// <returns>The remote state.</returns>
        ///
        public WizState ReceiveFrom(WizHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle cannot be null");
            byte[] buffer = new byte[BUFFER_SIZE];

            EndPoint ep = new IPEndPoint(handle.Ip, 0);
            int rLen = socket.ReceiveFrom(buffer, ref ep);
            return WizState.Parse(new ArraySegment<byte>(buffer, 0, rLen));
        }

        /// <summary>
        /// Sends data asynchronously to a connected OpenWiz.WizSocket.
        /// </summary>
        /// <param name="s">The data to send.</param>
        /// <param name="handle">The handle to the remote light.</param>
        /// <param name="callback">The System.AsyncCallback delegate to fall after the send completes.</param>
        /// <param name="state">An object that will be passed into the callback.</param>
        /// <exception cref="System.ArgumentNullException">
        /// s is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>An System.IAsyncResult that references the asynchronous send.</returns>
        /// 
        public IAsyncResult BeginSend(WizState s, WizHandle handle, AsyncCallback callback, object state)
        {
            byte[] buffer = s.ToUTF8();
            return socket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None,
                                      new IPEndPoint(handle.Ip, PORT_DISCOVER),
                                      callback, state);
        }
 
        /// <summary>
        /// Ends a pending asynchronous send.
        /// </summary>
        /// <param name="result">An System.IAsyncResult that stores state information
        /// for this asynchronous operation.</param>
        /// <exception cref="System.ArgumentException">
        /// result was not returned by a call to OpenWiz.WizSocket.BeginSend(System.AsyncCallback,System.Object)
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// OpenWiz.WizSocket.EndSend(System.IAsyncResult) was previously called
        /// for the asynchronous send.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// result is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>If successful, the number of bytes sent;
        /// otherwise, an invalid System.Net.Sockets.Socket error.</returns>
        /// 
        public int EndSend(IAsyncResult result)
        {
            return socket.EndSendTo(result);
        }

        /// <summary>
        /// Recieves data asynchronously from a connected OpenWiz.WizSocket.
        /// </summary>
        /// <param name="handle">The handle to the remote light.</param>
        /// <param name="callback">The System.AsyncCallback delegate to fall after the receive completes.</param>
        /// <param name="state">An object that will be passed into the callback.</param>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>An System.IAsyncResult that references the asynchronous receive.</returns>
        /// 
        public IAsyncResult BeginRecieveFrom(WizHandle handle, AsyncCallback callback, object state)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            bufferMap.Add(handle.Ip.ToString(), buffer);

            EndPoint ep = new IPEndPoint(handle.Ip, 0);
            return socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None,
                                           ref ep, callback, state);
        }

        /// <summary>
        /// Ends a pending asynchronous recieve.
        /// </summary>
        /// <param name="asyncResult">An System.IAsyncResult that stores state information
        /// for this asynchronous operation.</param>
        /// <exception cref="System.ArgumentException">
        /// asyncResult was not returned by a call to OpenWiz.WizSocket.BeginRecieve(System.AsyncCallback,System.Object)
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// OpenWiz.WizSocket.EndReceive(System.IAsyncResult) was previously called
        /// for the asynchronous receive.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// asyncResult or handle is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>If successful, the OpenWiz.WizState;
        /// otherwise, an invalid System.Net.Sockets.Socket error.</returns>
        /// 
        public WizState EndReceiveFrom(WizHandle handle, IAsyncResult result)
        {
            EndPoint ep = new IPEndPoint(handle.Ip, 0);
            int rLen = socket.EndReceiveFrom(result, ref ep);

            byte[] buffer = bufferMap[handle.Ip.ToString()];
            bufferMap.Remove(handle.Ip.ToString());

            return WizState.Parse(new ArraySegment<byte>(buffer, 0, rLen));
        }

        /// <summary>
        /// Retrieves the underlying socket.
        /// </summary>
        /// <returns>A System.Net.Sockets.Socket.</returns>
        public Socket GetSocket()
        {
            return socket;
        }
    }
}