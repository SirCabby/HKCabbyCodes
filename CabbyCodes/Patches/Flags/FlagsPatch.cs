using CabbyCodes.Patches.Flags.General;
using CabbyCodes.Patches.Flags.NPC_Status;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags
{
    public class FlagsPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Game Flags").SetColor(CheatPanel.headerColor));

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("General").SetColor(CheatPanel.subHeaderColor));
            CrossroadsInfectedPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("NPC Alive / Dead").SetColor(CheatPanel.subHeaderColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Must be outside of npc room when updating").SetColor(CheatPanel.warningColor));
            MylaWaifuPatch.AddPanel();

            //CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Boss Defeated").SetColor(CheatPanel.subHeaderColor));
            //CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Grubs").SetColor(CheatPanel.subHeaderColor));
            //CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Stag Stations").SetColor(CheatPanel.subHeaderColor));
        }
    }
}
