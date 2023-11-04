namespace CabbyCodes
{
    public class BoxedReference
    {
        public object Value
        {
            get; set;
        }

        public BoxedReference(object value = null)
        {
            Value = value;
        }
    }
}
