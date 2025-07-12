using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPatch : ISyncedReference<bool>
    {
        public static readonly Dictionary<string, List<string>> roomsInMaps = GetAreaToScenesMapping();

        private readonly string roomName;

        public MapRoomPatch(string roomName)
        {
            this.roomName = roomName;
        }

        public bool Get()
        {
            return FlagManager.ListFlagContains("scenesMapped", "Global", roomName);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                FlagManager.AddToListFlag("scenesMapped", "Global", roomName);
            }
            else if (!value && Get())
            {
                FlagManager.RemoveFromListFlag("scenesMapped", "Global", roomName);
            }
        }

        public static void AddPanels()
        {
            // Add area selector dropdown
            MapAreaSelector areaSelector = new MapAreaSelector();
            DropdownPanel areaDropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(areaDropdownPanel);

            // Add dynamic room manager
            MapRoomPanelManager roomManager = new MapRoomPanelManager(areaSelector);
            roomManager.AddAllPanelsToMenu();

            // Add update action to handle area changes (for menu update loop)
            areaDropdownPanel.updateActions.Add(roomManager.UpdateVisibleArea);
            // Hook dropdown value change event for immediate update
            areaDropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => roomManager.UpdateVisibleArea());
        }
    }
}
