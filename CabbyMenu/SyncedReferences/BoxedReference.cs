namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// A simple boxed reference implementation that stores and manages object values.
    /// </summary>
    public class BoxedReference : ISyncedReference<object>
    {
        /// <summary>
        /// The stored object value.
        /// </summary>
        private object value;

        /// <summary>
        /// Initializes a new instance of the BoxedReference class with an optional initial value.
        /// </summary>
        /// <param name="initialValue">The initial value to store. Defaults to null.</param>
        public BoxedReference(object initialValue = null)
        {
            value = initialValue;
        }

        /// <summary>
        /// Gets the current stored value.
        /// </summary>
        /// <returns>The current object value.</returns>
        public object Get()
        {
            return value;
        }

        /// <summary>
        /// Sets the stored value to the specified object.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void Set(object value)
        {
            this.value = value;
        }
    }
}