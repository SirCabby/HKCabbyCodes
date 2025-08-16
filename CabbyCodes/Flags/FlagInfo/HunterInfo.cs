namespace CabbyCodes.Flags.FlagInfo
{
    /// <summary>
    /// Represents hunter target information with killed and kills flags.
    /// </summary>
    public readonly struct HunterInfo
    {
        public string ReadableName => KilledFlag.ReadableName;
        public FlagDef KilledFlag { get; }
        public FlagDef KillsFlag { get; }

        public HunterInfo(FlagDef killedFlag, FlagDef killsFlag)
        {
            KilledFlag = killedFlag;
            KillsFlag = killsFlag;
        }
    }
} 