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
        public const string VERSION = "1.0.2";

        // Game Limits
        public const int MIN_HEALTH = 5;
        public const int MAX_HEALTH = 9;
        public const int MIN_GEO = 0;
        public const int MAX_GEO = 999999;
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
        public const int MAX_NAIL_UPGRADES = 4;

        // Magic Numbers
        public const int INFINITE_SOUL_CHARGE = 999;
        public const int ONE_HIT_KILL_DAMAGE = 9999;
        public const int MAX_PLAYTIME_DIGITS = 9;

        // UI Dimensions
        public const int TELEPORT_DROPDOWN_WIDTH = 400;
        public const int TELEPORT_BUTTON_WIDTH = 160;
        public const int TELEPORT_DESTROY_BUTTON_SIZE = 60;
        public const int HUNTER_INPUT_WIDTH = 200;
        public const int HUNTER_UNLOCK_BUTTON_WIDTH = 200;
        public const int HUNTER_LOCK_BUTTON_WIDTH = 160;
        public const int HUNTER_ZERO_BUTTON_WIDTH = 120;
        public const int HUNTER_ONE_BUTTON_WIDTH = 120;
        public const int MAP_ROOM_BUTTON_SIZE = 120;
        public const int NAIL_DROPDOWN_WIDTH = 350;
        public const int SPELL_DROPDOWN_WIDTH = 350;
        public const int CHARM_DROPDOWN_WIDTH = 230;
        public const int GRIMM_CHILD_DROPDOWN_WIDTH = 120;
        public const int ROYAL_CHARM_DROPDOWN_WIDTH = 230;
        public const int DEFAULT_PANEL_HEIGHT = 60;
        public const int PANEL_WIDTH_120 = 120;
        public const int PANEL_WIDTH_180 = 180;
        public const int MAX_PLAYTIME_PANEL_WIDTH = 180;

        // Royal Charm States
        public const int ROYAL_CHARM_STATE_MIN = 0;
        public const int ROYAL_CHARM_STATE_MAX = 3;

        // Teleport Locations
        public const float TOWN_X_POSITION = 136f;
        public const float TOWN_Y_POSITION = 12f;

        // Common Vector2 Constants
        public static readonly UnityEngine.Vector2 MIDDLE_ANCHOR_VECTOR = new UnityEngine.Vector2(0.5f, 0.5f);
        public static readonly UnityEngine.Vector2 DEFAULT_PANEL_SIZE = new UnityEngine.Vector2(120, 60);
        public static readonly UnityEngine.Vector2 DEFAULT_ICON_SIZE = new UnityEngine.Vector2(60, 60);

        // Boolean/Int Defaults
        public const int PERMADEATH_MODE_ENABLED = 1;
        public const int DELICATE_FLOWER_BROKEN_STATE = 1;
        public const int DELICATE_FLOWER_RETURNED_STATE = 2;

        // Dev Options
        public const bool DEFAULT_DEV_OPTIONS_ENABLED = false;
    }
}