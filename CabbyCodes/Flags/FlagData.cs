namespace CabbyCodes.Flags
{
    public class FlagData
    {
        public string Id { get; }
        public string SceneName { get; }
        public bool SemiPersistent { get; }
        public string Type { get; }

        public FlagData(string id, string sceneName, bool semiPersistent, string type)
        {
            Id = id;
            SceneName = sceneName;
            SemiPersistent = semiPersistent;
            Type = type;
        }
    }
} 