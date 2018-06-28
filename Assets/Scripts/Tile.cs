namespace Tiles
{
    public enum TileType
    {
        Air,
        Robot,
    }

    public enum BaseTileType
    {
        Dirt,
        Grass,
        Stone,
        Water,
        Sand,
        Concrete,
    }

    public struct Tile
    {
        public BaseTileType baseType;
        public TileType type;

        public Tile(BaseTileType baseType, TileType type)
        {
            this.baseType = baseType;
            this.type = type;
        }
    }
}
