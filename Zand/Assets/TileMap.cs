using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zand.ECS.Components;

namespace Zand.Assets
{
    public class TileMap : Component, IRenderable
    {
        private int _tileSize;
        private Point _mapSize;
        private SpriteSheet _spriteSheet;
        private Tile[][] _visualMap;
        // logical grid

        public TileMap(int tileSize, Point mapSize, SpriteSheet sprites)
        {
            _tileSize = tileSize;
            _mapSize = mapSize;
            _spriteSheet = sprites;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            GenerateMap();
        }

        public void GenerateMap()
        {
            // Instantiate
            _visualMap = new Tile[_mapSize.Y][];
            for (int y = 0; y < _visualMap.Length; y++)
            {
                _visualMap[y] = new Tile[_mapSize.X];
            }

            // Populate
            var rand = new Random();
            for (int y = 0; y < _visualMap.Length; y++)
            {
                for (int x = 0; x < _visualMap[y].Length; x++)
                {
                    // use a reference to a "Tile Repo" instead of creating multiple types of tile
                    Tile newTile = new Tile(rand.Next(0, 64));
                    _visualMap[y][x] = newTile;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(spriteBatch, Scene.Camera);
        }

        private void DrawMap(SpriteBatch sbatch, Camera camera)
        {
            (Point min, Point max) cullingBounds = GetCullingBounds(camera);
            for (int yIndex = cullingBounds.min.Y; yIndex < cullingBounds.max.Y; yIndex++)
            {
                for (int xIndex = cullingBounds.min.X; xIndex < cullingBounds.max.X; xIndex++)
                {
                    sbatch.Draw(
                        _spriteSheet.Texture,
                        new Vector2(xIndex * _tileSize, yIndex * _tileSize),
                        _spriteSheet.GetFrame(_visualMap[yIndex][xIndex].Id),
                        Color.White
                    );
                }
            }
        }

        public (Point minIndex, Point maxIndex) GetCullingBounds(Camera camera)
        {
            int maxX = GetMaxBound(camera.Position.X, camera.Width, _mapSize.X);
            int maxY = GetMaxBound(camera.Position.Y, camera.Height, _mapSize.Y);
            int minX = GetMinBound(camera.Position.X, camera.Width, 0);
            int minY = GetMinBound(camera.Position.Y, camera.Height, 0);

            return (new Point(minX, minY), new Point(maxX, maxY));
        }

        public void ResolveMapCollisions(CircleCollider collider)
        {
            if (CollidesWithTile(collider.RightCenter))
            {
                Point tilePos = GetTilePosition(collider.RightCenter);
                collider.Entity.Position.X = tilePos.X - collider.Radius - collider.Offset.X;
            }

            if (CollidesWithTile(collider.LeftCenter))
            {
                Point tilePos = GetTilePosition(collider.LeftCenter);
                collider.Entity.Position.X = tilePos.X + _tileSize + collider.Radius - collider.Offset.X;
            }

            if (CollidesWithTile(collider.TopCenter))
            {
                Point tilePos = GetTilePosition(collider.TopCenter);
                collider.Entity.Position.Y = tilePos.Y + _tileSize + collider.Radius - collider.Offset.Y;
            }

            if (CollidesWithTile(collider.BottomCenter))
            {
                Point tilePos = GetTilePosition(collider.BottomCenter);
                collider.Entity.Position.Y = tilePos.Y - collider.Radius - collider.Offset.Y;
            }
        }

        public bool CollidesWithTile(Vector2 position)
        {
            return GetTile(position).Static;
        }

        private Tile GetTile(Vector2 position)
        {
            Point tileCoords = GetTileCoords(position);
            return _visualMap[tileCoords.Y][tileCoords.X];
        }

        public Point GetTileCoords(Vector2 position)
        {
            return new Point(
                (int)position.X / _tileSize,
                (int)position.Y / _tileSize
            );
        }

        private Point GetTilePosition(Vector2 position)
        {
            Point tileCoords = GetTileCoords(position);
            return new Point(tileCoords.X * _tileSize, tileCoords.Y * _tileSize);
        }

        private int GetMaxBound(float posCoordinate, int cameraDimension, int maxValue)
        {
            int initValue = (int)(posCoordinate + cameraDimension / 2) / _tileSize + 1;
            return Math.Min(maxValue, initValue);
        }

        private int GetMinBound(float posCoordinate, int cameraDimension, int minValue)
        {
            int initValue = ((int)posCoordinate - cameraDimension / 2) / _tileSize;
            return Math.Max(minValue, initValue);
        }
    }
}
