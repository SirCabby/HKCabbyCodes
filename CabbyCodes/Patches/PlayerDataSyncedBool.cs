using CabbyMenu.SyncedReferences;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Generic implementation of ISyncedReference<bool> for PlayerData boolean values.
    /// Eliminates code duplication across patch classes.
    /// </summary>
    public class PlayerDataSyncedBool : ISyncedReference<bool>
    {
        private readonly string fieldName;

        /// <summary>
        /// Initializes a new instance of the PlayerDataSyncedReference class for GetBool/SetBool methods.
        /// </summary>
        /// <param name="boolName">The name of the boolean field in PlayerData.</param>
        public PlayerDataSyncedBool(string boolName)
        {
            fieldName = boolName;
        }

        /// <summary>
        /// Gets the current boolean value from PlayerData.
        /// </summary>
        /// <returns>The current boolean value.</returns>
        public bool Get()
        {
            return CabbyCodes.Flags.FlagManager.GetBoolFlag(fieldName, "Global");
        }

        /// <summary>
        /// Sets the boolean value in PlayerData.
        /// </summary>
        /// <param name="value">The boolean value to set.</param>
        public virtual void Set(bool value)
        {
            CabbyCodes.Flags.FlagManager.SetBoolFlag(fieldName, "Global", value);
        }
    }
} 