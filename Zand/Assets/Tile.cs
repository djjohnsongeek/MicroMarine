namespace Zand.Assets
{
    public struct Tile
    {
        public int? Id;
        public bool Static;

        public Tile(int id, bool isStatic)
        {
            Id = id;
            Static = isStatic;
        }

        public Tile(bool isNull)
        {
            Id = isNull ? null : 0;
            Static = false;
        }
    }
}
