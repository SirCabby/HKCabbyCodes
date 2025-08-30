using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Player
{
    /// <summary>
    /// Main player patch class that coordinates all player-related cheat panels.
    /// </summary>
    public class PlayerPatch : BasePatch
    {
        // Static state tracking for max health changes that require save/reload
        private static int startingMaxHealth = -1;
        
        // Configuration entries for player cheat settings
        private static ConfigEntry<bool> invulnerabilityEnabled;
        private static ConfigEntry<bool> oneHitKillsEnabled;
        private static ConfigEntry<bool> infiniteSoulEnabled;
        private static ConfigEntry<bool> infiniteGeoEnabled;
        
        /// <summary>
        /// Initializes the configuration entries.
        /// </summary>
        private static void InitializeConfig()
        {
            invulnerabilityEnabled = CabbyCodesPlugin.configFile.Bind("Player", "Invulnerability", false, 
                "Enable player invulnerability");
            oneHitKillsEnabled = CabbyCodesPlugin.configFile.Bind("Player", "OneHitKills", false, 
                "Enable one-hit kills for enemies");
            infiniteSoulEnabled = CabbyCodesPlugin.configFile.Bind("Player", "InfiniteSoul", false, 
                "Enable infinite soul");
            infiniteGeoEnabled = CabbyCodesPlugin.configFile.Bind("Player", "InfiniteGeo", false, 
                "Enable infinite geo");
        }
        
        public static void AddPanels()
        {
            // Initialize configuration
            InitializeConfig();
            
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

            // Create patch instances and check if they should be initially active
            var invulPatch = new InvulPatch();
            var damagePatch = new DamagePatch();
            var soulPatch = new SoulPatch();
            var geoPatch = new GeoPatch();

            // Activate patches if they were enabled in config
            if (invulnerabilityEnabled.Value)
            {
                invulPatch.Set(true);
            }
            if (oneHitKillsEnabled.Value)
            {
                damagePatch.Set(true);
            }
            if (infiniteSoulEnabled.Value)
            {
                soulPatch.Set(true);
            }
            if (infiniteGeoEnabled.Value)
            {
                geoPatch.Set(true);
            }

            var panels = new List<CheatPanel>
            {
                new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor),
                new TogglePanel(invulPatch, FlagInstances.isInvincible.ReadableName),
                new InfoPanel("WARNING: Do not use OHKO on False Knight, Failed Champion, or Nailmasters").SetColor(CheatPanel.warningColor),
                new TogglePanel(damagePatch, "One Hit Kills (Enemies can't block)"),
                new TogglePanel(new JumpPatch(), FlagInstances.infiniteAirJump.ReadableName),
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
                new TogglePanel(soulPatch, "Infinite Soul"),
                new TogglePanel(geoPatch, "Infinite Geo"),
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
