namespace Zand.Assets
{
    public struct Tile
    {
        public int X;
        public int Y;
        public int GId;
        public TileSet TileSet;

        public Tile(int gId, int x, int y, TileSet tSet)
        {
            GId = gId;
            X = x;
            Y = y;
            TileSet = tSet;
        }
    }
}
