namespace CabbyCodes.SyncedReferences
{
    public class BoxedReference : ISyncedReference<object>
    {
        private object value;

        public BoxedReference(object initialValue = null)
        {
            value = initialValue;
        }

        public object Get()
        {
            return value;
        }

        public void Set(object value)
        {
            this.value = value;
        }
    }
}
