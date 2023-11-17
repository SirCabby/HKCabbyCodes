using System.Collections.Generic;

namespace CabbyCodes.SyncedReferences
{
    public interface ISyncedValueList : ISyncedReference<int>
    {
        public List<string> GetValueList();
    }
}
