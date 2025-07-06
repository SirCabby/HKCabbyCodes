using CabbyMenu.SyncedReferences;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Maps
{
    public class MapAreaSelector : ISyncedValueList
    {
        private static readonly List<string> areaNames = new List<string>
        {
            "Dirtmouth",
            "Abyss",
            "City",
            "Cliffs",
            "Crossroads",
            "Mines",
            "Deepnest",
            "FogCanyon",
            "FungalWastes",
            "Greenpath",
            "Outskirts",
            "RoyalGardens",
            "RestingGrounds",
            "Waterways"
        };

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
            return "Dirtmouth"; // Default fallback
        }
    }
} 