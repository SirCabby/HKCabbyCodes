namespace CabbyCodes.Flags.FlagInfo
{
    /// <summary>
    /// Represents hunter target information with killed and kills flags.
    /// </summary>
    public readonly struct HunterInfo
    {
        private readonly string _readableName;

        public string EnemyName { get; }
        public string ReadableName => string.IsNullOrEmpty(_readableName) ? EnemyName : _readableName;
        public FlagDef KilledFlag { get; }
        public FlagDef KillsFlag { get; }

        public HunterInfo(string enemyName, FlagDef killedFlag, FlagDef killsFlag, string readableName = "")
        {
            EnemyName = enemyName;
            _readableName = readableName;
            KilledFlag = killedFlag;
            KillsFlag = killsFlag;
        }
    }
} 