using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Adds the settings panel to the main menu.
    /// </summary>
    public class SettingsPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Mod Configuration").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new SettingsPanel());
        }
    }
} 