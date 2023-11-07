namespace CabbyCodes.Patches
{
    public abstract class CommonPatches
    {
        public static bool Prefix_SkipOriginal()
        {
            return false;
        }
    }
}
