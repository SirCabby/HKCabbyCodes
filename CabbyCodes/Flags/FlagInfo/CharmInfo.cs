namespace CabbyCodes.Flags.FlagInfo
{
    /// <summary>
    /// Represents a charm with all its properties.
    /// </summary>
    public readonly struct CharmInfo
    {
        public int Id { get; }
        public string Name => GotFlag.ReadableName;
        public FlagDef GotFlag { get; }
        public FlagDef CostFlag { get; }
        public FlagDef BrokenFlag { get; }
        public FlagDef UpgradeFlag { get; }
        public FlagDef GaveFlag { get; }
        public FlagDef PooedFlag { get; }
        public bool CanBeBroken { get; }
        public bool CanBeUpgraded { get; }

        public CharmInfo(int id, FlagDef gotFlag, FlagDef costFlag, 
                       FlagDef brokenFlag = null, FlagDef upgradeFlag = null,
                       FlagDef gaveFlag = null, FlagDef pooedFlag = null)
        {
            Id = id;
            GotFlag = gotFlag;
            CostFlag = costFlag;
            BrokenFlag = brokenFlag;
            UpgradeFlag = upgradeFlag;
            GaveFlag = gaveFlag;
            PooedFlag = pooedFlag;
            CanBeBroken = brokenFlag != null;
            CanBeUpgraded = upgradeFlag != null;
        }
    }
} 