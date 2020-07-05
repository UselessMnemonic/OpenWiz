namespace OpenWiz
{
    /// <summary>Class <c>WizError</c> stores error information in a <c>WizState</c></summary>
    ///
    public class WizError
    {
        /// <summary>
        /// The error code of the error.
        /// </summary>
        /// <value>An int value, or null if none is given.</value>
        /// 
        public int? Code { get; set; }

        /// <summary>
        /// The error message of the error.
        /// </summary>
        /// <value>A string, or null if none is given.</value>
        /// 
        public string Message { get; set; }
    }
}