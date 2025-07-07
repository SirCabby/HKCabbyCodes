using System.Collections.Generic;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu
{
    /// <summary>
    /// Manages shared state and references across the mod using a dictionary-based storage system.
    /// </summary>
    public class CodeState
    {
        /// <summary>
        /// Dictionary storing boxed references with string keys.
        /// </summary>
        private static readonly Dictionary<string, object> boxes = new Dictionary<string, object>();

        /// <summary>
        /// Gets or creates a boxed reference for the specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the reference.</param>
        /// <param name="initialValue">The initial value to store if the key doesn't exist.</param>
        /// <returns>A BoxedReference<T> containing the value for the specified key.</returns>
        public static BoxedReference<T> Get<T>(string key, T initialValue)
        {
            if (!boxes.ContainsKey(key))
            {
                boxes.Add(key, new BoxedReference<T>(initialValue));
            }
            return (BoxedReference<T>)boxes[key];
        }
    }
}