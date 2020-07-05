namespace OpenWiz
{
    /// <summary>Class <c>WizParams</c> stores parameter information in a <c>WizState</c></summary>
    ///
    public class WizParams
    {
        /// <summary>
        /// The Home ID of the remote light.
        /// </summary>
        /// <value>An int value, or null if none is given.</value>
        /// 
        public int? HomeId { get; set; }

        /// <summary>
        /// The IP of the client host machine, in standard dot notation.
        /// </summary>
        /// <value>A string, or null if none is given.</value>
        /// 
        public string PhoneIp { get; set; }

        /// <summary>
        /// The MAC of the client host machine, as an unformatted lowercase hex string.
        /// </summary>
        /// <value>A string, or null if none is given.</value>
        /// 
        public string PhoneMac { get; set; }

        /// <summary>
        /// Wether a registration request is successful.
        /// </summary>
        /// <value>true if a register request is successful; false or null, otherwise.</value>
        /// 
        public bool? Register { get; set; }
    }
}