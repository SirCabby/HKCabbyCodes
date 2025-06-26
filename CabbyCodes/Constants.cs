namespace CabbyCodes
{
    /// <summary>
    /// Centralized constants for the CabbyCodes mod.
    /// </summary>
    public static class Constants
    {
        // Plugin Information
        public const string GUID = "cabby.cabbycodes";
        public const string NAME = "Cabby Codes";
        public const string VERSION = "0.0.1";

        // UI Constants
        public const int DEFAULT_CHARACTER_LIMIT = 1;
        public const int DEFAULT_PANEL_WIDTH = 120;
        public const int DEFAULT_PANEL_HEIGHT = 60;
        public const int CLICK_TIMER_DELAY = 100;

        // Game Limits
        public const int MIN_HEALTH = 5;
        public const int MAX_HEALTH = 9;
        public const int MIN_GEO = 0;
        public const int MAX_GEO = 9999999;
        public const int MIN_DREAM_ESSENCE = 0;
        public const int MAX_DREAM_ESSENCE = 9999;

        // Hunter Journal
        public const int MIN_HUNTER_KILLS = 0;
        public const int MAX_HUNTER_KILLS = 99;

        // Currency Limits
        public const int MAX_WANDERERS_JOURNALS = 14;
        public const int MAX_HALLOWNEST_SEALS = 17;
        public const int MAX_KINGS_IDOLS = 8;
        public const int MAX_ARCANE_EGGS = 4;
        public const int MAX_RANCID_EGGS = 80;
        public const int MAX_PALE_ORE = 6;

        // Upgrade Limits
        public const int MAX_MASK_SHARDS = 3;
        public const int MAX_VESSEL_FRAGMENTS = 2;
        public const int MAX_SIMPLE_KEYS = 4;
        public const int MIN_CHARM_NOTCHES = 3;
        public const int MAX_CHARM_NOTCHES = 11;
    }
}