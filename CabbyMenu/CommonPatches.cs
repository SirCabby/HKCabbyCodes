namespace CabbyMenu
{
    /// <summary>
    /// Common patch utility methods. 
    /// Note: With the migration to MonoMod hooks, the Harmony prefix skip patterns are no longer needed.
    /// This class is kept for potential future utility methods.
    /// </summary>
    public abstract class CommonPatches
    {
        // Harmony prefix methods removed - MonoMod hooks skip the original by not calling orig()
    }
}
