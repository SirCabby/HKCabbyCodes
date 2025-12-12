#if LUMAFLY
using BepInEx.Configuration;
using CabbyMenu.UI;

namespace CabbyCodes
{
    /// <summary>
    /// Compatibility stub for CabbyCodesPlugin to allow existing code to work in Lumafly builds.
    /// This provides the same static interface as the BepInEx version.
    /// </summary>
    public static class CabbyCodesPlugin
    {
        /// <summary>
        /// Logger wrapper for HKAPI compatibility.
        /// </summary>
        public static LumaflyLoggerWrapper BLogger { get; set; }

        /// <summary>
        /// Configuration file wrapper for HKAPI compatibility.
        /// </summary>
        public static ConfigFile configFile { get; set; }

        /// <summary>
        /// Main menu instance for the mod.
        /// </summary>
        public static CabbyMainMenu cabbyMenu
        {
            get { return CabbyCodesModLumafly.cabbyMenu; }
            set { CabbyCodesModLumafly.cabbyMenu = value; }
        }
    }
}
#endif

