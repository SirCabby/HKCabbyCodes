using CabbyCodes.Patches.Inventory.Abilities;
using CabbyCodes.Patches.Inventory.Currency;
using CabbyCodes.Patches.Inventory.NailArts;
using CabbyCodes.Patches.Inventory.Spells;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory
{
    public class InventoryPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Inventory").SetColor(CheatPanel.headerColor));

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Currency").SetColor(CheatPanel.subHeaderColor));
            GeoValuePatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Ability Items").SetColor(CheatPanel.subHeaderColor));
            MothwingCloakPatch.AddPanel();
            MantisClawPatch.AddPanel();
            CrystalHeartPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Nail Arts").SetColor(CheatPanel.subHeaderColor));
            CyclonePatch.AddPanel();
            DashSlashPatch.AddPanel();
            UpwardSlashPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Spells").SetColor(CheatPanel.subHeaderColor));
            FocusPatch.AddPanel();
            VengefulSpiritPatch.AddPanel();
            DesolateDivePatch.AddPanel();
            HowlingWraithsPatch.AddPanel();
        }
    }
}
