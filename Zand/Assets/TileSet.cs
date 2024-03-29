﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
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
        public string ImageSource;
        public int ImageWidth;
        public int ImageHeight;
        public Texture2D Texture;
        public TiledMap Map;

        public Dictionary<int, Rectangle> TileBank;

        public TileSet(TiledMap map, XmlNode tilesetNode)
        {
            Map = map;
            var tilesetImageNode = tilesetNode.FirstChild;

            // Parse Attributes
            var attributes = tilesetNode.Attributes;

            FirstGId = int.Parse(attributes.GetNamedItem("firstgid").Value);
            Name = attributes.GetNamedItem("name").Value;
            TileWidth = int.Parse(attributes.GetNamedItem("tilewidth").Value);
            TileHeight = int.Parse(attributes.GetNamedItem("tileheight").Value);
            TileCount = int.Parse(attributes.GetNamedItem("tilecount").Value);
            Columns = int.Parse(attributes.GetNamedItem("columns").Value);

            attributes = tilesetImageNode.Attributes;
            ImageSource = attributes.GetNamedItem("source").Value;
            ImageWidth = int.Parse(attributes.GetNamedItem("width").Value);
            ImageHeight = int.Parse(attributes.GetNamedItem("height").Value);

            Rows = ImageHeight / TileWidth;
            LastGId = TileCount + FirstGId - 1;

            // Populate Tile Bank
            int tileGid = FirstGId;
            TileBank = new Dictionary<int, Rectangle>(TileCount);
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    TileBank[tileGid] = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                    tileGid++;
                }
            }
            TileBank.TrimExcess();

            // Load Texture
            Texture = Texture2D.FromStream(
                Core.GraphicsManager.GraphicsDevice,
                File.OpenRead(Path.Join(Map.CustomProperties["ContentDirectory"], ImageSource))
            );
        }

        public Rectangle GetSrcRect(int tileId)
        {
            return TileBank[tileId];
        }

        public bool Contains(int gid)
        {
            return gid >= FirstGId && gid <= LastGId;
        }
    }
}
