using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWiz
{
    /// <summary>
    /// A socket-like class that wraps a standard Socket to handle
    ///  Wiz state objects.
    /// </summary>
    ///
    public class WizSocket : IDisposable
    {
        private const int PILOT_PORT = 38900;
        private Socket socket;

        /// <summary>
        /// Gets a value that indicates whether the underlying socket is connected
        /// to a remote host as of the last Send or Recieve operation.
        /// </summary>
        /// <value>true if the underlying socket was connected to a remote resource
        /// as of the most recent operation; otherwise, false.</value>
        ///
        public bool Connected
        {
            get { return socket.Connected; }
        }

        /// <summary>
        /// Gets the remote endpoint.
        /// </summary>
        /// <value>The System.Net.EndPoint with which the underlying socket is communicating.</value>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        ///
        public EndPoint RemoteEndPoint
        {
            get { return socket.RemoteEndPoint; }
        }

        /// <summary>
        /// Gets the local endpoint.
        /// </summary>
        /// <value>The System.Net.EndPoint that the underlying socket uses for communicating.</value>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        ///
        public EndPoint LocalEndPoint
        {
            get { return socket.LocalEndPoint; }
        }

        /// <summary>
        /// Initializes a new UDP/IP Datagram socket for Wiz lights.
        /// </summary>
        ///
        public WizSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// Establishes a connection to a remote Wiz light.
        /// </summary>
        /// <param name="h">A WizHandle that represents the remote light.</param>
        /// <exception cref="System.ArgumentNullException">
        /// h is null, or the IP address in h is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// A caller higher in the call stack does not have permission for the requested
        /// operation.
        /// </exception>
        ///
        public void Connect(WizHandle h)
        {
            if (h == null) throw new System.ArgumentNullException("Handle cannot be null.");
            socket.Connect(h.Ip, PILOT_PORT);
        }

        /// <summary>
        /// Closes the socket connection and allows reuse of the socket.
        /// </summary>
        /// <param name="reuseSocket">true if this socket can be reused after the current connection
        /// is closed; otherwise, false.</param>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        ///
        public void Disconnect(bool reuseSocket)
        {
            socket.Disconnect(reuseSocket);
        }

        /// <summary>
        /// Establishes a connection to a remote Wiz light.
        /// </summary>
        /// <param name="h">A WizHandle that represents the remote light.</param>
        /// <returns>An asynchronous Task.</returns>
        ///
        public async Task ConnectAsync(WizHandle h)
        {
            await socket.ConnectAsync(h.Ip, PILOT_PORT);
        }

        /// <summary>
        /// Begins an asynchronous request to disconnect from a remote Wiz light.
        /// </summary>
        /// <param name="e">The System.Net.Sockets.SocketAsyncEventArgs object to use
        /// for this asynchronous socket operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The e parameter cannot be null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// A socket operation was already in progress using the System.Net.Sockets.SocketAsyncEventArgs
        /// object specified in the e parameter.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <returns>true if the IO operation will complete asynchronously;
        /// false if the IO operation completed synchronously.</returns>
        ///
        public bool DisconnectAsync(SocketAsyncEventArgs e)
        {
            return socket.DisconnectAsync(e);
        }

        /// <summary>
        /// Begins an asynchronous request for a remote Wiz light connection.
        /// </summary>
        /// <param name="h">A WizHandle that represents the remote light.</param>
        /// <param name="callback">An System.AsyncCallback delegate that references the method to invoke when the
        /// connect operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the connect operation.
        /// This object is passed to the callback delegate when the operation is complete.</param>
        /// <exception cref="System.ArgumentNullException">
        /// h is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>An System.IAsyncResult that references the asynchronous connection.</returns>
        ///
        public IAsyncResult BeginConnect(WizHandle h, AsyncCallback callback, object state)
        {
            return socket.BeginConnect(h.Ip, PILOT_PORT, callback, state);
        }

        /// <summary>
        /// Ends a pending asynchronous connection request.
        /// </summary>
        /// <param name="result">An System.IAsyncResult object that stores state
        /// information and any user-defined data for this asynchronous operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// result is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// result was not returned by a call to OpenWiz.WizSocket.BeginConnect(OpenWiz.WizHandle,System.AsyncCallback,System.Object).
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// OpenWiz.WizSocket.EndConnect(System.IAsyncResult) was previously called
        /// for the asynchronous connection.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// 
        public void EndConnect(IAsyncResult result)
        {
            if (result == null) throw new ArgumentNullException("result cannot be null.");
            socket.EndConnect((result as WizAsyncResult).GetBackingResult());
        }

        /// <summary>
        /// Begins an asynchronous request to disconnect from a remote Wiz light.
        /// </summary>
        /// <param name="reuseSocket">true if this socket can be reused after the connection is closed;
        /// otherwise, false.</param>
        /// <param name="callback">An System.AsyncCallback delegate that references the method to invoke when the
        /// connect operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the connect operation.
        /// This object is passed to the callback delegate when the operation is complete.</param>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>An System.IAsyncResult that references the asynchronous disconnect.</returns>
        ///
        public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
        {
            return socket.BeginDisconnect(reuseSocket, callback, state);
        }

        /// <summary>
        /// Ends a pending asynchronous disconnect request.
        /// </summary>
        /// <param name="result">An System.IAsyncResult object that stores state
        /// information and any user-defined data for this asynchronous operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// result is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// result was not returned by a call to OpenWiz.WizSocket.BeginDisconnect(OpenWiz.WizHandle,System.AsyncCallback,System.Object).
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// OpenWiz.WizSocket.EndDisconnect(System.IAsyncResult) was previously called
        /// for the asynchronous connection.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.Net.WebException">
        /// The disconnect request has timed out.
        /// </exception>
        ///
        public void EndDisconnect(IAsyncResult result)
        {
            if (result == null) throw new ArgumentNullException("result cannot be null.");
            socket.EndDisconnect((result as WizAsyncResult).GetBackingResult());
        }

        /// <summary>
        /// Closes the System.Net.Sockets.Socket connection and releases all associated resources.
        /// </summary>
        /// 
        public void Close()
        {
            socket.Close();
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
        /// <exception cref="System.ArgumentNullException">
        /// s is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>true if the state was sent successfully; false, otherwise.</returns>
        /// 
        public bool Send(WizState s)
        {
            byte[] bytes = s.ToUTF8();
            return bytes.Length == socket.Send(bytes);
        }
        
        /// <summary>
        /// Receives data from a bound OpenWiz.WizSocket.
        /// </summary>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// A caller in the call stack does not have the required permissions.
        /// </exception>
        /// <returns>The remote state.</returns>
        ///
        public WizState Receive()
        {
            byte[] buffer = new byte[1024];
            int rLen = socket.Receive(buffer);
            return WizState.Parse(new ArraySegment<byte>(buffer, 0, rLen));
        }
        
        /// <summary>
        /// Sends data to a connected socket.
        /// </summary>
        /// <param name="s">The data to send.</param>
        /// <returns>An asynchronous task that completes with true if
        /// the operation was successful. Otherwise, the task will complete with an invalid
        /// socket error.</returns>
        /// 
        public async Task<bool> SendAsync(WizState s)
        {
            byte[] bytes = s.ToUTF8();
            return bytes.Length == await socket.SendAsync(bytes, SocketFlags.None);
        }

        /// <summary>
        /// Receives data from a connected socket.
        /// </summary>
        /// <returns>A task that represents the asynchronous receive operation.</returns>
        /// 
        public async Task<WizState> RecieveAsync()
        {
            byte[] buffer = new byte[1024];
            int rLen = await socket.ReceiveAsync(buffer, SocketFlags.None);
            return WizState.Parse(new ArraySegment<byte>(buffer, 0, rLen));
        }
    
        /// <summary>
        /// Sends data asynchronously to a connected OpenWiz.WizSocket.
        /// </summary>
        /// <param name="s">The data to send.</param>
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
        public IAsyncResult BeginSend(WizState s, AsyncCallback callback, object state)
        {
            byte[] buffer = s.ToUTF8();
            IAsyncResult ar = socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callback, state);
            return new WizAsyncResult(ar, buffer);
        }
 
        /// <summary>
        /// Ends a pending asynchronous send.
        /// </summary>
        /// <param name="asyncResult">An System.IAsyncResult that stores state information
        /// for this asynchronous operation.</param>
        /// <exception cref="System.ArgumentException">
        /// asyncResult was not returned by a call to OpenWiz.WizSocket.BeginSend(System.AsyncCallback,System.Object)
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// OpenWiz.WizSocket.EndSend(System.IAsyncResult) was previously called
        /// for the asynchronous send.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// asyncResult is null.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// An error occurred when attempting to access the underlying socket.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The underlying socket has been closed.
        /// </exception>
        /// <returns>If successful, true;
        /// otherwise, an invalid System.Net.Sockets.Socket error.</returns>
        /// 
        public bool EndSend(IAsyncResult asyncResult)
        {
            WizAsyncResult result = (WizAsyncResult) asyncResult;
            int len = socket.EndSend(result.GetBackingResult());
            return result.GetBuffer().Length == len;
        }

        /// <summary>
        /// Recieves data asynchronously from a connected OpenWiz.WizSocket.
        /// </summary>
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
        public IAsyncResult BeginRecieve(AsyncCallback callback, object state)
        {
            byte[] buffer = new byte[1024];
            IAsyncResult ar = socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, state);
            return new WizAsyncResult(ar, buffer);
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
        /// asyncResult is null.
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
        public WizState EndReceive(IAsyncResult asyncResult)
        {
            WizAsyncResult result = (WizAsyncResult) asyncResult;
            int rLen = socket.EndReceive(result.GetBackingResult());
            return WizState.Parse(new ArraySegment<byte>(result.GetBuffer(), 0, rLen));
        }

        /// <summary>
        /// Retrieves the underlying socket.
        /// </summary>
        /// <returns>A System.Net.Sockets.Socket.</returns>
        public Socket GetSocket()
        {
            return socket;
        }

        class WizAsyncResult : IAsyncResult
        {
            private IAsyncResult _result;
            private byte[] _buffer;

            public object AsyncState { get { return _result.AsyncState; } }
            public WaitHandle AsyncWaitHandle { get { return _result.AsyncWaitHandle; } }
            public bool CompletedSynchronously { get { return _result.CompletedSynchronously; } }
            public bool IsCompleted { get { return _result.IsCompleted; } }

            public byte[] GetBuffer() { return _buffer; }
            public IAsyncResult GetBackingResult() { return _result; }
            public WizAsyncResult(IAsyncResult ar, byte[] buffer)
            {
                _buffer = buffer;
                _result = ar;
            }
        }
    }
}