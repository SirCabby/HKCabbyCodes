using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;
using System.Collections.Generic;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Base class for patches that can handle multiple flags with automatic type detection
    /// </summary>
    public abstract class BasePatch
    {
        /// <summary>
        /// Virtual method to get the description for a flag. Can be overridden by child classes.
        /// </summary>
        /// <param name="flag">The flag to get description for</param>
        /// <returns>The description string</returns>
        protected virtual string GetDescription(FlagDef flag)
        {
            return flag.ReadableName;
        }
        
        /// <summary>
        /// Creates panels for this patch. Override this for custom panel organization.
        /// </summary>
        /// <returns>List of panels to display</returns>
        public virtual List<CheatPanel> CreatePanels()
        {
            var flags = GetFlags();
            var panels = new List<CheatPanel>();
            
            foreach (var flag in flags)
            {
                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            
            return panels;
        }
        
        /// <summary>
        /// Gets the flags for this patch. Override this for simple flag-based patches.
        /// </summary>
        /// <returns>Array of flags to create panels for</returns>
        protected virtual FlagDef[] GetFlags()
        {
            return new FlagDef[0];
        }
        
        protected virtual IPatch CreatePatch(FlagDef flag)
        {
            // Determine type from FlagDef's type
            switch (flag.Type)
            {
                case "PlayerData_Bool":
                    return new BoolPatch(flag, GetDescription(flag));
                case "PlayerData_Int":
                    return new IntPatch(flag, GetDescription(flag));
                case "PlayerData_Single":
                    return new FloatPatch(flag, GetDescription(flag));
                case "PersistentBoolData":
                    return new BoolPatch(flag, GetDescription(flag));
                case "PersistentIntData":
                    return new IntPatch(flag, GetDescription(flag));
                case "GeoRockData":
                    return new GeoRockPatch(flag, GetDescription(flag));
                default:
                    return new BoolPatch(flag, GetDescription(flag)); // Default to bool
            }
        }
    }
} 