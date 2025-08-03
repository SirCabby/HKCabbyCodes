using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Interface for patches that can create panels
    /// </summary>
    public interface IPatch
    {
        CheatPanel CreatePanel();
    }
} 