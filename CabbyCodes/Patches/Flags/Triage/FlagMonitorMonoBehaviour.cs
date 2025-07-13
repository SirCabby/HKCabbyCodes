using UnityEngine;

namespace CabbyCodes.Patches.Flags.Triage
{
    /// <summary>
    /// Helper MonoBehaviour for coroutines in the Flag Monitor system.
    /// This class exists solely to provide MonoBehaviour functionality for coroutines.
    /// </summary>
    public class FlagMonitorMonoBehaviour : MonoBehaviour
    {
        private static FlagMonitorMonoBehaviour instance;
        public static FlagMonitorMonoBehaviour Instance => instance;

        public static void EnsureInstance()
        {
            if (instance != null) return;
            var go = GameObject.Find("FlagMonitorMonoBehaviour") ?? new GameObject("FlagMonitorMonoBehaviour");
            DontDestroyOnLoad(go);
            instance = go.GetComponent<FlagMonitorMonoBehaviour>() ?? go.AddComponent<FlagMonitorMonoBehaviour>();
        }
    }
} 