using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zand.Colliders;
using Zand.ECS.Components;

namespace Zand.Assets
{
    public class TileMap : Component, IRenderable
    {
        public int TileSize { get; private set; }
        public Point MapSize { get; private set; }
        private SpriteSheet _spriteSheet;
        private Tile[][] _visualMap;

        public Point MapCenter => new Point(MapSize.X / 2 * TileSize, MapSize.Y / 2 * TileSize);
        // logical grid

        public TileMap(int tileSize, Point mapSize, SpriteSheet sprites)
        {
            TileSize = tileSize;
            MapSize = mapSize;
            _spriteSheet = sprites;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            GenerateMap();
        }

        public override void OnRemovedFromEntity()
        {
            _spriteSheet.OnRemovedFromEntity();
            foreach (var row in _visualMap)
            {
                Array.Clear(row, 0, row.Length);
            }
            Array.Clear(_visualMap, 0, _visualMap.Length);
            base.OnRemovedFromEntity();
        }

        public void GenerateMap()
        {
            // Instantiate
            _visualMap = new Tile[MapSize.Y][];
            for (int y = 0; y < _visualMap.Length; y++)
            {
                _visualMap[y] = new Tile[MapSize.X];
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
                        new Vector2(xIndex * TileSize, yIndex * TileSize),
                        _spriteSheet.GetFrame(_visualMap[yIndex][xIndex].Id),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        0
                    );
                }
            }
        }

        public (Point minIndex, Point maxIndex) GetCullingBounds(Camera camera)
        {
            int maxX = GetMaxBound(camera.Position.X, camera.Width, MapSize.X);
            int maxY = GetMaxBound(camera.Position.Y, camera.Height, MapSize.Y);
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
                collider.Entity.Position.X = tilePos.X + TileSize + collider.Radius - collider.Offset.X;
            }

            if (CollidesWithTile(collider.TopCenter))
            {
                Point tilePos = GetTilePosition(collider.TopCenter);
                collider.Entity.Position.Y = tilePos.Y + TileSize + collider.Radius - collider.Offset.Y;
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

            if (tileCoords.X < 0 || tileCoords.Y < 0)
            {
                return new Tile(true);
            }

            return _visualMap[tileCoords.Y][tileCoords.X];
        }

        public Point GetTileCoords(Vector2 position)
        {
            return new Point(
                (int)position.X / TileSize,
                (int)position.Y / TileSize
            );
        }

        private Point GetTilePosition(Vector2 position)
        {
            Point tileCoords = GetTileCoords(position);
            return new Point(tileCoords.X * TileSize, tileCoords.Y * TileSize);
        }

        private int GetMaxBound(float posCoordinate, int cameraDimension, int maxValue)
        {
            int initValue = (int)(posCoordinate + cameraDimension / 2) / TileSize + 1;
            return Math.Min(maxValue, initValue);
        }

        private int GetMinBound(float posCoordinate, int cameraDimension, int minValue)
        {
            int initValue = ((int)posCoordinate - cameraDimension / 2) / TileSize;
            return Math.Max(minValue, initValue);
        }
    }
}
