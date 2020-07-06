namespace OpenWiz
{
    /// <summary>
    /// An interface for objects that listen to the discovery service.
    /// </summary>
    /// 
    public interface IWizDiscoveryListener
    {
        /// <summary>
        /// Called by the discovery service when a Wiz light is found.
        /// </summary>
        /// <param name="handle">Can be used to connect to the remote light.</param>
        /// 
        void OnDiscover(WizHandle handle);
    }
}