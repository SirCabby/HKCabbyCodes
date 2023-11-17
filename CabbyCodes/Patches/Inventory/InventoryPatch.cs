using CabbyCodes.Patches.Inventory.Abilities;
using CabbyCodes.Patches.Inventory.Currency;
using CabbyCodes.Patches.Inventory.Items;
using CabbyCodes.Patches.Inventory.Map;
using CabbyCodes.Patches.Inventory.NailArts;
using CabbyCodes.Patches.Inventory.PowerUps;
using CabbyCodes.Patches.Inventory.Spells;
using CabbyCodes.Patches.Inventory.Upgrades;
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
            PaleOrePatch.AddPanel();
            RancidEggPatch.AddPanel();
            WanderersJournalPatch.AddPanel();
            HallownestSealPatch.AddPanel();
            KingsIdolPatch.AddPanel();
            ArcaneEggPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Ability Items").SetColor(CheatPanel.subHeaderColor));
            MothwingCloakPatch.AddPanel();
            MonarchWingsPatch.AddPanel();
            MantisClawPatch.AddPanel();
            CrystalHeartPatch.AddPanel();
            IsmasTearPatch.AddPanel();
            DreamNailPatch.AddPanel();
            DreamgatePatch.AddPanel();
            WorldSensePatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Upgrades").SetColor(CheatPanel.subHeaderColor));
            NailPatch.AddPanel();
            MaskShardPatch.AddPanel();
            VesselFragmentPatch.AddPanel();
            SalubrasBlessingPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Nail Arts").SetColor(CheatPanel.subHeaderColor));
            CyclonePatch.AddPanel();
            DashSlashPatch.AddPanel();
            UpwardSlashPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Spells").SetColor(CheatPanel.subHeaderColor));
            FocusPatch.AddPanel();
            VengefulSpiritPatch.AddPanel();
            DesolateDivePatch.AddPanel();
            HowlingWraithsPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Items").SetColor(CheatPanel.subHeaderColor));
            MapItemPatch.AddPanel();
            QuillPatch.AddPanel();
            LumaflyLanternPatch.AddPanel();
            TramPassPatch.AddPanel();
            CollectorsMapPatch.AddPanel();
            HuntersJournalPatch.AddPanel();
            HuntersMarkPatch.AddPanel();
            DelicateFlowerPatch.AddPanel();
            GodtunerPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Map Accessories").SetColor(CheatPanel.subHeaderColor));
            ScarabMarkerPatch.AddPanel();
            ShellMarkerPatch.AddPanel();
            GleamingMarkerPatch.AddPanel();
            TokenMarkerPatch.AddPanel();
            WhisperingRootPinPatch.AddPanel();
            WarriorsGravePinPatch.AddPanel();
            StagStationPinPatch.AddPanel();
            TramPinPatch.AddPanel();
            VendorPinPatch.AddPanel();
            HotSpringPinPatch.AddPanel();
            CocoonPinPatch.AddPanel();
            BenchPinPatch.AddPanel();
        }
    }
}
