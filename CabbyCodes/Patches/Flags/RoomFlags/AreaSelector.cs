using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches.Flags.RoomFlags
{
    public class AreaSelector : ISyncedReference<int>, ISyncedValueList
    {
        private int currentIndex;
        private readonly List<string> areaNames;
        private bool showAllFlags;

        public AreaSelector(int defaultIndex = 0, bool showAllFlags = false)
        {
            this.showAllFlags = showAllFlags;
            areaNames = showAllFlags 
                ? Scenes.SceneManagement.GetAllAreaFlags().Keys.ToList()
                : Scenes.SceneManagement.GetAreaFlags().Keys.ToList();
            currentIndex = defaultIndex >= 0 && defaultIndex < areaNames.Count ? defaultIndex : 0;
        }

        public int Get() => currentIndex;
        public void Set(int value) => currentIndex = value >= 0 && value < areaNames.Count ? value : 0;
        public List<string> GetValueList() => new List<string>(areaNames);
        
        /// <summary>
        /// Updates the area names list based on the showAllFlags setting and rebuilds the list.
        /// </summary>
        /// <param name="newShowAllFlags">Whether to show all flags including unused ones.</param>
        public void UpdateFlagList(bool newShowAllFlags)
        {
            if (showAllFlags != newShowAllFlags)
            {
                showAllFlags = newShowAllFlags;
                var newAreaNames = showAllFlags 
                    ? Scenes.SceneManagement.GetAllAreaFlags().Keys.ToList()
                    : Scenes.SceneManagement.GetAreaFlags().Keys.ToList();
                
                string currentAreaName = areaNames.Count > 0 && currentIndex < areaNames.Count 
                    ? areaNames[currentIndex] 
                    : null;
                
                areaNames.Clear();
                areaNames.AddRange(newAreaNames);
                
                if (!string.IsNullOrEmpty(currentAreaName) && areaNames.Contains(currentAreaName))
                {
                    currentIndex = areaNames.IndexOf(currentAreaName);
                }
                else
                {
                    currentIndex = areaNames.Count > 0 ? 0 : 0;
                }
            }
        }
    }
} 