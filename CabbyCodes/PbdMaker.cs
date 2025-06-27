namespace CabbyCodes
{
    /// <summary>
    /// Utility class for creating and managing PersistentBoolData objects for scene state management.
    /// </summary>
    public class PbdMaker
    {
        /// <summary>
        /// Creates or retrieves a PersistentBoolData object for the specified scene and ID.
        /// </summary>
        /// <param name="id">The unique identifier for the persistent data.</param>
        /// <param name="sceneName">The name of the scene this data belongs to.</param>
        /// <param name="semiPersistent">Whether the data should be semi-persistent (survives scene reloads).</param>
        /// <returns>A PersistentBoolData object that can be used to track state.</returns>
        public static PersistentBoolData GetPbd(string id, string sceneName, bool semiPersistent = false)
        {
            PersistentBoolData pbd = new()
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