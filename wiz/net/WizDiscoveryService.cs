using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace OpenWiz
{
    /// <summary>
    /// Models the discovery routine used by Wiz lights on the local network.
    /// </summary>
    /// TODO: Use WizSocket as backend, someday
    ///
    public class WizDiscoveryService
    {
        private const int PORT_DISCOVERY = 38899;
        
        private readonly byte[] pingData;
        private readonly string hostIp;

        private UdpClient discoveryClient;
        private volatile bool KeepAlive;

        /// <summary>
        /// Constructs the service, targeting the a specific home of lights.
        /// </summary>
        /// <param name="homeId">The Home ID to which the user's lights belong</param>
        /// <param name="hostIp">The IPv4 of the host machine, in standard dot notation</param>
        /// <param name="hostMac">The MAC of the host machine's interface card, 6 bytes wide</param>
        ///
        public WizDiscoveryService(int homeId, string hostIp, byte[] hostMac)
        {
            WizState registrationData = WizState.MakeRegistration(homeId, hostIp, hostMac);
            Debug.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Made registration info: {registrationData}");
            pingData = registrationData.ToUTF8();
            this.hostIp = hostIp;
            KeepAlive = false;
        }

        /// <summary>
        /// Starts the discovery service asynchronously. Be cautious of
        /// invoking the service on the same listener before a prior call completes.
        /// </summary>
        /// <param name="listener">A <c>IWizUpdateListener</c> that responds to discoveries.</param>
        ///
        public void Start(Action<WizHandle> discoveryAction)
        {
            if (KeepAlive | discoveryAction == null | discoveryClient != null) return;

            discoveryClient = new UdpClient(PORT_DISCOVERY);
            Debug.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Starting service");
            KeepAlive = true;
            try
            {
                discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), discoveryAction);
                discoveryClient.SendAsync(pingData, pingData.Length, new IPEndPoint(IPAddress.Broadcast, PORT_DISCOVERY));
            }
            catch (SocketException e)
            {
                KeepAlive = false;
                Debug.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Could not ping lights -- {e.Message}");
                Debug.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Requests for the discovery service to stop.
        /// Should be called soon after the service has started.
        /// </summary>
        ///
        public void Stop()
        {
            if (KeepAlive)
            {
                KeepAlive = false;
                Debug.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Stopping service");
                discoveryClient.Close();
                discoveryClient = null;
            }
        }

        /// Responsible for handling responses from Wiz lights
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!KeepAlive) return;

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT_DISCOVERY);
            byte[] data = discoveryClient.EndReceive(ar, ref ep);
            string jsonString = Encoding.UTF8.GetString(data);
            WizState wState = WizState.Parse(data);

            Action<WizHandle> discoveryAction = (Action<WizHandle>)ar.AsyncState;
            if (wState == null)
            {
                Debug.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Got bad json message:");
                Debug.WriteLine($"\t{jsonString}");
            }
            else if (wState.Error != null)
            {
                Debug.Write($"[WARNING] WizDiscoveryService@{hostIp}: Encountered ");

                if (wState.Error.Code == null) Debug.Write("unknwon error");
                else Debug.Write($"error {wState.Error.Code}");
                if (wState.Error.Message != null) Debug.Write($" -- {wState.Error.Message}");
                Debug.WriteLine($" from {ep.Address}");
                Debug.WriteLine($"\t{jsonString}");
            }
            else if (wState.Result != null)
            {
                Debug.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Got response:");
                Debug.WriteLine($"\t{jsonString}");
                WizHandle handle = new WizHandle(wState.Result.Mac, ep.Address);
                discoveryAction.Invoke(handle);
            }

            discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), discoveryAction);
        }
    }
}