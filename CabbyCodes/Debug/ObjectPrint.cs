using System;
using System.Reflection;

namespace CabbyCodes.Debug
{
    public class ObjectPrint
    {
        private const string tab = "    ";

        public static void DisplayObjectInfo(Object o)
        {
            // Include the type of the object
            Type type = o.GetType();
            CabbyCodesPlugin.BLogger.LogInfo("Type: " + type.Name);

            // Include information for each Field
            CabbyCodesPlugin.BLogger.LogInfo(tab + "Fields:");
            FieldInfo[] fields = type.GetFields();
            if (fields.Length > 0)
            {
                foreach (FieldInfo f in fields)
                {
                    CabbyCodesPlugin.BLogger.LogInfo(tab + tab + f.ToString() + " = " + f.GetValue(o));
                }
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo(tab + tab + "None");
            }

            // Include information for each Property
            CabbyCodesPlugin.BLogger.LogInfo(tab + "Properties:");
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length > 0)
            {
                foreach (PropertyInfo p in properties)
                {
                    CabbyCodesPlugin.BLogger.LogInfo(tab + tab + p.ToString() + " = " + p.GetValue(o, null));
                }
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo(tab + tab + "None");
            }
        }
    }
}
