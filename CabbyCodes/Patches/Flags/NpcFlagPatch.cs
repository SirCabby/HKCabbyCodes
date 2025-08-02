using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class NpcFlagPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.Crossroads_45__Zombie_Myla;
        public bool Get() => FlagManager.GetBoolFlag(flag);
        public void Set(bool value) => FlagManager.SetBoolFlag(flag, value);
        public static List<CheatPanel> CreatePanels() => new List<CheatPanel>{ new TogglePanel(new NpcFlagPatch(), flag.ReadableName) };
    }
} 