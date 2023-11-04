namespace CabbyCodes.Patches
{
    public abstract class BasePatch
    {
        public static bool Prefix_SkipOriginal()
        {
            return false;
        }

        public abstract void Patch();
        public abstract void UnPatch();
    }
}
