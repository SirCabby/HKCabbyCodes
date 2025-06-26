using CabbyCodes.Patches.Player;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Main player patch class that coordinates all player-related cheat panels.
    /// </summary>
    public class PlayerPatch
    {
        /// <summary>
        /// Adds all player-related cheat panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor));
            InvulPatch.AddPanel();
            DamagePatch.AddPanel();
            JumpPatch.AddPanel();
            SoulPatch.AddPanel();
            PermadeathPatch.AddPanel();
            HealthPatch.AddPanel();
            GeoPatch.AddPanel();
            PlayTimePatch.AddPanel();
        }
    }
}
