namespace CabbyCodes.Flags
{
    public class FlagDef
    {
        private readonly string _readableName;

        public string Id { get; }
        public string SceneName { get; }
        public bool SemiPersistent { get; }
        public string Type { get; }
        public string ReadableName => string.IsNullOrEmpty(_readableName) ? Id : _readableName;

        public FlagDef(string id, string sceneName, bool semiPersistent, string type, string readableName = "")
        {
            Id = id;
            SceneName = sceneName;
            SemiPersistent = semiPersistent;
            Type = type;
            _readableName = readableName;
        }
    }
} 