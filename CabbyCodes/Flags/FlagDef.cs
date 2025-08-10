using CabbyCodes.Scenes;

namespace CabbyCodes.Flags
{
    public class FlagDef
    {
        private readonly string _readableName;

        public string Id { get; }
        public SceneMapData Scene { get; }
        public string SceneName => Scene?.SceneName ?? "Global";
        public bool SemiPersistent { get; }
        public string Type { get; }
        public string ReadableName => string.IsNullOrEmpty(_readableName) ? Id : _readableName;

        public FlagDef(string id, SceneMapData scene, bool semiPersistent, string type, string readableName = "")
        {
            Id = id;
            Scene = scene;
            SemiPersistent = semiPersistent;
            Type = type;
            _readableName = readableName;
        }
    }
} 