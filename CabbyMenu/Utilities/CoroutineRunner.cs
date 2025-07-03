using UnityEngine;

namespace CabbyMenu.Utilities
{
    /// <summary>
    /// Provides a singleton MonoBehaviour for running coroutines from non-MonoBehaviour classes.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineRunner>();
                }
                return _instance;
            }
        }
    }
} 