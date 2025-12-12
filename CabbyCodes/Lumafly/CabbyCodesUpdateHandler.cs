#if LUMAFLY
using UnityEngine;

namespace CabbyCodes
{
    /// <summary>
    /// MonoBehaviour to handle Update loop for the Lumafly mod.
    /// </summary>
    internal class CabbyCodesUpdateHandler : MonoBehaviour
    {
        private void Update()
        {
            CabbyCodesModLumafly.Instance?.OnUpdate();
        }
    }
}
#endif

