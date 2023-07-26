using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zand.Assets
{
    public class Layer
    {
        public TiledMap Map;
        public string Name;
        public int Id;
        public int Width;
        public int Height;
        public int[] Data;
        public Tile[] Tiles;

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
                        tiles.Add(new Tile(gIds[index], x * Map.TileWidth, y * Map.TileHeight, Map.GetGIdTileSet(gIds[index])));
                    }
                    index++;
                }
            }

            return tiles.ToArray();
        }
    }
}
