using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory
{
    public class DelicateFlowerReference : ISyncedValueList
    {
        public int Get()
        {
            if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && !FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken))
                return 2;
            else if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken))
                return 1;
            return 0;
        }

        public void Set(int value)
        {
            if (value == 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, true);
            }
            else if (value == 2)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, false);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string> { "NONE", FlagInstances.xunFlowerBroken.ReadableName, FlagInstances.hasXunFlower.ReadableName };
        }
    }
} 