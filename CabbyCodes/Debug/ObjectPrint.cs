using System;
using System.Reflection;

namespace CabbyCodes.Debug
{
    public class ObjectPrint
    {
        private const string tab = "    ";

        public static void DisplayObjectInfo(object o)
        {
            Type type = o.GetType();
            CabbyCodesPlugin.BLogger.LogInfo("Type: " + type.Name);

            // Print each Field
            CabbyCodesPlugin.BLogger.LogInfo(tab + "Public Fields:");
            PrintFields(o, type.GetFields());

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Public Static Fields:");
            PrintFields(o, type.GetFields(BindingFlags.Public | BindingFlags.Static));

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Private Fields:");
            PrintFields(o, type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Private Static Fields:");
            PrintFields(o, type.GetFields(BindingFlags.NonPublic | BindingFlags.Static));

            // Print each Property
            CabbyCodesPlugin.BLogger.LogInfo(tab + "Public Properties:");
            PrintProperties(o, type.GetProperties());

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Public Static Properties:");
            PrintProperties(o, type.GetProperties(BindingFlags.Public | BindingFlags.Static));

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Private Properties:");
            PrintProperties(o, type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance));

            CabbyCodesPlugin.BLogger.LogInfo(tab + "Private Static Properties:");
            PrintProperties(o, type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static));
        }

        private static void PrintFields(object o, FieldInfo[] fields)
        {
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
        }

        private static void PrintProperties(object o, PropertyInfo[] properties)
        {
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
