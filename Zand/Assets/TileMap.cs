using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zand.Colliders;

namespace Zand.Assets
{
    public class TileMap : IRenderable
    {
        public int TileSize { get; private set; }
        public Point MapSize { get; private set; }
        private SpriteSheet[] _spriteSheets;
        private List<Tile[][]> Layers;
        private Random _rng;
        private bool _enabled;


        public Point Center => new Point(MapSize.X / 2 * TileSize, MapSize.Y / 2 * TileSize);

        public bool Enabled => _enabled;

        // logical grid

        public TileMap(int tileSize, Point mapSize, SpriteSheet mapSprites, SpriteSheet objectSprites)
        {
            TileSize = tileSize;
            MapSize = mapSize;
            _spriteSheets = new SpriteSheet[2];
            _spriteSheets[0] = mapSprites;
            _spriteSheets[1] = objectSprites;
            _rng = new Random();
            Layers = new List<Tile[][]>(2);
            Layers.Add(GenerateBaseLayer());
            Layers.Add(GenerateObjectLayer());
        }

        public Tile[][] GenerateBaseLayer()
        {
            // Instantiate
            var baseLayer = InitLayer();

            // Populate
            for (int y = 0; y < baseLayer.Length; y++)
            {
                for (int x = 0; x < baseLayer[y].Length; x++)
                {
                    // TODO use a reference to a "Tile Repo" instead of creating multiple types of tile
                    int id = _rng.Next(0, 14);
                    Tile newTile = new Tile(id, false);
                    baseLayer[y][x] = newTile;
                }
            }

            return baseLayer;
        }

        public Tile[][] GenerateObjectLayer()
        {
            var objectLayer = InitLayer();

            objectLayer[29][32] = new Tile(0, true);
            objectLayer[29][33] = new Tile(1, true);
            objectLayer[29][34] = new Tile(2, true);
            objectLayer[29][35] = new Tile(3, true);
            objectLayer[29][36] = new Tile(4, true);

            objectLayer[30][32] = new Tile(5, true);
            objectLayer[30][33] = new Tile(6, true);
            objectLayer[30][34] = new Tile(7, true);
            objectLayer[30][35] = new Tile(8, true);
            objectLayer[30][36] = new Tile(9, true);

            objectLayer[31][32] = new Tile(10, true);
            objectLayer[31][33] = new Tile(11, true);
            objectLayer[31][34] = new Tile(12, true);
            objectLayer[31][35] = new Tile(13, true);
            objectLayer[31][36] = new Tile(14, true);

            return objectLayer;
        }

        private Tile[][] InitLayer()
        {
            var layer = new Tile[MapSize.Y][];
            for (int y = 0; y < layer.Length; y++)
            {
                layer[y] = new Tile[MapSize.X];
            }

            return layer;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetTransformation(), samplerState: SamplerState.PointClamp); ;
            DrawMap(spriteBatch, camera);
            spriteBatch.End();
        }

        private void DrawMap(SpriteBatch sbatch, Camera camera)
        {
            (Point min, Point max) cullingBounds = GetCullingBounds(camera);

            for (int yIndex = cullingBounds.min.Y; yIndex < cullingBounds.max.Y; yIndex++)
            {
                for (int xIndex = cullingBounds.min.X; xIndex < cullingBounds.max.X; xIndex++)
                {
                    for (int layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
                    {
                        var tile = Layers[layerIndex][yIndex][xIndex];
                        if (tile.Id is null)
                        {
                            continue;
                        }

                        sbatch.Draw(
                            _spriteSheets[layerIndex].Texture,
                            new Vector2(xIndex * TileSize, yIndex * TileSize),
                            _spriteSheets[layerIndex].GetFrame(tile.Id.Value),
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

            if ((tileCoords.X < 0 || tileCoords.Y < 0) || (tileCoords.X >= MapSize.X || tileCoords.Y >= MapSize.Y))
            {
                return new Tile(true);
            }

            foreach (var layer in Layers)
            {
                Tile tile = layer[tileCoords.Y][tileCoords.X];
                if (tile.Static)
                {
                    return tile;
                }
            }

            return new Tile(true);
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
