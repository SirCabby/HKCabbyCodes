using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches.Flags.RoomFlags
{
    public class AreaSelector : ISyncedReference<int>, ISyncedValueList
    {
        private int currentIndex;
        private readonly List<string> areaNames;

        public AreaSelector(int defaultIndex = 0)
        {
            areaNames = Scenes.SceneManagement.GetAreaFlags().Keys.ToList();
            currentIndex = defaultIndex >= 0 && defaultIndex < areaNames.Count ? defaultIndex : 0;
        }

        public int Get() => currentIndex;
        public void Set(int value) => currentIndex = value >= 0 && value < areaNames.Count ? value : 0;
        public List<string> GetValueList() => new List<string>(areaNames);
    }
} 