using System;

namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// A generic ISyncedReference implementation that uses delegates for get/set.
    /// </summary>
    public class DelegateReference<T> : ISyncedReference<T>
    {
        private readonly Func<T> getter;
        private readonly Action<T> setter;

        public DelegateReference(Func<T> getter, Action<T> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public T Get() => getter();
        public void Set(T value) => setter(value);
    }
} 