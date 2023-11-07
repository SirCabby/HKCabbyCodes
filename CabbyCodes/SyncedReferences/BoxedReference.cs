namespace CabbyCodes.SyncedReferences
{
    public class BoxedReference : SyncedReference<object>
    {
        private object value;

        public BoxedReference(object initialValue = null)
        {
            value = initialValue;

            Get = () =>
            {
                return value;
            };

            Set = (obj) =>
            {
                value = obj;
            };
        }
    }
}
