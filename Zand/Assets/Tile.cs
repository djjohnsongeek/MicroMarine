namespace Zand.Assets
{
    public struct Tile
    {
        public int Id;
        public bool Static;

        public Tile(int id)
        {
            Id = id;
            Static = Config.StaticTiles.Contains(id);
        }

        public Tile(bool isNull)
        {
            Id = 0;
            Static = false;
        }
    }
}
