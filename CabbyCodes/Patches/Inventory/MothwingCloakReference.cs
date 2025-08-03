using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory
{
    public class MothwingCloakReference : ISyncedValueList
    {
        public int Get()
        {
            if (!FlagManager.GetBoolFlag(FlagInstances.hasDash))
                return 0;
            else if (!FlagManager.GetBoolFlag(FlagInstances.hasShadowDash))
                return 1;
            return 2;
        }

        public void Set(int value)
        {
            if (value > 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, true);
            }
            else if (value == 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, false);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string> { "NONE", FlagInstances.hasDash.ReadableName, FlagInstances.hasShadowDash.ReadableName };
        }
    }
} 