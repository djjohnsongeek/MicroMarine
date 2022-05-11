﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Zand.Assets
{
    public class TileMap
    {
        private int _tileSize;
        private Point _mapSize;
        private SpriteSheet _spriteSheet;
        private int[][] _visualMap;
        // logical grid

        public TileMap(int tileSize, Point mapSize, SpriteSheet sprites)
        {
            _tileSize = tileSize;
            _mapSize = mapSize;
            _spriteSheet = sprites;
        }

        public void GenerateMap()
        {
            // Instantiate
            _visualMap = new int[_mapSize.Y][];
            for (int y = 0; y < _visualMap.Length; y++)
            {
                _visualMap[y] = new int[_mapSize.X];
            }

            // Populate
            var rand = new Random();
            for (int y = 0; y < _visualMap.Length; y++)
            {
               for (int x = 0; x < _visualMap[y].Length; x++)
                {
                    _visualMap[y][x] = rand.Next(0, 55);
                }
            }
        }

        public void Draw(SpriteBatch sbatch)
        {
            (Point min, Point max) cullingBounds = GetCullingBounds();
            for (int yIndex = cullingBounds.min.Y; yIndex < cullingBounds.max.Y; yIndex++)
            {
                for (int xIndex = cullingBounds.min.X; xIndex < cullingBounds.max.X; xIndex++)
                {
                    sbatch.Draw(_spriteSheet.Texture, new Vector2(xIndex * _tileSize, yIndex * _tileSize), _spriteSheet.GetFrame(_visualMap[yIndex][xIndex]), Color.White);
                }
            }
        }

        public (Point minIndex, Point maxIndex) GetCullingBounds()
        {
            return (new Point(2, 0), new Point(60, 60));
        }
    }
}
