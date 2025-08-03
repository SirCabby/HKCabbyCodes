using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Charms
{
    /// <summary>
    /// Custom BoolPatch for charm toggles that manages charmsOwned and hasCharm flags
    /// </summary>
    public class CharmTogglePatch : BoolPatch
    {
        public CharmTogglePatch(FlagDef flagDef) : base(flagDef)
        {
        }

        public override void Set(bool value)
        {
            // Get current charm count before changing the flag
            int currentCharmsOwned = FlagManager.GetIntFlag(FlagInstances.charmsOwned);
            bool wasCharmOwned = FlagManager.GetBoolFlag(flag);
            
            // Set the charm flag
            FlagManager.SetBoolFlag(flag, value);
            
            // Update charmsOwned count
            if (value && !wasCharmOwned)
            {
                // Charm is being turned on - increment count
                FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned + 1);
                
                // If this is the first charm, set hasCharm to true
                if (currentCharmsOwned == 0)
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasCharm, true);
                }
            }
            else if (!value && wasCharmOwned)
            {
                // Charm is being turned off - decrement count
                FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned - 1);
                
                // If this was the last charm, set hasCharm to false
                if (currentCharmsOwned == 1)
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasCharm, false);
                }
            }
        }
    }
} 