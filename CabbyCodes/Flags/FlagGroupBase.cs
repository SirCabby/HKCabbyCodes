using System.Collections.Generic;

namespace CabbyCodes.Flags
{
    public abstract class FlagGroupBase
    {
        public List<FlagData> AllFlags => GetAllFlags(GetType());

        protected static List<FlagData> GetAllFlags(System.Type nestedType)
        {
            var list = new List<FlagData>();
            var properties = nestedType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(FlagData))
                {
                    var flagData = (FlagData)property.GetValue(null);
                    if (flagData != null)
                        list.Add(flagData);
                }
            }
            return list;
        }

        // Helper method that automatically determines the calling class type
        protected static List<FlagData> GetAllFlagsFromCaller()
        {
            // Get the calling type by looking at the stack frame
            var stackFrame = new System.Diagnostics.StackFrame(1);
            var callingType = stackFrame.GetMethod().DeclaringType;
            return GetAllFlags(callingType);
        }
    }
}