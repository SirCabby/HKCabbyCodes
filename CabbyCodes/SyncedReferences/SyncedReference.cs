using System;

namespace CabbyCodes.SyncedReferences
{
    public class SyncedReference<T>
    {
        public Func<T> Get;
        public Action<T> Set;
    }
}
