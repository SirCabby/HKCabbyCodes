namespace CabbyCodes.Flags.FlagInfo
{
    /// <summary>
    /// Represents a charm with all its properties.
    /// </summary>
    public readonly struct CharmInfo
    {
        public int Id { get; }
        public string Name { get; }
        public FlagDef GotFlag { get; }
        public FlagDef CostFlag { get; }
        public FlagDef BrokenFlag { get; }
        public FlagDef UpgradeFlag { get; }
        public bool CanBeBroken { get; }
        public bool CanBeUpgraded { get; }

        public CharmInfo(int id, string name, FlagDef gotFlag, FlagDef costFlag, 
                       FlagDef brokenFlag = null, FlagDef upgradeFlag = null)
        {
            Id = id;
            Name = name;
            GotFlag = gotFlag;
            CostFlag = costFlag;
            BrokenFlag = brokenFlag;
            UpgradeFlag = upgradeFlag;
            CanBeBroken = brokenFlag != null;
            CanBeUpgraded = upgradeFlag != null;
        }
    }
} 