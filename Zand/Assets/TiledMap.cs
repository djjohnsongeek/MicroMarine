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


        public TiledMap(string filename)
        {
            var doc = new XmlDocument();
            doc.Load(filename);


            var mapNode = doc.DocumentElement.FirstChild;
            var attributes = mapNode.Attributes;

            Width = int.Parse(attributes.GetNamedItem("width").Value);
            Height = int.Parse(attributes.GetNamedItem("height").Value);
            TileWidth = int.Parse(attributes.GetNamedItem("tilewidth").Value);
            TileHeight = int.Parse(attributes.GetNamedItem("tileheight").Value);
            NextLayerId = int.Parse(attributes.GetNamedItem("nextlayerid").Value);
            NextObjectId = int.Parse(attributes.GetNamedItem("nextobjectid").Value);

            TileSets = new List<TileSet>();

            var tileSetNodes = doc.GetElementsByTagName("tileset");

            // Parse Tilesets
            foreach (XmlNode node in tileSetNodes)
            {
                attributes = node.Attributes;
                int firstgId = int.Parse(attributes.GetNamedItem("firstgid").Value);
                string source = attributes.GetNamedItem("source").Value;

                TileSets.Add(new TileSet(firstgId, source));
            }

            var layerNodes = doc.GetElementsByTagName("layer");

            foreach (XmlNode layerNode in layerNodes)
            {

            }

            //Read in the TMX file.
            //Parse the TMX file as an XML file.
            //Load all the tileset images.
            //Arrange the tileset images into our map layout, layer by layer.
            //Read map object.
        }
    }
}
