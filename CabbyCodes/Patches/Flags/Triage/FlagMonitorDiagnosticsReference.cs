using CabbyMenu.SyncedReferences;

namespace CabbyCodes.Patches.Flags.Triage
{
    /// <summary>
    /// Synced reference for flag monitor diagnostics toggle.
    /// </summary>
    public class FlagMonitorDiagnosticsReference : ISyncedReference<bool>
    {
        private static FlagMonitorDiagnosticsReference instance;
        public static FlagMonitorDiagnosticsReference Instance => instance ?? (instance = new FlagMonitorDiagnosticsReference());

        public bool Get()
        {
            return FlagMonitorDiagnostics.DiagnosticsEnabled;
        }

        public void Set(bool newValue)
        {
            FlagMonitorDiagnostics.DiagnosticsEnabled = newValue;
        }
    }
} 