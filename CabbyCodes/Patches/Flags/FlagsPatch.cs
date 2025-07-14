using CabbyCodes.Patches.Flags.General;
using CabbyCodes.Patches.Flags.NPC_Status;
using CabbyCodes.Patches.Flags.Triage;
using CabbyMenu.UI.CheatPanels;

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

            FlagExtractionPatch.AddPanel();
            FlagMonitorPatch.AddPanel();

            // Add Room Flags section
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Room Flags").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Toggling room flags will not update until entering the room").SetColor(CheatPanel.warningColor));
            
            // Add area selector dropdown
            SceneFlagsAreaSelector areaSelector = new SceneFlagsAreaSelector();
            DropdownPanel areaDropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(areaDropdownPanel);
            
            // Add dynamic scene flags manager
            SceneFlagsPanelManager sceneFlagsManager = new SceneFlagsPanelManager(areaSelector);
            sceneFlagsManager.AddAllPanelsToMenu();
            
            // Add update action to handle area changes (for menu update loop)
            areaDropdownPanel.updateActions.Add(sceneFlagsManager.UpdateVisibleArea);
            // Hook dropdown value change event for immediate update
            areaDropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => sceneFlagsManager.UpdateVisibleArea());

            //CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Boss Defeated").SetColor(CheatPanel.subHeaderColor));
            //CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Stag Stations").SetColor(CheatPanel.subHeaderColor));
        }
    }
}
