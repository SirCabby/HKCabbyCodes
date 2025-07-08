namespace CabbyCodes.Patches.Flags
{
    public class FlagData
    {
        public string Id { get; }
        public string SceneName { get; }
        public string Value { get; }
        public bool SemiPersistent { get; }
        public string Type { get; }

        public FlagData(string id, string sceneName, string value, bool semiPersistent, string type)
        {
            Id = id;
            SceneName = sceneName;
            Value = value;
            SemiPersistent = semiPersistent;
            Type = type;
        }
    }
} 