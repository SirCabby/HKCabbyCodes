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
        protected abstract FlagDef[] GetFlags();
        
        /// <summary>
        /// Virtual method to get the description for a flag. Can be overridden by child classes.
        /// </summary>
        /// <param name="flag">The flag to get description for</param>
        /// <returns>The description string</returns>
        protected virtual string GetDescription(FlagDef flag)
        {
            return flag.ReadableName;
        }
        
        public virtual List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();
            
            // Get the flags from the derived class
            var flags = GetFlags();
            
            foreach (var flag in flags)
            {
                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            
            return panels;
        }
        
        protected IPatch CreatePatch(FlagDef flag)
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
                    return new BoolPatch(flag, GetDescription(flag));
                default:
                    return new BoolPatch(flag, GetDescription(flag)); // Default to bool
            }
        }
    }
} 