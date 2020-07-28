namespace OpenWiz
{
    /// <summary>
    /// TODO: A class representing an RGBCW color space.
    /// This color space is a further abstraction of RGB color,
    /// wherein saturation of pure hues is controled by either "warm"
    /// or "cool" white light. Much research is needed to understand 
    /// the color dynamics and capabilties of Wiz brand lights to
    /// produce a color space suitable for production.
    /// 
    /// </summary>
    public abstract class WizColor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// 
        public int Red { get; } 

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// 
        public int Green { get; } 

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// 
        public int Blue { get; } 

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// 
        public int CoolWhite { get; } 

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// 
        public int WarmWhite { get; } 
    }
}