using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Popups;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CabbyCodes.Patches.Flags
{
    public class GeoRocksFlagPatch : BasePatch
    {
        private static List<FlagDef> allGeoRockFlags;

        protected override FlagDef[] GetFlags()
        {
            // Initialize the list of all geo rock flags if not already done
            if (allGeoRockFlags == null)
            {
                allGeoRockFlags = GetAllGeoRockFlags();
            }
            
            return allGeoRockFlags.ToArray();
        }

        private static List<FlagDef> GetAllGeoRockFlags()
        {
            var flags = new List<FlagDef>();

            // Get all static fields from FlagInstances class using reflection
            var flagInstancesType = typeof(FlagInstances);
            var staticFields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in staticFields)
            {
                try
                {
                    if (field.FieldType == typeof(FlagDef))
                    {
                        var flagDef = (FlagDef)field.GetValue(null);
                        if (flagDef != null && flagDef.Type == "GeoRockData")
                        {
                            flags.Add(flagDef);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[GeoRocksFlagPatch] Error accessing field {field.Name}: {ex.Message}");
                }
            }

            // Sort by scene name and then by readable name for better organization
            return flags.OrderBy(f => f.SceneName).ThenBy(f => f.ReadableName).ToList();
        }
        
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();
            
            // Add the "Set all Geo Rocks to 0" button panel at the beginning
            var resetAllButton = new ButtonPanel(() =>
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu != null)
                {
                    string msg = "This will set all Geo Rocks to 0 hits remaining.\n\nAre you sure you want to proceed?";
                    var popup = new ConfirmationPopup(
                        menu,
                        "Confirm Reset",
                        msg,
                        "Yes",
                        "No",
                        () => 
                        {
                            // Set all geo rocks to 0
                            SetAllGeoRocksToZero();
                            // Update all panels to refresh their values
                            menu.UpdateCheatPanels();
                        },
                        null);
                    popup.Show();
                }
                else
                {
                    // Fallback if menu is not available
                    SetAllGeoRocksToZero();
                }
            }, "Activate", "Set all Geo Rocks to 0");
            
            panels.Add(resetAllButton);
            
            // Add all the individual geo rock panels
            var flags = GetFlags();
            foreach (var flag in flags)
            {
                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            
            return panels;
        }
        
        private void SetAllGeoRocksToZero()
        {
            if (SceneData.instance?.geoRocks == null) return;
            
            var flags = GetFlags();
            foreach (var flag in flags)
            {
                // For GeoRockData flags, we need to directly update the SceneData.instance.geoRocks data structure
                // This is the same approach used by GeoRockPatch.Set()
                foreach (var grd in SceneData.instance.geoRocks)
                {
                    if (grd.id == flag.Id && grd.sceneName == flag.SceneName)
                    {
                        grd.hitsLeft = 0;
                        break;
                    }
                }
            }
        }
        
        protected override string GetDescription(FlagDef flag)
        {
            var sceneDisplayName = flag.Scene?.ReadableName ?? flag.SceneName;
            return $"{sceneDisplayName}: {flag.ReadableName}";
        }
    }
} 