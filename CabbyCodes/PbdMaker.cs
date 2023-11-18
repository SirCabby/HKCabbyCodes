namespace CabbyCodes
{
    public class PbdMaker
    {
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
