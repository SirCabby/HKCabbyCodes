namespace CabbyCodes.SyncedReferences
{
    public interface ISyncedReference<T>
    {
        public T Get();
        public void Set(T value);
    }
}
