using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace OpenWiz
{
    /// <summary>Class <c>WizDiscoveryService</c> models the discovery routine used
    ///   by Wiz lights on the local network.</summary>
    ///
    public class WizDiscoveryService
    {
        private const int PORT_DISCOVERY = 38899;
        private const int PORT_PILOT = 38900;
        
        private byte[] pingData;
        private string hostIp;

        private UdpClient discoveryClient;
        private volatile bool KeepAlive;

        /// <summary>Constructs a <c>WizDiscoveryService</c> targeting the
        ///   specified home</summary>
        /// <param name="homeId">The Home ID to which the user's lights belong</param>
        /// <param name="hostIp">The IPv4 of the host machine, in standard dot notation</param>
        /// <param name="hostMac">The MAC of the host machine's interface card, 6 bytes wide</param>
        ///
        public WizDiscoveryService(int homeId, string hostIp, byte[] hostMac)
        {
            this.pingData = WizState.MakeRegistration(homeId, hostIp, hostMac).ToUTF8();
            this.hostIp = hostIp;
            this.KeepAlive = false;
            this.discoveryClient = new UdpClient(new IPEndPoint(IPAddress.Broadcast, PORT_DISCOVERY));
        }

        /// <summary>Method <c>Start</c> starts the discovery service asynchronously.</summary>
        /// <param name="listener">A <c>IWizUpdateListener</c> that responds to discoveries.</param>
        /// The service can be used any time. Be cautious of invoking the service on the same
        ///   listener before a prior call completes. It should be stopped soon after.
        ///
        public void Start(IWizDiscoveryListener listener)
        {
            if (KeepAlive | listener == null) return;
            KeepAlive = true;
            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Starting service");
            try
            {
                discoveryClient.Send(pingData, pingData.Length);
                discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), listener);
            }
            catch (SocketException e)
            {
                KeepAlive = false;
                Console.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Could not ping lights -- {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>Requests for the discovery service to stop.</summary>
        /// Should be called soon after the service has started.
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
                if (wState.Error.Message != null) Console.WriteLine($" -- {wState.Error.Message}");
                else Console.WriteLine();
                Console.WriteLine($"\t{jsonString}");
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