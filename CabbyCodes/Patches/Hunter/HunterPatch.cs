using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using CabbyMenu.Utilities;
using CabbyCodes.Flags.FlagInfo;
using CabbyCodes.Flags.FlagData;

namespace CabbyCodes.Patches.Hunter
{
    public class HunterPatch : ISyncedReference<int>
    {
        public static List<HunterInfo> hunterTargets = HunterData.GetAllHunterTargets();

        private readonly HunterInfo hunterInfo;

        public HunterPatch(string targetName)
        {
            hunterInfo = HunterData.GetHunterTarget(targetName);
        }

        public int Get()
        {
            return PlayerData.instance.GetInt(hunterInfo.KillsFlag.Id);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_HUNTER_KILLS, Constants.MAX_HUNTER_KILLS, nameof(value));
            PlayerData.instance.SetInt(hunterInfo.KillsFlag.Id, value);
        }

        private static RangeInputFieldPanel<int> BuildCheatPanel(HunterInfo hunterInfo)
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new HunterPatch(hunterInfo.EnemyName), KeyCodeMap.ValidChars.Numeric, Constants.MIN_HUNTER_KILLS, Constants.MAX_HUNTER_KILLS, hunterInfo.ReadableName);
            PanelAdder.AddToggleButton(panel, 0, new HunterKilledPatch(hunterInfo.EnemyName));
            return panel;
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Hunter's Journal Entries").SetColor(CheatPanel.headerColor));

            // Unlock All toggle
            ButtonPanel buttonPanel = new ButtonPanel(() =>
            {
                foreach (HunterInfo hunterInfo in hunterTargets)
                {
                    PlayerData.instance.SetBool(hunterInfo.KilledFlag.Id, true);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Unlock All", "Update all entries");

            // Lock all toggle
            PanelAdder.AddButton(buttonPanel, 1, () =>
            {
                foreach (HunterInfo hunterInfo in hunterTargets)
                {
                    PlayerData.instance.SetBool(hunterInfo.KilledFlag.Id, false);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Lock All", new Vector2(Constants.HUNTER_LOCK_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);

            // Set kills remaining 0
            ButtonPanel setPanel = new ButtonPanel(() =>
            {
                foreach (HunterInfo hunterInfo in hunterTargets)
                {
                    PlayerData.instance.SetInt(hunterInfo.KillsFlag.Id, 0);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "0", "All Entries: Set kills remaining");

            // Set kills remaining 1
            PanelAdder.AddButton(setPanel, 1, () =>
            {
                foreach (HunterInfo hunterInfo in hunterTargets)
                {
                    PlayerData.instance.SetInt(hunterInfo.KillsFlag.Id, 1);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "1", new Vector2(Constants.HUNTER_ONE_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(setPanel);

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("<ON> to unlock entry, kills left to unlock notes (0 = unlocked)").SetColor(CheatPanel.subHeaderColor));
            foreach (HunterInfo hunterInfo in hunterTargets)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(BuildCheatPanel(hunterInfo));
            }
        }
    }
}
