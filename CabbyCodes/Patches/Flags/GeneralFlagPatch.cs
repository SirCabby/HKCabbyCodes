using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class GeneralFlagPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.crossroadsInfected;
        public bool Get() => FlagManager.GetBoolFlag(flag);
        public void Set(bool value) => FlagManager.SetBoolFlag(flag, value);
        public static List<CheatPanel> CreatePanels() => new List<CheatPanel>{ new TogglePanel(new GeneralFlagPatch(), flag.ReadableName) };
    }
} 