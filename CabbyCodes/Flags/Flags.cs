namespace CabbyCodes.Flags
{
    public static class Flags
    {
        /// <summary>
        /// Determines if a flag is a global flag based on its scene name.
        /// </summary>
        /// <param name="flagData">The flag data to check.</param>
        /// <returns>True if the flag is a global flag, false otherwise.</returns>
        private static bool IsGlobalFlag(FlagData flagData)
        {
            return flagData?.SceneName == "Global";
        }

        /// <summary>
        /// Sets a boolean flag to the specified value. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagData">The flag data containing the flag information.</param>
        /// <param name="value">The boolean value to set.</param>
        public static void SetBoolFlag(FlagData flagData, bool value)
        {
            if (flagData == null)
                return;

            if (IsGlobalFlag(flagData))
            {
                // Use PlayerData's built-in SetBool method
                PlayerData.instance.SetBool(flagData.Id, value);
            }
            else if (!string.IsNullOrEmpty(flagData.SceneName))
            {
                // Scene flag stored as PersistentBoolData
                PersistentBoolData pbd = PbdMaker.GetPbd(flagData.Id, flagData.SceneName);
                pbd.activated = value;
                SceneData.instance.SaveMyState(pbd);
            }
        }

        /// <summary>
        /// Sets an integer flag to the specified value. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagData">The flag data containing the flag information.</param>
        /// <param name="value">The integer value to set.</param>
        public static void SetIntFlag(FlagData flagData, int value)
        {
            if (flagData == null)
                return;

            if (IsGlobalFlag(flagData))
            {
                PlayerData.instance.SetInt(flagData.Id, value);
            }
            else if (!string.IsNullOrEmpty(flagData.SceneName))
            {
                PersistentIntData pid = new PersistentIntData
                {
                    id = flagData.Id,
                    sceneName = flagData.SceneName,
                    value = value,
                    semiPersistent = flagData.SemiPersistent
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
        /// <param name="flagData">The flag data containing the flag information.</param>
        /// <returns>The current boolean value of the flag, or false if not found.</returns>
        public static bool GetBoolFlag(FlagData flagData)
        {
            if (flagData == null)
                return false;

            if (IsGlobalFlag(flagData))
            {
                return PlayerData.instance.GetBool(flagData.Id);
            }
            else if (!string.IsNullOrEmpty(flagData.SceneName))
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(flagData.Id, flagData.SceneName);
                return pbd.activated;
            }
            
            return false;
        }

        /// <summary>
        /// Gets the current integer value of a flag. Automatically handles both global and scene flags.
        /// </summary>
        /// <param name="flagData">The flag data containing the flag information.</param>
        /// <returns>The current integer value of the flag, or -1 if not found.</returns>
        public static int GetIntFlag(FlagData flagData)
        {
            if (flagData == null)
                return -1;

            if (IsGlobalFlag(flagData))
            {
                return PlayerData.instance.GetInt(flagData.Id);
            }
            else if (!string.IsNullOrEmpty(flagData.SceneName))
            {
                PersistentIntData pid = new PersistentIntData
                {
                    id = flagData.Id,
                    sceneName = flagData.SceneName,
                    value = -1,
                    semiPersistent = flagData.SemiPersistent
                };

                PersistentIntData result = SceneData.instance.FindMyState(pid);
                return result?.value ?? -1;
            }
            
            return -1;
        }
    }
}
