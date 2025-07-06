using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Maps
{
    public class MapPatch : ISyncedReference<bool>
    {
        private readonly string mapName;

        public MapPatch(string mapName)
        {
            this.mapName = mapName;
        }

        public bool Get()
        {
            return (bool)typeof(PlayerData).GetField(mapName, BindingFlags.Public | BindingFlags.Instance).GetValue(PlayerData.instance);
        }

        public void Set(bool value)
        {
            typeof(PlayerData).GetField(mapName, BindingFlags.Public | BindingFlags.Instance).SetValue(PlayerData.instance, value);
        }

        private static void AddMapPanels()
        {
            Type type = typeof(PlayerData).Assembly.GetType("PlayerData+MapBools");
            foreach (string name in Enum.GetNames(type))
            {
                string fixedName = char.ToLower(name[0]).ToString() + name.Substring(1);
                TogglePanel buttonPanel = new TogglePanel(new MapPatch(fixedName), fixedName.Substring(3));
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
            }
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Maps: Enable to have map for area").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Still requires Quill item to fill maps out").SetColor(CheatPanel.warningColor));
            AddMapPanels();
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Rooms: Enable to have room mapped out").SetColor(CheatPanel.headerColor));
            
            // Add area selector dropdown
            MapAreaSelector areaSelector = new MapAreaSelector();
            DropdownPanel areaDropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(areaDropdownPanel);
            
            // Add dynamic room manager
            DynamicMapRoomManager roomManager = new DynamicMapRoomManager(areaSelector);
            roomManager.AddAllPanelsToMenu();
            
            // Add update action to handle area changes (for menu update loop)
            areaDropdownPanel.updateActions.Add(roomManager.UpdateVisibleArea);
            // Hook dropdown value change event for immediate update
            areaDropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => roomManager.UpdateVisibleArea());
        }
    }
}
