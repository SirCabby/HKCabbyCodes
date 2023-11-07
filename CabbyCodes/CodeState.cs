using System.Collections.Generic;
using CabbyCodes.SyncedReferences;

namespace CabbyCodes
{
    public class CodeState
    {
        private static readonly Dictionary<string, BoxedReference> boxes = new();

        public CodeState Add(string key, BoxedReference boxedReference)
        {
            boxes.Add(key, boxedReference);
            return this;
        }

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
