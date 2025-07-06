using CabbyMenu.SyncedReferences;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Wrapper class that converts between dropdown index (0-3) and slot number (1-4).
    /// </summary>
    public class SaveSlotDropdownWrapper : ISyncedReference<int>
    {
        private readonly ISyncedReference<int> originalReference;

        public SaveSlotDropdownWrapper(ISyncedReference<int> originalReference)
        {
            this.originalReference = originalReference;
        }

        public int Get()
        {
            // Convert slot number (1-4) to dropdown index (0-3)
            return originalReference.Get() - 1;
        }

        public void Set(int value)
        {
            // Convert dropdown index (0-3) to slot number (1-4)
            originalReference.Set(value + 1);
        }
    }
} 