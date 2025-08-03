using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Player
{
    /// <summary>
    /// Main player patch class that coordinates all player-related cheat panels.
    /// </summary>
    public class PlayerPatch : BasePatch
    {
        public static void AddPanels()
        {
            var playerPatch = new PlayerPatch();
            var panels = playerPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor),
                new TogglePanel(new InvulPatch(), FlagInstances.isInvincible.ReadableName),
                new TogglePanel(new DamagePatch(), "One Hit Kills (Enemies can't block)"),
                new TogglePanel(new JumpPatch(), FlagInstances.infiniteAirJump.ReadableName),
                new RangeInputFieldPanel<int>(new HealthPatch(), KeyCodeMap.ValidChars.Numeric, Constants.MIN_HEALTH, Constants.MAX_HEALTH, FlagInstances.maxHealthBase.ReadableName),
                new TogglePanel(new SoulPatch(), "Infinite Soul"),
                new TogglePanel(new GeoPatch(), "Infinite Geo"),
                new TogglePanel(new PermadeathPatch(), FlagInstances.permadeathMode.ReadableName),
                new RangeInputFieldPanel<float>(new PlayTimePatch(), KeyCodeMap.ValidChars.Decimal, 0f, 999999f, "Playtime (seconds)")
            };

            return panels;
        }
    }
}
