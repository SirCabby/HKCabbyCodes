using CabbyMenu.SyncedReferences;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Generic implementation of ISyncedReference<float> for PlayerData float values.
    /// Eliminates code duplication across patch classes.
    /// </summary>
    public class PlayerDataSyncedFloat : ISyncedReference<float>
    {
        private readonly string fieldName;

        /// <summary>
        /// Initializes a new instance of the PlayerDataSyncedFloat class for GetFloat/SetFloat methods.
        /// </summary>
        /// <param name="floatName">The name of the float field in PlayerData.</param>
        public PlayerDataSyncedFloat(string floatName)
        {
            fieldName = floatName;
        }

        /// <summary>
        /// Gets the current float value from PlayerData.
        /// </summary>
        /// <returns>The current float value.</returns>
        public float Get()
        {
            return CabbyCodes.Flags.FlagManager.GetFloatFlag(fieldName, "Global");
        }

        /// <summary>
        /// Sets the float value in PlayerData.
        /// </summary>
        /// <param name="value">The float value to set.</param>
        public virtual void Set(float value)
        {
            CabbyCodes.Flags.FlagManager.SetFloatFlag(fieldName, "Global", value);
        }
    }
} 