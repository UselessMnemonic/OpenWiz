namespace OpenWiz
{
    /// <summary>Class <c>WizParams</c> stores parameter information in a <c>WizState</c></summary>
    ///
    public class WizParams
    {
        /*
         * These parameters are common for regular function.
         */

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
        /// <value>A non-negative int, or null if no scene is active.</value>
        /// 
        public int? SceneId { get; set; }

        /// <summary>
        /// The current speed at which a scene plays, as a percent.
        /// </summary>
        /// <value>An int in [0,100], or null if no value is given.</value>
        /// 
        public int? Speed { get; set; }

        /// <summary>
        /// Whether the current scene is playing.
        /// </summary>
        /// <value>true if the current scene is playing; false, otherwise</value>
        /// 
        public int? Play { get; set; }

        /// <summary>
        /// The current Red component of the set color.
        /// </summary>
        /// <value>An int in [0,255], or null if no value is given.</value>
        /// 
        public int? R { get; set; }

        /// <summary>
        /// The current Green component of the set color.
        /// </summary>
        /// <value>An int in [0,255], or null if no value is given.</value>
        /// 
        public int? G { get; set; }

        /// <summary>
        /// The current Blue component of the set color.
        /// </summary>
        /// <value>An int in [0,255], or null if no value is given.</value>
        /// 
        public int? B { get; set; }

        /// <summary>
        /// The current Cool White component of the set color.
        /// </summary>
        /// <value>An int in [0,100], or null if no value is given.</value>
        /// 
        public int? C { get; set; }

        /// <summary>
        /// The current Warm White component of the set color.
        /// </summary>
        /// <value>An int in [0,100], or null if no value is given.</value>
        /// 
        public int? W { get; set; }

        /// <summary>
        /// The current white-light temperature, in Kelvin
        /// </summary>
        /// <value>A positive int, or null if no value is given.</value>
        /// 
        public int? Temp { get; set; }

        /// <summary>
        /// The dimming/intensity of the light, as a percent.
        /// </summary>
        /// <value>An int in [0,100], or null if no value is given.</value>
        /// 
        public int? Dimming { get; set; }

        /*
         * These parameters are used in registration
         */
        
        /// <summary>
        /// The IP of the client host machine, in standard dot notation.
        /// </summary>
        /// <value>An IPv4 address in dot notation, or null if none is given.</value>
        /// 
        public string PhoneIp { get; set; }

        /// <summary>
        /// The MAC of the client host machine, as an unformatted lowercase hex string.
        /// </summary>
        /// <value>A 12-digit hex string, or null if none is given.</value>
        /// 
        public string PhoneMac { get; set; }

        /// <summary>
        /// Wether a registration request is successful.
        /// </summary>
        /// <value>true if a register request is successful; false or null, otherwise.</value>
        /// 
        public bool? Register { get; set; }

        /*
         * These parameters are for configuration
         */

        /// <summary>
        /// The name of the remote light.
        /// </summary>
        /// <value>A string, or null if none is given</value>
        /// 
        public string ModuleName { get; set; }

        /// <summary>
        /// The MAC of the remote light.
        /// </summary>
        /// <value>A 12-digit hex string, or null if none is given</value>
        /// 
        public string Mac { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>An int value, or null if none is given.</value>
        /// 
        public int? TypeId { get; set; }

        /// <summary>
        /// The Home ID of the remote light.
        /// </summary>
        /// <value>A positive int value, or null if none is given.</value>
        /// 
        public int? HomeId { get; set; }

        /// <summary>
        /// The Group ID of the remote light.
        /// </summary>
        /// <value>A non-negative int, or null if none is given.</value>
        /// 
        public int? GroupId { get; set; }

        /// <summary>
        /// The Room ID of the remote light.
        /// </summary>
        /// <value>A non-negative int value, or null if none is given.</value>
        /// 
        public int? RoomId { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>A bool, or null if no value is given.</value>
        /// 
        public bool? HomeLock { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>A bool, or null if no value is given.</value>
        /// 
        public bool? PairingLock { get; set; }

        /// <summary>
        /// The remote light's firmware version.
        /// </summary>
        /// <value>A string, or null if no value is given.</value>
        /// 
        public string FwVersion { get; set; }

        /// <summary>
        /// The fade-in time in milliseconds (power-on)
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? FadeIn { get; set; }

        /// <summary>
        /// The fade-out time in milliseconds (power-off)
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? FadeOut { get; set; }

        /// <summary>
        /// Whether to fade the nightlight (?)
        /// </summary>
        /// <value>A bool, or null if no value is given.</value>
        /// 
        public bool? FadeNight { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>An int, or null if no value is given.</value>
        /// 
        public int? DftDim { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>An int pair, or null if no value is given.</value>
        /// 
        public int[] PwmRange { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>An int pair, or null if no value is given.</value>
        /// 
        public int[] DrvConf { get; set; }

        /// <summary>
        /// The white temperature range of the light
        /// </summary>
        /// <value>An int pair, or null if no value is given.</value>
        /// 
        public int[] WhiteRange { get; set; }

        /// <summary>
        /// The white temperature range advertised to the user
        /// </summary>
        /// <value>An int pair, or null if no value is given.</value>
        /// 
        public int[] ExtRange { get; set; }

        /// <summary>
        /// Unsure
        /// </summary>
        /// <value>A bool, or null if no value is given.</value>
        /// 
        public bool? Po { get; set; }
    }
}