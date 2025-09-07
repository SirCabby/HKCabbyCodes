using System.Collections.Generic;

namespace CabbyCodes.Flags
{
    public static class FlagManager
    {
        /// <summary>
        /// Determines if a flag is a global flag based on its scene name.
        /// </summary>
        /// <param name="flagDef">The flag definition to check.</param>
        /// <returns>True if the flag is a global flag, false otherwise.</returns>
        private static bool IsGlobalFlag(FlagDef flagDef)
        {
            return flagDef?.SceneName == "Global";
        }

        /// <summary>
        /// Sets a boolean flag to the specified value. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="value">The boolean value to set.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        public static void SetBoolFlag(FlagDef flagDef, bool value, bool semiPersistent = false)
        {
            if (flagDef == null)
                return;

            if (IsGlobalFlag(flagDef))
            {
                // Use PlayerData's built-in SetBool method
                try
                {
                    PlayerData.instance.SetBool(flagDef.Id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            else if (!string.IsNullOrEmpty(flagDef.SceneName))
            {
                // Scene flag stored as PersistentBoolData
                PersistentBoolData pbd = GetPbd(flagDef.Id, flagDef.SceneName, semiPersistent);
                pbd.activated = value;
                SceneData.instance.SaveMyState(pbd);
            }
        }

        /// <summary>
        /// Sets a boolean flag to the specified value using direct parameters. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="value">The boolean value to set.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        public static void SetBoolFlag(string id, string sceneName, bool value, bool semiPersistent = false)
        {
            if (string.IsNullOrEmpty(id))
                return;

            if (sceneName == "Global")
            {
                // Use PlayerData's built-in SetBool method
                try
                {
                    PlayerData.instance.SetBool(id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            else if (!string.IsNullOrEmpty(sceneName))
            {
                // Scene flag stored as PersistentBoolData
                PersistentBoolData pbd = GetPbd(id, sceneName, semiPersistent);
                pbd.activated = value;
                SceneData.instance.SaveMyState(pbd);
            }
        }

        /// <summary>
        /// Sets an integer flag to the specified value. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="value">The integer value to set.</param>
        public static void SetIntFlag(FlagDef flagDef, int value)
        {
            if (flagDef == null)
                return;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    PlayerData.instance.SetInt(flagDef.Id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            else if (!string.IsNullOrEmpty(flagDef.SceneName))
            {
                PersistentIntData pid = new PersistentIntData
                {
                    id = flagDef.Id,
                    sceneName = flagDef.SceneName,
                    value = value,
                    semiPersistent = flagDef.SemiPersistent
                };

                PersistentIntData result = SceneData.instance.FindMyState(pid);
                if (result == null)
                {
                    SceneData.instance.SaveMyState(pid);
                }
                else
                {
                    result.value = value;
                    SceneData.instance.SaveMyState(result);
                }
            }
        }

        /// <summary>
        /// Gets the current boolean value of a flag. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        /// <returns>The current boolean value of the flag, or false if not found.</returns>
        public static bool GetBoolFlag(FlagDef flagDef, bool semiPersistent = false)
        {
            if (flagDef == null)
                return false;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    return PlayerData.instance.GetBool(flagDef.Id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return false
                    return false;
                }
            }
            else if (!string.IsNullOrEmpty(flagDef.SceneName))
            {
                PersistentBoolData pbd = GetPbd(flagDef.Id, flagDef.SceneName, semiPersistent);
                return pbd.activated;
            }
            
            return false;
        }

        /// <summary>
        /// Gets the current boolean value of a flag using direct parameters. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        /// <returns>The current boolean value of the flag, or false if not found.</returns>
        public static bool GetBoolFlag(string id, string sceneName, bool semiPersistent = false)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            if (sceneName == "Global")
            {
                try
                {
                    return PlayerData.instance.GetBool(id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return false
                    return false;
                }
            }
            else if (!string.IsNullOrEmpty(sceneName))
            {
                PersistentBoolData pbd = GetPbd(id, sceneName, semiPersistent);
                return pbd.activated;
            }
            
            return false;
        }

        /// <summary>
        /// Gets the current integer value of a flag. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <returns>The current integer value of the flag, or -1 if not found.</returns>
        public static int GetIntFlag(FlagDef flagDef)
        {
            if (flagDef == null)
                return -1;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    return PlayerData.instance.GetInt(flagDef.Id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return -1
                    return -1;
                }
            }
            else if (!string.IsNullOrEmpty(flagDef.SceneName))
            {
                PersistentIntData pid = new PersistentIntData
                {
                    id = flagDef.Id,
                    sceneName = flagDef.SceneName,
                    value = -1,
                    semiPersistent = flagDef.SemiPersistent
                };

                PersistentIntData result = SceneData.instance.FindMyState(pid);
                return result?.value ?? -1;
            }
            
            return -1;
        }

        /// <summary>
        /// Adds an item to a list flag. Automatically handles global list flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="item">The item to add to the list.</param>
        public static void AddToListFlag(FlagDef flagDef, string item)
        {
            if (flagDef == null || string.IsNullOrEmpty(item))
                return;

            if (IsGlobalFlag(flagDef))
            {
                var list = GetPlayerDataListField(flagDef.Id);
                if (list != null && !list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds an item to a list flag using direct parameters. Automatically handles global list flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="item">The item to add to the list.</param>
        public static void AddToListFlag(string id, string sceneName, string item)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(item))
                return;

            if (sceneName == "Global")
            {
                var list = GetPlayerDataListField(id);
                if (list != null && !list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes an item from a list flag. Automatically handles global list flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="item">The item to remove from the list.</param>
        public static void RemoveFromListFlag(FlagDef flagDef, string item)
        {
            if (flagDef == null || string.IsNullOrEmpty(item))
                return;

            if (IsGlobalFlag(flagDef))
            {
                var list = GetPlayerDataListField(flagDef.Id);
                if (list != null && list.Contains(item))
                {
                    list.Remove(item);
                }
            }
        }

        /// <summary>
        /// Removes an item from a list flag using direct parameters. Automatically handles global list flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="item">The item to remove from the list.</param>
        public static void RemoveFromListFlag(string id, string sceneName, string item)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(item))
                return;

            if (sceneName == "Global")
            {
                var list = GetPlayerDataListField(id);
                if (list != null && list.Contains(item))
                {
                    list.Remove(item);
                }
            }
        }

        /// <summary>
        /// Checks if an item exists in a list flag. Automatically handles global list flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="item">The item to check for.</param>
        /// <returns>True if the item exists in the list, false otherwise.</returns>
        public static bool ContainsInListFlag(FlagDef flagDef, string item)
        {
            if (flagDef == null || string.IsNullOrEmpty(item))
                return false;

            if (IsGlobalFlag(flagDef))
            {
                var list = GetPlayerDataListField(flagDef.Id);
                return list != null && list.Contains(item);
            }
            
            return false;
        }

        /// <summary>
        /// Checks if an item exists in a list flag using direct parameters. Automatically handles global list flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="item">The item to check for.</param>
        /// <returns>True if the item exists in the list, false otherwise.</returns>
        public static bool ContainsInListFlag(string id, string sceneName, string item)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(item))
                return false;

            if (sceneName == "Global")
            {
                var list = GetPlayerDataListField(id);
                return list != null && list.Contains(item);
            }
            
            return false;
        }

        /// <summary>
        /// Sets a float flag to the specified value. Automatically handles global float flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="value">The float value to set.</param>
        public static void SetFloatFlag(FlagDef flagDef, float value)
        {
            if (flagDef == null)
                return;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    PlayerData.instance.SetFloat(flagDef.Id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            // Note: Unity doesn't have PersistentFloatData, so scene-specific float flags are not supported
        }

        /// <summary>
        /// Sets a float flag to the specified value using direct parameters. Automatically handles global float flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="value">The float value to set.</param>
        public static void SetFloatFlag(string id, string sceneName, float value)
        {
            if (string.IsNullOrEmpty(id))
                return;

            if (sceneName == "Global")
            {
                try
                {
                    PlayerData.instance.SetFloat(id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            // Note: Unity doesn't have PersistentFloatData, so scene-specific float flags are not supported
        }

        /// <summary>
        /// Gets the current float value of a flag. Automatically handles global float flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <returns>The current float value of the flag, or 0f if not found.</returns>
        public static float GetFloatFlag(FlagDef flagDef)
        {
            if (flagDef == null)
                return 0f;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    return PlayerData.instance.GetFloat(flagDef.Id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return 0f
                    return 0f;
                }
            }
            
            return 0f;
        }

        /// <summary>
        /// Gets the current float value of a flag using direct parameters. Automatically handles global float flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <returns>The current float value of the flag, or 0f if not found.</returns>
        public static float GetFloatFlag(string id, string sceneName)
        {
            if (string.IsNullOrEmpty(id))
                return 0f;

            if (sceneName == "Global")
            {
                try
                {
                    return PlayerData.instance.GetFloat(id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return 0f
                    return 0f;
                }
            }
            
            return 0f;
        }

        /// <summary>
        /// Sets a string flag to the specified value. Automatically handles global string flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="value">The string value to set.</param>
        public static void SetStringFlag(FlagDef flagDef, string value)
        {
            if (flagDef == null)
                return;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    PlayerData.instance.SetString(flagDef.Id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            // Note: Unity doesn't have PersistentStringData, so scene-specific string flags are not supported
        }

        /// <summary>
        /// Sets a string flag to the specified value using direct parameters. Automatically handles global string flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="value">The string value to set.</param>
        public static void SetStringFlag(string id, string sceneName, string value)
        {
            if (string.IsNullOrEmpty(id))
                return;

            if (sceneName == "Global")
            {
                try
                {
                    PlayerData.instance.SetString(id, value);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, ignore the operation
                }
            }
            // Note: Unity doesn't have PersistentStringData, so scene-specific string flags are not supported
        }

        /// <summary>
        /// Gets the current string value of a flag. Automatically handles global string flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <returns>The current string value of the flag, or empty string if not found.</returns>
        public static string GetStringFlag(FlagDef flagDef)
        {
            if (flagDef == null)
                return string.Empty;

            if (IsGlobalFlag(flagDef))
            {
                try
                {
                    return PlayerData.instance.GetString(flagDef.Id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return empty string
                    return string.Empty;
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Gets the current string value of a flag using direct parameters. Automatically handles global string flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <returns>The current string value of the flag, or empty string if not found.</returns>
        public static string GetStringFlag(string id, string sceneName)
        {
            if (string.IsNullOrEmpty(id))
                return string.Empty;

            if (sceneName == "Global")
            {
                try
                {
                    return PlayerData.instance.GetString(id);
                }
                catch (System.Exception)
                {
                    // If the flag doesn't exist in PlayerData, return empty string
                    return string.Empty;
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Creates or retrieves a PersistentBoolData object for the specified scene and ID.
        /// This method provides the same functionality as PbdMaker.GetPbd for internal use.
        /// </summary>
        /// <param name="id">The unique identifier for the persistent data.</param>
        /// <param name="sceneName">The name of the scene this data belongs to.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        /// <returns>A PersistentBoolData object that can be used to track state.</returns>
        private static PersistentBoolData GetPbd(string id, string sceneName, bool semiPersistent = false)
        {
            // First, build a template object to search for existing state
            var template = new PersistentBoolData
            {
                id = id,
                sceneName = sceneName,
                semiPersistent = semiPersistent,
                activated = false // default value; will be overwritten if existing state is found
            };

            // Attempt to retrieve an existing state for this flag from SceneData
            var existing = SceneData.instance.FindMyState(template);

            if (existing != null)
            {
                // Return the existing state so we reflect the actual stored value
                return existing;
            }

            // If no existing state was found, persist and return the new template
            SceneData.instance.SaveMyState(template);
            return template;
        }

        /// <summary>
        /// Helper method to get a field value from PlayerData using reflection
        /// </summary>
        /// <param name="fieldName">The name of the field to get</param>
        /// <returns>The field value, or null if the field doesn't exist</returns>
        private static object GetPlayerDataField(string fieldName)
        {
            if (PlayerData.instance == null) return null;
            
            var field = typeof(PlayerData).GetField(fieldName);
            if (field != null)
            {
                return field.GetValue(PlayerData.instance);
            }
            return null;
        }

        /// <summary>
        /// Helper method to set a field value in PlayerData using reflection
        /// </summary>
        /// <param name="fieldName">The name of the field to set</param>
        /// <param name="value">The value to set</param>
        /// <returns>True if the field was set successfully, false otherwise</returns>
        private static bool SetPlayerDataField(string fieldName, object value)
        {
            if (PlayerData.instance == null) return false;
            
            var field = typeof(PlayerData).GetField(fieldName);
            if (field != null)
            {
                field.SetValue(PlayerData.instance, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper method to get a List<string> field from PlayerData using reflection
        /// </summary>
        /// <param name="fieldName">The name of the field to get</param>
        /// <returns>The list, or null if the field doesn't exist or isn't a List<string></returns>
        private static List<string> GetPlayerDataListField(string fieldName)
        {
            if (PlayerData.instance == null) return null;
            
            var field = typeof(PlayerData).GetField(fieldName);
            if (field != null && field.FieldType == typeof(List<string>))
            {
                return (List<string>)field.GetValue(PlayerData.instance);
            }
            return null;
        }

        /// <summary>
        /// Gets the current BossStatue value of a Completion flag. Automatically handles global Completion flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <returns>The current BossStatue value of the flag, or null if not found.</returns>
        public static object GetCompletionFlag(FlagDef flagDef)
        {
            if (flagDef == null)
                return null;

            if (IsGlobalFlag(flagDef))
            {
                return GetPlayerDataField(flagDef.Id);
            }
            
            // For non-global flags, use PBD system (though most boss statues are global)
            var pbd = GetPbd(flagDef.Id, flagDef.SceneName);
            return pbd?.activated;
        }

        /// <summary>
        /// Gets the current BossStatue value of a Completion flag using direct parameters. Automatically handles global Completion flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <returns>The current BossStatue value of the flag, or null if not found.</returns>
        public static object GetCompletionFlag(string id, string sceneName)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            if (sceneName == "Global")
            {
                return GetPlayerDataField(id);
            }
            
            // For non-global flags, use PBD system
            var pbd = GetPbd(id, sceneName);
            return pbd?.activated;
        }

        /// <summary>
        /// Sets the value of a Completion flag. Automatically handles global Completion flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="value">The new value to set.</param>
        public static void SetCompletionFlag(FlagDef flagDef, object value)
        {
            if (flagDef == null)
                return;

            if (IsGlobalFlag(flagDef))
            {
                SetPlayerDataField(flagDef.Id, value);
            }
            else
            {
                // For non-global flags, use PBD system
                var pbd = GetPbd(flagDef.Id, flagDef.SceneName);
                if (pbd != null)
                {
                    pbd.activated = (bool)value;
                }
            }
        }

        /// <summary>
        /// Sets the value of a Completion flag using direct parameters. Automatically handles global Completion flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="value">The new value to set.</param>
        public static void SetCompletionFlag(string id, string sceneName, object value)
        {
            if (string.IsNullOrEmpty(id))
                return;

            if (sceneName == "Global")
            {
                SetPlayerDataField(id, value);
            }
            else
            {
                // For non-global flags, use PBD system
                var pbd = GetPbd(id, sceneName);
                if (pbd != null)
                {
                    pbd.activated = (bool)value;
                }
            }
        }
    }
}
