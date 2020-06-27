using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace OpenWIZ
{
    /// <summary>Class <c>WizDiscoveryService</c> models the discovery routine used
    ///   by Wiz lights on the local network.</summary>
    ///
    public class WizDiscoveryService
    {
        public const int PORT_BROADCAST = 38899;
        public const int PORT_LISTEN = 38899;
        public const int PING_PERIOD_MS = 5000;
        
        private int homeId;
        private string hostIp;
        private string hostMac;
        private bool keepAlive;

        private UdpClient pinger;
        private UdpClient listener;
        private Timer timer;
        private byte[] pingData;

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
            this.homeId = homeId;
            this.hostIp = hostIp;
            this.hostMac = hostMac;

            pinger = null;
            listener = null;
            timer = null;
            pingData = Encoding.ASCII.GetBytes(WizLightState.MakeRegistration(homeId, hostIp, hostMac).ToString());
        }

        /// Responsible for handling responses from Wiz lights
        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT_LISTEN);
            byte[] data = listener.EndReceive(ar, ref ep);

            // TODO: Share data with any listeners
            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Got {data.Length} bytes:");
            Console.WriteLine($"\t{Encoding.ASCII.GetString(data, 0, data.Length)}");

            if (keepAlive) listener.BeginReceive(new AsyncCallback(ReceiveCallback), null);
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
            Console.WriteLine($"\t{Encoding.ASCII.GetString(pingData, 0, pingData.Length)}");
            pinger.Send(pingData, pingData.Length, new IPEndPoint(IPAddress.Broadcast, PORT_BROADCAST));
        }

        /// <summary>Method <c>Start</c> starts the discovery service.</summary>
        /// <para>The service can be started any time. Subsequent calls while the service is already
        ///   active will be ignored.</para>
        ///
        public void Start()
        {
            if (keepAlive) return;
            
            Console.WriteLine($"[INFO] WizDiscoveryService@{hostIp}: Starting service");

            try
            {
                listener = new UdpClient(PORT_LISTEN);
                pinger = new UdpClient();
                pinger.EnableBroadcast = true;

                keepAlive = true;
                listener.BeginReceive(new AsyncCallback(ReceiveCallback), null);
                timer = new Timer(new TimerCallback(Ping), null, PING_PERIOD_MS, PING_PERIOD_MS);
            }
            catch (SocketException e)
            {
                keepAlive = false;
                Console.WriteLine($"[WARNING] WizDiscoveryService@{hostIp}: Could not start service -- {e.Message}");
            }
            
        }

        /// <summary>Method <c>Stop</c> stops the discovery service.</summary>
        /// <para>The service can be stopped any time. Subsequent calls while the service is not
        ///   active will be ignored.</para>
        ///
        public void Stop()
        {
            keepAlive = false;
        }
    }
}