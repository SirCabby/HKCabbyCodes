using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Main settings patch class that coordinates all settings-related cheat panels.
    /// </summary>
    public class SettingsPatch
    {
        /// <summary>
        /// Adds all settings-related cheat panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Settings").SetColor(CheatPanel.headerColor));
            QuickStartPatch.AddPanel();
            CustomSaveLoadPatch.AddPanels();
        }
    }
} 