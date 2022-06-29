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
        private int[][] _visualMap;
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
            _visualMap = new int[_mapSize.Y][];
            for (int y = 0; y < _visualMap.Length; y++)
            {
                _visualMap[y] = new int[_mapSize.X];
            }

            // Populate
            var rand = new Random();
            int tileId;
            int staticTilesCount = 0;
            for (int y = 0; y < _visualMap.Length; y++)
            {
               for (int x = 0; x < _visualMap[y].Length; x++)
                {
                    tileId = rand.Next(0, 64);
                    if (tileId == 63 && staticTilesCount >= 5)
                    {
                        tileId = rand.Next(0, 63);
                    }
                    _visualMap[y][x] = tileId;

                    // Add static tile
                    if (tileId == 63)
                    {
                        //Entity staticTile = Entity.Scene.CreateEntity("staticTile", new Vector2(x * _tileSize, y * _tileSize));
                        //var collider = new BoxCollider(new Rectangle(new Point(x * _tileSize, y * _tileSize), new Point(_tileSize, _tileSize)), Vector2.Zero);
                        //collider.Static = true;
                        //staticTile.AddComponent(collider);
                        //Entity.Scene.RegisterCollider(collider);
                        //staticTilesCount++;
                    }
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
                    sbatch.Draw(_spriteSheet.Texture, new Vector2(xIndex * _tileSize, yIndex * _tileSize), _spriteSheet.GetFrame(_visualMap[yIndex][xIndex]), Color.White);
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
            // check current tile for all sides
                // calculate sides using circle collider edges
            // for any side that is colliding with a static tile
                // update entity's (and collider?) position
                    // collider's x position = tile's static edge + or - collider radius
                    // collider's y poistion = tile's static edge + or - collider radius

        }

        public bool IsOnStaticTile(Vector2 position)
        {
            return GetTile(position) == 63;
        }

        private int GetTile(Vector2 position)
        {
            int x = (int)position.X / _tileSize;
            int y = (int)position.Y / _tileSize;

            return _visualMap[y][x];
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
