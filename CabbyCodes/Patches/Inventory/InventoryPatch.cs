using CabbyCodes.Patches.Inventory.NailArts;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory
{
    public class InventoryPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Inventory").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Nail Arts").SetColor(CheatPanel.subHeaderColor));
            CyclonePatch.AddPanel();
            DashSlashPatch.AddPanel();
            UpwardSlashPatch.AddPanel();
        }
    }
}
