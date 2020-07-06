namespace OpenWiz
{
    /// <summary>
    /// Lists method names in the Wiz light API.
    /// </summary>
    ///
    public enum WizMethod
    {
        registration,
        pulse,
        firstBeat,

        getPilot,
        setPilot,
        syncPilot,

        getSystemConfig,
        setSystemConfig,
        getUserConfig,
        setUserConfig
    }
}