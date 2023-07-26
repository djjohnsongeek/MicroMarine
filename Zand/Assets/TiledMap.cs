using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace Zand.Assets
{
    public class TiledMap
    {
        public int Width;
        public int Height;
        public int TileWidth;
        public int TileHeight;
        public int NextLayerId;
        public int NextObjectId;

        public List<TileSet> TileSets;
        public List<Layer> Layers;
        public List<ObjectGroup> ObjectGroups;


        public TiledMap(string filename)
        {
            var doc = new XmlDocument();
            doc.Load(filename);

            var mapNode = doc.DocumentElement;
            var attributes = mapNode.Attributes;

            Width = int.Parse(attributes.GetNamedItem("width").Value);
            Height = int.Parse(attributes.GetNamedItem("height").Value);
            TileWidth = int.Parse(attributes.GetNamedItem("tilewidth").Value);
            TileHeight = int.Parse(attributes.GetNamedItem("tileheight").Value);
            NextLayerId = int.Parse(attributes.GetNamedItem("nextlayerid").Value);
            NextObjectId = int.Parse(attributes.GetNamedItem("nextobjectid").Value);



            var tileSetNodes = doc.GetElementsByTagName("tileset");

            // Parse Tilesets
            TileSets = new List<TileSet>();
            foreach (XmlNode node in tileSetNodes)
            {
                attributes = node.Attributes;
                int firstgId = int.Parse(attributes.GetNamedItem("firstgid").Value);
                string source = attributes.GetNamedItem("source").Value;

                TileSets.Add(new TileSet(firstgId, source));
            }

            var layerNodes = doc.GetElementsByTagName("layer");

            Layers = new List<Layer>();
            foreach (XmlNode layerNode in layerNodes)
            {
                Layers.Add(new Layer(this, layerNode));
            }

            // load object layers
            ObjectGroups = new List<ObjectGroup>();
            foreach (XmlNode objGroupNode in doc.GetElementsByTagName("objectgroup"))
            {
                ObjectGroups.Add(new ObjectGroup(this, objGroupNode));
            }
        }

        public void InitMap()
        {

        }
    }
}
