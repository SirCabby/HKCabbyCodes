using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Flags.FlagInfo;
using CabbyCodes.Flags.FlagData;

namespace CabbyCodes.Patches.Hunter
{
    public class HunterPatch : BasePatch
    {
        public static void AddPanels()
        {
            var hunterPatch = new HunterPatch();
            var panels = hunterPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Hunter's Journal Entries").SetColor(CheatPanel.headerColor)
            };

            // Unlock All toggle
            ButtonPanel buttonPanel = new ButtonPanel(() =>
            {
                foreach (HunterInfo hunterInfo in HunterData.GetAllHunterTargets())
                {
                    PlayerData.instance.SetBool(hunterInfo.KilledFlag.Id, true);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Unlock All", "Update all entries");

            // Lock all toggle
            PanelAdder.AddButton(buttonPanel, 1, () =>
            {
                foreach (HunterInfo hunterInfo in HunterData.GetAllHunterTargets())
                {
                    PlayerData.instance.SetBool(hunterInfo.KilledFlag.Id, false);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "Lock All", new Vector2(Constants.HUNTER_LOCK_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            panels.Add(buttonPanel);

            // Set kills remaining 0
            ButtonPanel setPanel = new ButtonPanel(() =>
            {
                foreach (HunterInfo hunterInfo in HunterData.GetAllHunterTargets())
                {
                    PlayerData.instance.SetInt(hunterInfo.KillsFlag.Id, 0);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "0", "All Entries: Set kills remaining");

            // Set kills remaining 1
            PanelAdder.AddButton(setPanel, 1, () =>
            {
                foreach (HunterInfo hunterInfo in HunterData.GetAllHunterTargets())
                {
                    PlayerData.instance.SetInt(hunterInfo.KillsFlag.Id, 1);
                }

                CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
            }, "1", new Vector2(Constants.HUNTER_ONE_BUTTON_WIDTH, Constants.DEFAULT_PANEL_HEIGHT));
            panels.Add(setPanel);

            panels.Add(new InfoPanel("<ON> to unlock entry, kills left to unlock notes (0 = unlocked)").SetColor(CheatPanel.subHeaderColor));

            // Add individual hunter panels
            foreach (HunterInfo hunterInfo in HunterData.GetAllHunterTargets())
            {
                panels.AddRange(CreateHunterPanels(hunterInfo));
            }

            return panels;
        }

        private List<CheatPanel> CreateHunterPanels(HunterInfo hunterInfo)
        {
            var panels = new List<CheatPanel>();

            // Create kills panel (IntPatch)
            var killsPatch = new IntPatch(hunterInfo.KillsFlag, hunterInfo.ReadableName);
            var killsPanel = killsPatch.CreatePanel();
            
            // Add toggle button for killed flag
            PanelAdder.AddToggleButton(killsPanel, 0, new BoolPatch(hunterInfo.KilledFlag, "Unlock Entry"));
            
            panels.Add(killsPanel);

            return panels;
        }
    }
}
