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
        
        protected static IPatch CreatePatch(FlagDef flag)
        {
            // Determine type from FlagDef's type
            switch (flag.Type)
            {
                case "PlayerData_Bool":
                    return new BoolPatch(flag);
                case "PlayerData_Int":
                    return new IntPatch(flag);
                case "PlayerData_Single":
                    return new FloatPatch(flag);
                case "PersistentBoolData":
                    return new BoolPatch(flag);
                case "PersistentIntData":
                    return new IntPatch(flag);
                case "GeoRockData":
                    return new BoolPatch(flag);
                default:
                    return new BoolPatch(flag); // Default to bool
            }
        }
    }
} 