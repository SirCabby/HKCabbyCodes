namespace CabbyCodes.Patches
{
    /// <summary>
    /// Abstract base class providing common patch methods for Harmony patches.
    /// </summary>
    public abstract class CommonPatches
    {
        /// <summary>
        /// Prefix method that skips the original method execution.
        /// </summary>
        /// <returns>False to skip the original method.</returns>
        public static bool Prefix_SkipOriginal()
        {
            return false;
        }

        /// <summary>
        /// Prefix method that allows the original method to execute.
        /// </summary>
        /// <returns>True to allow the original method to execute.</returns>
        public static bool Prefix_RunOriginal()
        {
            return true;
        }
    }
}
