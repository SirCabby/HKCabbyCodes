using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyMenu.SyncedReferences;
using CabbyCodes.SavedGames;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Player
{
    /// <summary>
    /// Main player patch class that coordinates all player-related cheat panels.
    /// </summary>
    public class PlayerPatch : BasePatch
    {
        // Static state tracking for max health changes that require save/reload
        private static int startingMaxHealth = -1;
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
            startingMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);

            var panels = new List<CheatPanel>
            {
                new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor),
                new TogglePanel(new InvulPatch(), FlagInstances.isInvincible.ReadableName),
                new TogglePanel(new DamagePatch(), "One Hit Kills (Enemies can't block)"),
                new TogglePanel(new JumpPatch(), FlagInstances.infiniteAirJump.ReadableName),
                new RangeInputFieldPanel<int>(
                    new DelegateReference<int>(
                        () => FlagManager.GetIntFlag(FlagInstances.maxHealthBase),
                        value =>
                        {
                            var validation = FlagValidationData.GetIntValidationData(FlagInstances.maxHealthBase);
                            value = ValidationUtils.ValidateRange(value, validation.MinValue, validation.MaxValue);
                            FlagManager.SetIntFlag(FlagInstances.maxHealthBase, value);
                            FlagManager.SetIntFlag(FlagInstances.maxHealth, value);

                            // Check if current max health differs from starting max health
                            var finalMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                            
                            bool maxHealthDiffersFromStart = finalMaxHealth != startingMaxHealth;
                            
                            if (maxHealthDiffersFromStart)
                            {
                                // Max health has changed from starting state, request reload
                                GameReloadManager.RequestReload($"MaxHealth");
                            }
                            else
                            {
                                // Max health matches starting state, cancel reload request
                                GameReloadManager.CancelReload($"MaxHealth");
                            }
                        }),
                    FlagValidationData.GetIntValidationData(FlagInstances.maxHealthBase).ValidChars, 
                    FlagValidationData.GetIntValidationData(FlagInstances.maxHealthBase).MinValue, 
                    FlagValidationData.GetIntValidationData(FlagInstances.maxHealthBase).MaxValue, 
                    FlagInstances.maxHealthBase.ReadableName),
                new RangeInputFieldPanel<int>(
                    new DelegateReference<int>(
                        () => FlagManager.GetIntFlag(FlagInstances.healthBlue),
                        value =>
                        {
                            var validation = FlagValidationData.GetIntValidationData(FlagInstances.healthBlue);
                            value = ValidationUtils.ValidateRange(value, validation.MinValue, validation.MaxValue);
                            FlagManager.SetIntFlag(FlagInstances.healthBlue, value);
                        }),
                    FlagValidationData.GetIntValidationData(FlagInstances.healthBlue).ValidChars, 
                    FlagValidationData.GetIntValidationData(FlagInstances.healthBlue).MinValue, 
                    FlagValidationData.GetIntValidationData(FlagInstances.healthBlue).MaxValue, 
                    FlagInstances.healthBlue.ReadableName),
                new TogglePanel(new SoulPatch(), "Infinite Soul"),
                new TogglePanel(new GeoPatch(), "Infinite Geo"),
                new TogglePanel(new PermadeathPatch(), FlagInstances.permadeathMode.ReadableName),
                new RangeInputFieldPanel<float>(
                    new DelegateReference<float>(
                        () => FlagManager.GetFloatFlag("playTime", "Global"),
                        value =>
                        {
                            var validation = FlagValidationData.GetFloatValidationData(FlagInstances.playTime);
                            value = ValidationUtils.ValidateRange(value, validation.MinValue, validation.MaxValue);
                            FlagManager.SetFloatFlag("playTime", "Global", value);
                        }),
                    FlagValidationData.GetFloatValidationData(FlagInstances.playTime).ValidChars,
                    FlagValidationData.GetFloatValidationData(FlagInstances.playTime).MinValue,
                    FlagValidationData.GetFloatValidationData(FlagInstances.playTime).MaxValue,
                    "Playtime (seconds)")
            };

            return panels;
        }
    }
}
