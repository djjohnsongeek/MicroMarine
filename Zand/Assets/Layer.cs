using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zand.Assets
{
    public class Layer : RenderableComponent
    {
        public TiledMap Map;
        public string Name;
        public int Id;
        public int Width;
        public int Height;
        public int[] Data;
        public Tile[] Tiles;
        public int TileCount;

        public Layer(TiledMap map, XmlNode layerNode)
        {
            Map = map;
            var attributes = layerNode.Attributes;

            Name = attributes.GetNamedItem("name").Value;
            Id = int.Parse(attributes.GetNamedItem("id").Value);
            Width = int.Parse(attributes.GetNamedItem("width").Value);
            Height = int.Parse(attributes.GetNamedItem("height").Value);

            var layerData = layerNode.FirstChild.InnerText.Split(',');
            Data = LoadData(layerData);
            Tiles = LoadTiles(Data);
        }

        private int[] LoadData(string[] strData)
        {
            var data = new int[strData.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = int.Parse(strData[i]);
            }
            return data;
        }

        private Tile[] LoadTiles(int[] gIds)
        {
            var tiles = new List<Tile>();
            int index = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // Ignore 0, means empty tile
                    if (gIds[index] > 0)
                    {
                        var tile = new Tile
                        {
                            GId = gIds[index],
                            X = x * Map.TileWidth,
                            Y = y * Map.TileHeight,
                            TileSet = Map.GetGIdTileSet(gIds[index])
                        };
                        tiles.Add(tile);
                    }
                    index++;
                }
            }

            TileCount = tiles.Count;
            return tiles.ToArray();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < TileCount; i++)
            {
                var tile = Tiles[i];
                spriteBatch.Draw(
                    texture: tile.TileSet.Texture,
                    position: new Vector2(tile.X, tile.Y),
                    sourceRectangle: tile.TileSet.GetSrcRect(tile.GId),
                    color: Color.White,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: Vector2.One,
                    effects: SpriteEffects.None,
                    layerDepth: 0
                );
            }
        }
    }
}
