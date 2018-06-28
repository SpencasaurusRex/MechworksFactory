using System;
using System.Collections.Generic;

namespace Tiles
{
    public class World
    {
        readonly Dictionary<ChunkCoords, Chunk> chunks = new Dictionary<ChunkCoords, Chunk>();

        public void SetTile(Int64 gx, Int64 gy, Tile tile)
        {
            int index = GetIndex(gx, gy);
            GetChunk(gx, gy).tiles[index] = tile;
        }

        public Tile GetTile(Int64 gx, Int64 gy)
        {
            int index = GetIndex(gx, gy);
            return GetChunk(gx, gy).tiles[index];
        }

        public void SetType(Int64 gx, Int64 gy, TileType type)
        {
            int index = GetIndex(gx, gy);
            var chunk = GetChunk(gx, gy);
            var tile = chunk.tiles[index];
            tile.type = type;
            chunk.tiles[index] = tile;
        }

        int GetIndex(Int64 gx, Int64 gy)
        {
            return (int)(((gy & Chunk.MASK_Y) << Chunk.WIDTH_BITS) | (gx & Chunk.MASK_X));
        }

        Chunk GetChunk(Int64 gx, Int64 gy)
        {
            var coords = new ChunkCoords(gx >> Chunk.WIDTH_BITS, gy >> Chunk.HEIGHT_BITS);
            if (!chunks.ContainsKey(coords))
            {
                var chunk = new Chunk(coords);
                chunks[coords] = chunk;
                return chunk;
            }
            return chunks[coords];
        }
    }
    
    struct ChunkCoords : IEquatable<ChunkCoords>
    {
        public readonly Int64 cx;
        public readonly Int64 cy;

        public ChunkCoords(Int64 cx, Int64 cy)
        {
            this.cx = cx;
            this.cy = cy;
        }

        public static ChunkCoords FromBlockCoords(Int64 gx, Int64 gy)
        {
            return new ChunkCoords(gx >> Chunk.WIDTH_BITS, gy >> Chunk.HEIGHT_BITS);
        }

        public override int GetHashCode()
        {
            return cx.GetHashCode() * 17 + cy.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ChunkCoords))
            {
                return false;
            }
            return Equals(obj);
        }

        public bool Equals(ChunkCoords other)
        {
            return this.cx == other.cx && this.cy == other.cy;
        }
    }

    struct Chunk
    {
        public const int WIDTH_BITS = 6; // 64x64 tile chunks
        public const int HEIGHT_BITS = 6;
        public const int WIDTH = 1 << WIDTH_BITS;
        public const int HEIGHT = 1 << HEIGHT_BITS;
        public const int MASK_X = WIDTH - 1;
        public const int MASK_Y = HEIGHT - 1;

        public readonly ChunkCoords coords;
        public readonly Tile[] tiles;

        public Chunk(ChunkCoords coords)
        {
            this.coords = coords;
            tiles = new Tile[WIDTH * HEIGHT];

            for (int i = 0; i < WIDTH * HEIGHT; i++)
            {
                tiles[i] = new Tile(BaseTileType.Grass, TileType.Air);
            }
        }
    }
}
