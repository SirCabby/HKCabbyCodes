using System;
using System.Reflection;

namespace CabbyCodes.Debug
{
    /// <summary>
    /// Utility class for debugging and displaying object information through reflection.
    /// </summary>
    public class ObjectPrint
    {
        /// <summary>
        /// Tab character used for indentation in debug output.
        /// </summary>
        private const string tab = "    ";

        /// <summary>
        /// Displays comprehensive information about an object including all fields and properties.
        /// </summary>
        /// <param name="o">The object to display information about.</param>
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

        /// <summary>
        /// Prints information about the specified fields of an object.
        /// </summary>
        /// <param name="o">The object whose fields to print.</param>
        /// <param name="fields">The array of FieldInfo objects to process.</param>
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

        /// <summary>
        /// Prints information about the specified properties of an object.
        /// </summary>
        /// <param name="o">The object whose properties to print.</param>
        /// <param name="properties">The array of PropertyInfo objects to process.</param>
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
