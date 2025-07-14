using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using static CabbyCodes.Scenes.SceneManagement;

namespace CabbyCodes.Patches.Maps
{
    public class MapAreaSelector : ISyncedValueList
    {
        private static readonly List<string> areaNames = GetAreaNames().ToList();
        private int selectedAreaIndex = 0;

        public MapAreaSelector()
        {
            // Set to current player area on initialization
            SetToCurrentPlayerArea();
        }

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

        private void SetToCurrentPlayerArea()
        {
            try
            {
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                var sceneData = GetSceneData(currentScene);
                if (sceneData != null)
                {
                    var currentAreaIndex = areaNames.IndexOf(sceneData.AreaName);
                    if (currentAreaIndex >= 0)
                    {
                        selectedAreaIndex = currentAreaIndex;
                    }
                }
            }
            catch
            {
                // If we can't get the current area, default to 0
                selectedAreaIndex = 0;
            }
        }
    }
} 