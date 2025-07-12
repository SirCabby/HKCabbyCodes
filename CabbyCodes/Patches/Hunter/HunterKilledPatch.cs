namespace CabbyCodes.Patches.Hunter
{
    public class HunterKilledPatch : PlayerDataSyncedBool
    {
        public HunterKilledPatch(string targetName) : base("killed" + targetName)
        {
        }
    }
}
