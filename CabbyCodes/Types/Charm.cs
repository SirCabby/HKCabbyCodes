namespace CabbyCodes.Types
{
    /// <summary>
    /// Represents a charm in the game with its properties and cost information.
    /// </summary>
    public struct Charm
    {
        /// <summary>
        /// The unique identifier for the charm.
        /// </summary>
        public int id;

        /// <summary>
        /// The display name of the charm.
        /// </summary>
        public string name;

        /// <summary>
        /// The default cost of the charm in notches.
        /// </summary>
        public int defaultCost;

        /// <summary>
        /// Initializes a new instance of the Charm struct.
        /// </summary>
        /// <param name="index">The charm index/ID.</param>
        /// <param name="name">The display name of the charm.</param>
        public Charm(int index, string name)
        {
            id = index;
            this.name = name;
            defaultCost = PlayerData.instance.GetInt("charmCost_" + index);
        }
    }
}
