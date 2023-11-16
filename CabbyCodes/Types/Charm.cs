namespace CabbyCodes.Types
{
    public struct Charm
    {
        public int id;
        public string name;
        public int defaultCost;

        public Charm(int index, string name)
        {
            this.id = index;
            this.name = name;
            defaultCost = PlayerData.instance.GetInt("charmCost_" + index);
        }
    }
}
