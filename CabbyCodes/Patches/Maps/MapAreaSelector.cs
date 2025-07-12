using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Scenes;
using static CabbyCodes.Scenes.Scenes;

namespace CabbyCodes.Patches.Maps
{
    public class MapAreaSelector : ISyncedValueList
    {
        private static readonly List<string> areaNames = GetAreaNames().ToList();
        private int selectedAreaIndex = 0;

        public int Get()
        {
            return selectedAreaIndex;
        }

        public void Set(int value)
        {
            if (value >= 0 && value < areaNames.Count)
            {
                selectedAreaIndex = value;
            }
        }

        public List<string> GetValueList()
        {
            return areaNames;
        }

        public string GetSelectedAreaName()
        {
            if (selectedAreaIndex >= 0 && selectedAreaIndex < areaNames.Count)
            {
                return areaNames[selectedAreaIndex];
            }
            // Default fallback - get the first area name from the list
            return areaNames.Count > 0 ? areaNames[0] : AreaInstances.Dirtmouth.MapName;
        }
    }
} 