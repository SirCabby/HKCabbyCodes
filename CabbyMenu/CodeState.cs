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
        private static readonly Dictionary<string, BoxedReference> boxes = new Dictionary<string, BoxedReference>();

        /// <summary>
        /// Gets or creates a boxed reference for the specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the reference.</param>
        /// <param name="initialValue">The initial value to store if the key doesn't exist.</param>
        /// <returns>A BoxedReference containing the value for the specified key.</returns>
        public static BoxedReference Get(string key, object initialValue)
        {
            if (!boxes.ContainsKey(key))
            {
                boxes.Add(key, new BoxedReference(initialValue));
            }

            return boxes[key];
        }
    }
}