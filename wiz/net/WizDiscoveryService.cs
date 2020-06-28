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
        public const int PORT_DISCOVERY = 38899;
        public const int PING_PERIOD_MS = 5000;
        
        private UdpClient discoveryClient;
        private Timer timer;
        private bool keepAlive;

        private byte[] pingData;
        private string hostIp;

        /// <summary>Constructs a <c>WizDiscoveryService</c> targeting the
        ///   specified home</summary>
        /// <param name="homeId">The Home ID to which the user's lights belong</param>
        /// <param name="hostIp">The IP of the host machine</param>
        /// <param name="hostMac">The MAC of the host machine's interface card,
        ///   on which lights are reachable</param>
        ///
        public WizDiscoveryService(int homeId, string hostIp, string hostMac)
        {
            this.keepAlive = false;
            this.hostIp = hostIp;

            discoveryClient = null;
            timer = null;
            pingData = Encoding.ASCII.GetBytes(WizLightState.MakeRegistration(homeId, hostIp, hostMac).ToString());
        }

        /// Responsible for handling responses from Wiz lights
        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT_DISCOVERY);
            byte[] data = discoveryClient.EndReceive(ar, ref ep);

            // TODO: Share data with any listeners
            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Got {data.Length} bytes:");
            Console.WriteLine($"\t{Encoding.UTF8.GetString(data, 0, data.Length)}");

            if (keepAlive) discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        /// Responsible for registering the host with Wiz lights 
        private void Ping(object state)
        {
            if (!keepAlive)
            {
                timer.Dispose();
                return;
            }

            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Sending {pingData.Length} bytes:");
            Console.WriteLine($"\t{Encoding.UTF8.GetString(pingData, 0, pingData.Length)}");
            discoveryClient.Send(pingData, pingData.Length, new IPEndPoint(IPAddress.Broadcast, PORT_DISCOVERY));
        }

        /// <summary>Method <c>Start</c> starts the discovery service.</summary>
        /// <para>The service can be started any time. Subsequent calls while the service is already
        ///   active will be ignored.</para>
        ///
        public void Start()
        {
            if (keepAlive) return;
            keepAlive = true;

            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Starting service");
            try
            {
                discoveryClient = new UdpClient(new IPEndPoint(IPAddress.Any, 38899));
                timer = new Timer(new TimerCallback(Ping), null, 0, PING_PERIOD_MS);
                discoveryClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
            catch (SocketException e)
            {
                keepAlive = false;
                Console.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Could not start service -- {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            
        }

        /// <summary>Method <c>Stop</c> stops the discovery service.</summary>
        /// <para>The service can be stopped any time. Subsequent calls while the service is not
        ///   active will be ignored.</para>
        ///
        public void Stop()
        {
            if (keepAlive)
            {
                keepAlive = false;
                Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Stopping service");
            }
        }
    }
}