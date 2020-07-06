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

        /// <summary>
        /// The MAC of the remote light.
        /// </summary>
        /// <value>A string, or null if none is given</value>
        /// 
        public string Mac { get; set; }

        /// <summary>
        /// Whether the remote light is on or off.
        /// </summary>
        /// <value>true if the light is on, false otherwise,
        /// or null if no value is given.</value>
        /// 
        public bool? State { get; set; }

        /// <summary>
        /// The Scene ID of the current scene.
        /// </summary>
        /// <value>An int, or null if no scene is active.</value>
        /// 
        public int? SceneId { get; set; }

        /// <summary>
        /// The current speed at which a scene plays.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? Speed { get; set; }

        /// <summary>
        /// The current Red component of the set color.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? R { get; set; }

        /// <summary>
        /// The current Green component of the set color.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? G { get; set; }

        /// <summary>
        /// The current Blue component of the set color.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? B { get; set; }

        /// <summary>
        /// The current Cool White component of the set color.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? C { get; set; }

        /// <summary>
        /// The current Warm White component of the set color.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? W { get; set; }

        /// <summary>
        /// The dimming/intensity of the light.
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? Dimming { get; set; }
    }
}