using System.Collections.Generic;

namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// Interface for synchronizing integer values with a list of string representations.
    /// </summary>
    public interface ISyncedValueList : ISyncedReference<int>
    {
        /// <summary>
        /// Gets the list of string values that correspond to the integer values.
        /// </summary>
        /// <returns>A list of string representations for the available values.</returns>
        List<string> GetValueList();
    }
}