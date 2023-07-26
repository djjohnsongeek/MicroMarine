using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zand.Assets
{
    public class TileSet
    {
        public int FirstGId;
        public int LastGId;
        public string Name;
        public int TileWidth;
        public int TileHeight;
        public int TileCount;
        public int Columns;
        public int Rows;
        public string ImagePath;
        public int ImageWidth;
        public int ImageHeight;
        public Texture2D Texture;

        public TileSet(int firstgid, string path)
        {
            FirstGId = firstgid;
            var doc = new XmlDocument();
            doc.Load(path);

            var tilesetNode = doc.DocumentElement;
            var tilesetImageNode = tilesetNode.FirstChild;
            var attributes = tilesetNode.Attributes;

            Name = attributes.GetNamedItem("name").Value;
            TileWidth = int.Parse(attributes.GetNamedItem("tilewidth").Value);
            TileHeight = int.Parse(attributes.GetNamedItem("tileheight").Value);
            TileCount = int.Parse(attributes.GetNamedItem("tilecount").Value);
            Columns = int.Parse(attributes.GetNamedItem("columns").Value);

            attributes = tilesetImageNode.Attributes;
            ImagePath = attributes.GetNamedItem("source").Value;
            ImageWidth = int.Parse(attributes.GetNamedItem("width").Value);
            ImageHeight = int.Parse(attributes.GetNamedItem("height").Value);

            Rows = ImageHeight / TileWidth;
            LastGId = TileCount + FirstGId - 1;




            Texture = Texture2D.FromStream(Core.GraphicsManager.GraphicsDevice, File.OpenRead(ImagePath));
        }

        public Rectangle GetTileBounds(int tileId)
        {
            if (tileId > LastGId || tileId < FirstGId)
            {
                throw new ArgumentOutOfRangeException($"Tile Id {tileId} does not exist in the {Name} Tileset");
            }




            return new Rectangle();
        }

    }
}
