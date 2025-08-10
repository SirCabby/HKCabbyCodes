using System;
using System.Collections.Generic;

namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// An ISyncedValueList implementation that delegates value access and list generation.
    /// </summary>
    public class DelegateValueList : ISyncedValueList
    {
        private readonly Func<int> getter;
        private readonly Action<int> setter;
        private readonly Func<List<string>> listProvider;

        public DelegateValueList(Func<int> getter, Action<int> setter, Func<List<string>> listProvider)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
            this.listProvider = listProvider ?? throw new ArgumentNullException(nameof(listProvider));
        }

        public int Get() => getter();
        public void Set(int value) => setter(value);
        public List<string> GetValueList() => listProvider();
    }
} 