using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CabbyCodes.Patches.Hunter
{
    public class HunterPatch : ISyncedReference<int>
    {
        public static List<string> hunterTargets = BuildHunterNames();

        private readonly string targetName;

        public HunterPatch(string targetName)
        {
            this.targetName = targetName;
        }

        public int Get()
        {
            return PlayerData.instance.GetInt("kills" + targetName);
        }

        public void Set(int value)
        {
            value = CabbyMenu.ValidationUtils.ValidateRange(value, Constants.MIN_HUNTER_KILLS, Constants.MAX_HUNTER_KILLS, nameof(value));
            PlayerData.instance.SetInt("kills" + targetName, value);
        }

        private static List<string> BuildHunterNames()
        {
            List<string> hunterNames = typeof(PlayerData).GetFields().Where(x => x.Name.StartsWith("killed")).Select(x => x.Name.Substring(6)).ToList();
            hunterNames.Remove("Dummy");
            return hunterNames;
        }

        private static InputFieldPanel<int> BuildCheatPanel(string targetName)
        {
            InputFieldPanel<int> panel = new(new HunterPatch(targetName), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 2, Constants.HUNTER_INPUT_WIDTH, targetName);
            PanelAdder.AddToggleButton(panel, 0, new HunterKilledPatch(targetName));

            return panel;
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Hunter's Journal Entries").SetColor(CheatPanel.headerColor));

            // Unlock All toggle
            ButtonPanel buttonPanel = new(() =>
            {
                foreach (string targetName in hunterTargets)
                {
                    PlayerData.instance.SetBool("killed" + targetName, true);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Unlock All", "Update all entries", Constants.HUNTER_UNLOCK_BUTTON_WIDTH);

            // Lock all toggle
            PanelAdder.AddButton(buttonPanel, 1, () =>
            {
                foreach (string targetName in hunterTargets)
                {
                    PlayerData.instance.SetBool("killed" + targetName, false);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Lock All", new Vector2(Constants.HUNTER_LOCK_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);

            // Set kills remaining 0
            ButtonPanel setPanel = new(() =>
            {
                foreach (string targetName in hunterTargets)
                {
                    PlayerData.instance.SetInt("kills" + targetName, 0);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "0", "All Entries: Set kills remaining", Constants.HUNTER_ZERO_BUTTON_WIDTH);

            // Set kills remaining 1
            PanelAdder.AddButton(setPanel, 1, () =>
            {
                foreach (string targetName in hunterTargets)
                {
                    PlayerData.instance.SetInt("kills" + targetName, 1);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "1", new Vector2(Constants.HUNTER_ONE_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(setPanel);

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("<ON> to unlock entry, kills left to unlock notes (0 = unlocked)").SetColor(CheatPanel.subHeaderColor));
            foreach (string targetName in hunterTargets)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(BuildCheatPanel(targetName));
            }
        }
    }
}
