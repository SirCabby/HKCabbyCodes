using CabbyCodes.Patches.Player;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    public class PlayerPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor));
            InvulPatch.AddPanel();
            DamagePatch.AddPanel();
            JumpPatch.AddPanel();
            SoulPatch.AddPanel();
            PermadeathPatch.AddPanel();
            GeoPatch.AddPanel();
            PlayTimePatch.AddPanel();
        }
    }
}
