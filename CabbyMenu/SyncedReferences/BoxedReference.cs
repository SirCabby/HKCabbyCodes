namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// A simple boxed reference implementation that stores and manages values of type T.
    /// </summary>
    public class BoxedReference<T> : ISyncedReference<T>
    {
        /// <summary>
        /// The stored value.
        /// </summary>
        private T value;

        /// <summary>
        /// Initializes a new instance of the BoxedReference class with an optional initial value.
        /// </summary>
        /// <param name="initialValue">The initial value to store. Defaults to default(T).</param>
        public BoxedReference(T initialValue = default)
        {
            value = initialValue;
        }

        /// <summary>
        /// Gets the current stored value.
        /// </summary>
        /// <returns>The current value.</returns>
        public T Get()
        {
            return value;
        }

        /// <summary>
        /// Sets the stored value to the specified value.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void Set(T value)
        {
            this.value = value;
        }
    }
}