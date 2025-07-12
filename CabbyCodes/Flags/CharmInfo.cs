using CabbyCodes.Flags.FlagTypes;

namespace CabbyCodes.Flags
{
    /// <summary>
    /// Represents a charm with all its properties.
    /// </summary>
    public struct CharmInfo
    {
        public int Id { get; }
        public string Name { get; }
        public FlagData GotFlag { get; }
        public FlagData CostFlag { get; }
        public FlagData BrokenFlag { get; }
        public FlagData UpgradeFlag { get; }
        public bool CanBeBroken { get; }
        public bool CanBeUpgraded { get; }

        public CharmInfo(int id, string name, FlagData gotFlag, FlagData costFlag, 
                       FlagData brokenFlag = null, FlagData upgradeFlag = null)
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