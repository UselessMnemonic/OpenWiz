using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

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
        
        private byte[] pingData;
        private string hostIp;

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
            this.pingData = WizState.MakeRegistration(homeId, hostIp, hostMac).ToUTF8();
            this.hostIp = hostIp;
            this.KeepAlive = false;
            this.discoveryClient = new UdpClient(PORT_DISCOVERY);
        }

        /// <summary>
        /// Starts the discovery service asynchronously. Be cautious of
        /// invoking the service on the same listener before a prior call completes.
        /// </summary>
        /// <param name="listener">A <c>IWizUpdateListener</c> that responds to discoveries.</param>
        ///
        public void Start(IWizDiscoveryListener listener)
        {
            if (KeepAlive | listener == null) return;
            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Starting service");
            KeepAlive = true;
            try
            {
                discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), listener);
                discoveryClient.SendAsync(pingData, pingData.Length, new IPEndPoint(IPAddress.Broadcast, PORT_DISCOVERY));
            }
            catch (SocketException e)
            {
                KeepAlive = false;
                Console.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Could not ping lights -- {e.Message}");
                Console.WriteLine(e.StackTrace);
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
                Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Stopping service");
            }
        }

        /// Responsible for handling responses from Wiz lights
        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT_DISCOVERY);
            byte[] data = discoveryClient.EndReceive(ar, ref ep);
            string jsonString = Encoding.UTF8.GetString(data);
            WizState wState = WizState.Parse(data);

            IWizDiscoveryListener listener = (IWizDiscoveryListener) ar.AsyncState;
            if (wState == null)
            {
                Console.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Got bad json message:");
                Console.WriteLine($"\t{jsonString}");
            }
            else if (wState.Error != null)
            {
                Console.Write($"[WARNING] WizDiscoveryService@{hostIp}: Encountered ");

                if (wState.Error.Code == null) Console.Write("unknwon error");
                else Console.Write($"error {wState.Error.Code}");
                if (wState.Error.Message != null) Console.Write($" -- {wState.Error.Message}");
                Console.WriteLine($" from {ep.Address}");
                //Console.WriteLine($"\t{jsonString}");
            }
            else if (wState.Result != null)
            {
                Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Got response:");
                Console.WriteLine($"\t{jsonString}");
                WizHandle handle = new WizHandle(wState.Result.Mac, ep.Address);
                listener.OnDiscover(handle);
            }

            if (KeepAlive) discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), listener);
        }
    }
}