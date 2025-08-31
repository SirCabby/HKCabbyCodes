using System;

namespace CabbyCodes.SavedGames
{
    /// <summary>
    /// Serializable class to store menu state data.
    /// </summary>
    [Serializable]
    public class MenuState
    {
        public int? MainCategoryIndex { get; set; }
        public int? FlagsCategoryIndex { get; set; }
        public int? PlayerFlagPage { get; set; }
        
        public MenuState()
        {
            MainCategoryIndex = null;
            FlagsCategoryIndex = null;
            PlayerFlagPage = null;
        }
    }
} 