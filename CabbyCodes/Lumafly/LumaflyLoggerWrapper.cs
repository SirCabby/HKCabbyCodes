#if LUMAFLY
using BepInEx.Logging;

namespace CabbyCodes
{
    /// <summary>
    /// Logger wrapper that mimics BepInEx's ManualLogSource using HKAPI's logging.
    /// </summary>
    public class LumaflyLoggerWrapper
    {
        private readonly string modName;

        public LumaflyLoggerWrapper(string name)
        {
            modName = name;
        }

        public void LogInfo(string message)
        {
            Modding.Logger.Log(string.Format("[{0}] {1}", modName, message));
        }

        public void LogWarning(string message)
        {
            Modding.Logger.LogWarn(string.Format("[{0}] {1}", modName, message));
        }

        public void LogError(string message)
        {
            Modding.Logger.LogError(string.Format("[{0}] {1}", modName, message));
        }

        public void LogDebug(string message)
        {
            Modding.Logger.LogDebug(string.Format("[{0}] {1}", modName, message));
        }

        public void Log(LogLevel level, object data)
        {
            switch (level)
            {
                case LogLevel.Error:
                case LogLevel.Fatal:
                    LogError(data?.ToString() ?? "");
                    break;
                case LogLevel.Warning:
                    LogWarning(data?.ToString() ?? "");
                    break;
                case LogLevel.Debug:
                    LogDebug(data?.ToString() ?? "");
                    break;
                default:
                    LogInfo(data?.ToString() ?? "");
                    break;
            }
        }
    }
}
#endif

