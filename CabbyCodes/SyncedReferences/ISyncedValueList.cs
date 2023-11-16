using System.Collections.Generic;

namespace CabbyCodes.SyncedReferences
{
    public interface ISyncedValueList<T> : ISyncedReference<T>
    {
        public List<string> GetValueList();
    }
}
