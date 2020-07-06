using System;
using System.Net;
using System.Net.Sockets;

namespace OpenWiz
{
    ///<summary>An object describing a Wiz light on the network.</summary>
    ///
    public class WizHandle
    {
        /// <summary>
        /// Gets the MAC of the remote light as a hex string.
        /// </summary>
        /// <value>An unformatted hex string.</value>
        /// 
        public string Mac { get; }

        /// <summary>
        /// Gets the IP of the remote light.
        /// </summary>
        /// <value>An IPv4 address.</value>
        /// 
        public IPAddress Ip { get; }

        /// <summary>
        /// Creates a handle that can be used to connect to, or identify, a remote Wiz light.
        /// </summary>
        /// <param name="mac">The MAC of the remote light, as an unformatted
        /// string of 12 hex digits</param>
        /// <param name="address">The IPv4 address of the remote light.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If mac or ip are null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// If ip is not IPv4, or if mac is not strictly 12 hex digits.
        /// </exception>
        /// 
        public WizHandle(string mac, IPAddress ip)
        {
            if (mac == null) throw new ArgumentNullException("MAC cannot be null.");
            if (ip == null) throw new ArgumentNullException("IP cannot be null.");
            if (mac.Length != 12) throw new FormatException("MAC must be exactly 12 hex digits.");
            if (ip.AddressFamily != AddressFamily.InterNetwork) throw new FormatException("IP must be IPv4.");
            Ip = ip;
            Mac = mac.ToLower();

            char c;
            for (int i = 0; i < 12; i++)
            {
                c = Mac[i];
                if ((c < '0' || c > '9') &&
                    (c < 'a' || c > 'f')) throw new FormatException("MAC must be a hex string.");
            }
        }
    }
}