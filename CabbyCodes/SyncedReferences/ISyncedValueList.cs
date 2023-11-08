namespace CabbyCodes.SyncedReferences
{
    public interface ISyncedValueList<T, T2> : ISyncedReference<T>
    {
        public T2 GetValueList();
    }
}
