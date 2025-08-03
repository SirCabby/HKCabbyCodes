using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory
{
    public class DreamNailReference : ISyncedValueList
    {
        public int Get()
        {
            if (FlagManager.GetBoolFlag(FlagInstances.dreamNailUpgraded))
                return 2;
            else if (FlagManager.GetBoolFlag(FlagInstances.hasDreamNail))
                return 1;
            return 0;
        }

        public void Set(int value)
        {
            if (value == 2)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, true);
            }
            else if (value == 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, false);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string> { "NONE", FlagInstances.hasDreamNail.ReadableName, FlagInstances.dreamNailUpgraded.ReadableName };
        }
    }
} 