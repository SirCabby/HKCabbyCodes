using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Geo rock patch implementation that controls the hits left for geo rocks
    /// </summary>
    public class GeoRockPatch : ISyncedReference<int>, IPatch
    {
        private readonly FlagDef flag;
        private readonly string description;
        
        public GeoRockPatch(FlagDef flagDef, string description = null)
        {
            flag = flagDef;
            this.description = description ?? flagDef.ReadableName;
        }
        
        public int Get()
        {
            if (SceneData.instance?.geoRocks == null) return 0;
            foreach (var grd in SceneData.instance.geoRocks)
                if (grd.id == flag.Id && grd.sceneName == flag.SceneName)
                    return grd.hitsLeft;
            return 0;
        }
        
        public void Set(int value)
        {
            if (SceneData.instance?.geoRocks == null) return;
            foreach (var grd in SceneData.instance.geoRocks)
                if (grd.id == flag.Id && grd.sceneName == flag.SceneName)
                {
                    grd.hitsLeft = value;
                    break;
                }
        }
        
        public CheatPanel CreatePanel()
        {
            return new RangeInputFieldPanel<int>(
                this, 
                KeyCodeMap.ValidChars.Numeric, 
                0, 
                10, 
                description + " Hits Left"
            );
        }
    }
} 