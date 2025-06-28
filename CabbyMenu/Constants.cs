namespace CabbyMenu
{
    public static class Constants
    {
        // UI Constants
        public const int DEFAULT_CHARACTER_LIMIT = 1;
        public const int DEFAULT_PANEL_WIDTH = 120;
        public const int DEFAULT_PANEL_HEIGHT = 60;
        public const double CLICK_TIMER_DELAY = 0.2;

        // Font Sizes
        public const int DEFAULT_FONT_SIZE = 36;
        public const int TITLE_FONT_SIZE = 60;
        public const int CLOSE_BUTTON_FONT_SIZE = 46;
        public const int DROPDOWN_SHOW_SIZE = 10;

        // UI Dimensions
        public const int DEFAULT_ICON_SIZE = 60;
        public const int DEFAULT_TOGGLE_BUTTON_SIZE = 120;
        public const int CHEAT_PANEL_HEIGHT = 50;
        public const int CATEGORY_TEXT_WIDTH = 400;
        public const int CATEGORY_TEXT_HEIGHT = 100;
        public const int CATEGORY_DROPDOWN_WIDTH = 280;
        public const int CATEGORY_DROPDOWN_HEIGHT = 60;
        public const int TITLE_TEXT_WIDTH = 400;
        public const int TITLE_TEXT_HEIGHT = 100;
        public const int VERSION_TEXT_WIDTH = 400;
        public const int VERSION_TEXT_HEIGHT = 100;
        public const int MAX_CATEGORY_SHOW_SIZE = 9;

        // Canvas Settings
        public const int REFERENCE_RESOLUTION_WIDTH = 2560;
        public const int REFERENCE_RESOLUTION_HEIGHT = 1440;

        // Anchor Positions
        public const float MENU_BUTTON_MIN_X = 0.07f;
        public const float MENU_BUTTON_MAX_X = 0.12f;
        public const float MENU_BUTTON_MIN_Y = 0.92f;
        public const float MENU_BUTTON_MAX_Y = 0.95f;
        public const float CATEGORY_TEXT_X = 0.11f;
        public const float CATEGORY_TEXT_Y = 0.74f;
        public const float CATEGORY_DROPDOWN_X = 0.08f;
        public const float CATEGORY_DROPDOWN_Y = 0.72f;
        public const float CHEAT_SCROLLABLE_MIN_X = 0.17f;
        public const float CHEAT_SCROLLABLE_MAX_X = 0.8f;
        public const float CHEAT_SCROLLABLE_MIN_Y = 0.05f;
        public const float CHEAT_SCROLLABLE_MAX_Y = 0.9f;
        public const float TITLE_TEXT_X = 0.5f;
        public const float TITLE_TEXT_Y = 0.93f;
        public const float CLOSE_BUTTON_MIN_X = 0.87f;
        public const float CLOSE_BUTTON_MAX_X = 0.92f;
        public const float CLOSE_BUTTON_MIN_Y = 0.68f;
        public const float CLOSE_BUTTON_MAX_Y = 0.7f;
        public const float VERSION_TEXT_X = 0.95f;
        public const float VERSION_TEXT_MIN_Y = 0.05f;
        public const float VERSION_TEXT_MAX_Y = 0.1f;
        public const float MIDDLE_ANCHOR = 0.5f;

        // Layout Settings
        public const int CHEAT_PANEL_PADDING = 20;
        public const int CHEAT_PANEL_SPACING = 50;
        public const int CHEAT_CONTENT_PADDING = 10;
        public const int CHEAT_CONTENT_SPACING = 10;

        // Colors
        public static readonly UnityEngine.Color ON_COLOR = new(0f, 0.8f, 1f, 1f);
        public static readonly UnityEngine.Color ON_HOVER_COLOR = new(0.3f, 1f, 1f, 1f);
        public static readonly UnityEngine.Color ON_PRESSED_COLOR = new(0f, 0.6f, 0.8f, 1f);
        public static readonly UnityEngine.Color OFF_COLOR = new(0.6f, 0.6f, 0.6f, 1f);
        public static readonly UnityEngine.Color OFF_HOVER_COLOR = new(0.8f, 0.8f, 0.8f, 1f);
        public static readonly UnityEngine.Color OFF_PRESSED_COLOR = new(0.4f, 0.4f, 0.4f, 1f);
        public static readonly UnityEngine.Color SELECTED_COLOR = new(1f, 1f, 0.56f, 1f);
        public static readonly UnityEngine.Color NORMAL_SCROLLBAR_COLOR = new(0.4f, 0.4f, 0.4f, 1f);
        public static readonly UnityEngine.Color HIGHLIGHT_SCROLLBAR_COLOR = new(0.6f, 0.6f, 0.6f, 1f);
        public static readonly UnityEngine.Color PRESSED_SCROLLBAR_COLOR = new(0.2f, 0.2f, 0.2f, 1f);
        public static readonly UnityEngine.Color PANEL_COLOR_1 = new(0.8f, 0.8f, 0.8f, 1f);
        public static readonly UnityEngine.Color PANEL_COLOR_2 = new(0.6f, 0.6f, 0.6f, 1f);
        public static readonly UnityEngine.Color WARNING_COLOR = new(1f, 0.5f, 0f, 1f);
        public static readonly UnityEngine.Color HEADER_COLOR = new(0.2f, 0.8f, 0.2f, 1f);
        public static readonly UnityEngine.Color SUB_HEADER_COLOR = new(0.5f, 0.5f, 0.8f, 1f);
        public static readonly UnityEngine.Color MENU_PANEL_COLOR = new(0f, 0f, 0f, 0.8f);
        public static readonly UnityEngine.Color UNEARNED_COLOR = new(0.57f, 0.57f, 0.57f, 0.57f);

        // Button Colors
        public static readonly UnityEngine.Color BUTTON_PRIMARY_NORMAL = new(0.2f, 0.4f, 0.8f, 1f);
        public static readonly UnityEngine.Color BUTTON_PRIMARY_HOVER = new(0.4f, 0.6f, 1f, 1f);
        public static readonly UnityEngine.Color BUTTON_PRIMARY_PRESSED = new(0.1f, 0.2f, 0.6f, 1f);
        public static readonly UnityEngine.Color BUTTON_SUCCESS_NORMAL = new(0.2f, 0.7f, 0.3f, 1f);
        public static readonly UnityEngine.Color BUTTON_SUCCESS_HOVER = new(0.4f, 0.9f, 0.5f, 1f);
        public static readonly UnityEngine.Color BUTTON_SUCCESS_PRESSED = new(0.1f, 0.5f, 0.1f, 1f);
        public static readonly UnityEngine.Color BUTTON_DANGER_NORMAL = new(0.8f, 0.2f, 0.2f, 1f);
        public static readonly UnityEngine.Color BUTTON_DANGER_HOVER = new(1f, 0.4f, 0.4f, 1f);
        public static readonly UnityEngine.Color BUTTON_DANGER_PRESSED = new(0.6f, 0.1f, 0.1f, 1f);

        // Dropdown Colors
        public static readonly UnityEngine.Color DROPDOWN_NORMAL = new(0.7f, 0.7f, 0.7f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_HOVER = new(1f, 1f, 1f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_PRESSED = new(0.4f, 0.4f, 0.4f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_ITEM_NORMAL = new(0f, 0f, 0f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_ITEM_HOVER = new(0f, 0f, 0f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_ITEM_PRESSED = new(0f, 0f, 0f, 1f);
        public static readonly UnityEngine.Color DROPDOWN_PANEL_BACKGROUND = new(0.1f, 0.1f, 0.1f, 0.9f);
        public static readonly UnityEngine.Color DROPDOWN_OPTION_BACKGROUND = new(0.7f, 0.7f, 0.7f, 1f);

        // Scrollable Area Colors
        public static readonly UnityEngine.Color CHEAT_SCROLLABLE_BACKGROUND = new(0.1f, 0.1f, 0.1f, 0.3f);

        // UI Vector2 Constants
        public static readonly UnityEngine.Vector2 MIDDLE_ANCHOR_VECTOR = new(MIDDLE_ANCHOR, MIDDLE_ANCHOR);
        public static readonly UnityEngine.Vector2 DEFAULT_PANEL_SIZE = new(DEFAULT_PANEL_WIDTH, DEFAULT_PANEL_HEIGHT);
        public static readonly UnityEngine.Vector2 DEFAULT_ICON_SIZE_VECTOR = new(DEFAULT_ICON_SIZE, DEFAULT_ICON_SIZE);

        // Boolean/Int Defaults
        public const float DEFAULT_FLOAT_VALUE = 0f;
        public const int FLEXIBLE_LAYOUT_VALUE = 1;
    }
}