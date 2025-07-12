namespace CabbyCodes.Flags
{
    /// <summary>
    /// Represents hunter target information with killed and kills flags.
    /// </summary>
    public readonly struct HunterInfo
    {
        private readonly string _readableName;

        public string EnemyName { get; }
        public string ReadableName => string.IsNullOrEmpty(_readableName) ? EnemyName : _readableName;
        public FlagData KilledFlag { get; }
        public FlagData KillsFlag { get; }

        public HunterInfo(string enemyName, FlagData killedFlag, FlagData killsFlag, string readableName = "")
        {
            EnemyName = enemyName;
            _readableName = readableName;
            KilledFlag = killedFlag;
            KillsFlag = killsFlag;
        }
    }
} 