using UnityEngine;

namespace CabbyMenu
{
    public static class Constants
    {
        // UI Constants
        public const int DEFAULT_CHARACTER_LIMIT = 1;
        public const int MIN_PANEL_WIDTH = 120;
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
        public const int CATEGORY_DROPDOWN_WIDTH = 300;
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
        public const float CATEGORY_DROPDOWN_X = 0.03f;
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
        public static readonly Color ON_COLOR = new Color(0f, 0.8f, 1f, 1f);
        public static readonly Color ON_HOVER_COLOR = new Color(0.3f, 1f, 1f, 1f);
        public static readonly Color ON_PRESSED_COLOR = new Color(0f, 0.6f, 0.8f, 1f);
        public static readonly Color OFF_COLOR = new Color(0.6f, 0.6f, 0.6f, 1f);
        public static readonly Color OFF_HOVER_COLOR = new Color(0.8f, 0.8f, 0.8f, 1f);
        public static readonly Color OFF_PRESSED_COLOR = new Color(0.4f, 0.4f, 0.4f, 1f);
        public static readonly Color SELECTED_COLOR = new Color(1f, 1f, 0.56f, 1f);
        public static readonly Color NORMAL_SCROLLBAR_COLOR = new Color(0.4f, 0.4f, 0.4f, 1f);
        public static readonly Color HIGHLIGHT_SCROLLBAR_COLOR = new Color(0.6f, 0.6f, 0.6f, 1f);
        public static readonly Color PRESSED_SCROLLBAR_COLOR = new Color(0.2f, 0.2f, 0.2f, 1f);
        public static readonly Color PANEL_COLOR_1 = new Color(0.8f, 0.8f, 0.8f, 1f);
        public static readonly Color PANEL_COLOR_2 = new Color(0.6f, 0.6f, 0.6f, 1f);
        public static readonly Color WARNING_COLOR = new Color(1f, 0.5f, 0f, 1f);
        public static readonly Color HEADER_COLOR = new Color(0.2f, 0.8f, 0.2f, 1f);
        public static readonly Color SUB_HEADER_COLOR = new Color(0.5f, 0.5f, 0.8f, 1f);
        public static readonly Color MENU_PANEL_COLOR = new Color(0f, 0f, 0f, 0.8f);
        public static readonly Color UNEARNED_COLOR = new Color(0.57f, 0.57f, 0.57f, 0.57f);

        // Button Colors
        public static readonly Color BUTTON_PRIMARY_NORMAL = new Color(0.2f, 0.4f, 0.8f, 1f);
        public static readonly Color BUTTON_PRIMARY_HOVER = new Color(0.4f, 0.6f, 1f, 1f);
        public static readonly Color BUTTON_PRIMARY_PRESSED = new Color(0.1f, 0.2f, 0.6f, 1f);
        public static readonly Color BUTTON_SUCCESS_NORMAL = new Color(0.2f, 0.7f, 0.3f, 1f);
        public static readonly Color BUTTON_SUCCESS_HOVER = new Color(0.4f, 0.9f, 0.5f, 1f);
        public static readonly Color BUTTON_SUCCESS_PRESSED = new Color(0.1f, 0.5f, 0.1f, 1f);
        public static readonly Color BUTTON_DANGER_NORMAL = new Color(0.8f, 0.2f, 0.2f, 1f);
        public static readonly Color BUTTON_DANGER_HOVER = new Color(1f, 0.4f, 0.4f, 1f);
        public static readonly Color BUTTON_DANGER_PRESSED = new Color(0.6f, 0.1f, 0.1f, 1f);

        // Dropdown Colors
        public static readonly Color DROPDOWN_NORMAL = new Color(0.9f, 0.9f, 0.9f, 1f);
        public static readonly Color DROPDOWN_HOVER = new Color(1f, 1f, 1f, 1f);
        public static readonly Color DROPDOWN_PRESSED = new Color(0.4f, 0.4f, 0.4f, 1f);
        public static readonly Color DROPDOWN_PANEL_BACKGROUND = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        public static readonly Color DROPDOWN_OPTION_BACKGROUND = new Color(0.7f, 0.7f, 0.7f, 1f);
        public static readonly Color DROPDOWN_OPTION_HOVER = new Color(0.7f, 0.8f, 1f, 1f);
        public static readonly Color DROPDOWN_OPTION_PRESSED = new Color(0.4f, 0.6f, 1f, 1f);

        // Scrollable Area Colors
        public static readonly Color CHEAT_SCROLLABLE_BACKGROUND = new Color(0.1f, 0.1f, 0.1f, 0.3f);

        // UI Vector2 Constants
        public static readonly Vector2 MIDDLE_ANCHOR_VECTOR = new Vector2(MIDDLE_ANCHOR, MIDDLE_ANCHOR);
        public static readonly Vector2 DEFAULT_PANEL_SIZE = new Vector2(MIN_PANEL_WIDTH, DEFAULT_PANEL_HEIGHT);
        public static readonly Vector2 DEFAULT_ICON_SIZE_VECTOR = new Vector2(DEFAULT_ICON_SIZE, DEFAULT_ICON_SIZE);

        // Boolean/Int Defaults
        public const float DEFAULT_FLOAT_VALUE = 0f;
        public const int FLEXIBLE_LAYOUT_VALUE = 1;
    }
}