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
                PlayerData.instance.SetBool(flagDef.Id, value);
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
                PlayerData.instance.SetBool(id, value);
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
                PlayerData.instance.SetInt(flagDef.Id, value);
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
                return PlayerData.instance.GetBool(flagDef.Id);
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
                return PlayerData.instance.GetBool(id);
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
                return PlayerData.instance.GetInt(flagDef.Id);
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
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(flagDef.Id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    if (list != null && !list.Contains(item))
                    {
                        list.Add(item);
                    }
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
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    if (list != null && !list.Contains(item))
                    {
                        list.Add(item);
                    }
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
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(flagDef.Id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    if (list != null && list.Contains(item))
                    {
                        list.Remove(item);
                    }
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
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    if (list != null && list.Contains(item))
                    {
                        list.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an item exists in a list flag. Automatically handles global list flags.
        /// </summary>
        /// <param name="flagDef">The flag definition containing the flag information.</param>
        /// <param name="item">The item to check for in the list.</param>
        /// <returns>True if the item exists in the list, false otherwise.</returns>
        public static bool ListFlagContains(FlagDef flagDef, string item)
        {
            if (flagDef == null || string.IsNullOrEmpty(item))
                return false;

            if (IsGlobalFlag(flagDef))
            {
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(flagDef.Id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    return list?.Contains(item) ?? false;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Checks if an item exists in a list flag using direct parameters. Automatically handles global list flags.
        /// </summary>
        /// <param name="id">The flag identifier.</param>
        /// <param name="sceneName">The scene name, or "Global" for global flags.</param>
        /// <param name="item">The item to check for in the list.</param>
        /// <returns>True if the item exists in the list, false otherwise.</returns>
        public static bool ListFlagContains(string id, string sceneName, string item)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(item))
                return false;

            if (sceneName == "Global")
            {
                // Get the list from PlayerData using reflection
                var listProperty = typeof(PlayerData).GetField(id);
                if (listProperty != null && listProperty.FieldType == typeof(List<string>))
                {
                    var list = (List<string>)listProperty.GetValue(PlayerData.instance);
                    return list?.Contains(item) ?? false;
                }
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
                PlayerData.instance.SetFloat(flagDef.Id, value);
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
                PlayerData.instance.SetFloat(id, value);
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
                return PlayerData.instance.GetFloat(flagDef.Id);
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
                return PlayerData.instance.GetFloat(id);
            }
            
            return 0f;
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
            PersistentBoolData pbd = new PersistentBoolData
            {
                id = id,
                sceneName = sceneName,
                activated = false,
                semiPersistent = semiPersistent
            };

            PersistentBoolData result = SceneData.instance.FindMyState(pbd);
            if (result == null)
            {
                SceneData.instance.SaveMyState(pbd);
                result = SceneData.instance.FindMyState(pbd);
            }

            return result;
        }
    }
}
