namespace OpenWiz
{
    /// <summary>Class <c>WizResult</c> stores result information in a <c>WizState</c></summary>
    ///
    public class WizResult : WizParams
    {
        /// <summary>
        /// Wether a method call was successful.
        /// </summary>
        /// <value>true if the method call is successful; false or null, otherwise</value>
        /// 
        public bool? Success { get; set; }
    }
}