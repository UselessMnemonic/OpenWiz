namespace OpenWiz
{
    /// <summary>Class <c>WizResult</c> stores result information in a <c>WizState</c></summary>
    ///
    public class WizResult
    {
        /// <summary>
        /// The Mac of the remote light, as a, unformatted lowercase hex string.
        /// </summary>
        /// <value>A hex string, or null if none is given.</value>
        /// 
        public string Mac { get; set; }

        /// <summary>
        /// Wether a method call was successful.
        /// </summary>
        /// <value>true if the method call is successful; false or null, otherwise</value>
        /// 
        public bool? Success { get; set; }
    }
}