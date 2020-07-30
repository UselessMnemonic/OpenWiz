namespace OpenWiz
{
    /// <summary>
    /// Stores result information from a remote method call.
    /// </summary>
    ///
    public class WizResult : WizParams
    {
        /// <summary>
        /// Whether a method call was successful. Not always used.
        /// </summary>
        /// <value>true if a method call is successful; false or null, otherwise</value>
        /// 
        public bool? Success { get; set; }

        /// <summary>
        /// The RSSI of the transmission.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? RSSI { get; set; }
    }
}