using Microsoft.Xna.Framework;
using System.Xml;
using System;
using Zand.Colliders;

namespace Zand.Assets
{
    public class ObjectGroup
    {
        public int Id;
        public string Name;
        public TiledObject[] Objects;
        public TiledMap Map;

        public ObjectGroup(TiledMap map, XmlNode node)
        {
            Map = map;
            var attributes = node.Attributes;

            Id = int.Parse(attributes.GetNamedItem("id").Value);
            Name = attributes.GetNamedItem("name").Value;

            var objectNodes = node.ChildNodes;
            Objects = new TiledObject[objectNodes.Count];
            for (int i = 0; i < objectNodes.Count; i++)
            {
                Objects[i] = new TiledObject(objectNodes[i]);
            }
        }

        public TiledObject GetObject(string name)
        {
            foreach (var tiledObject in Objects)
            {
                if (tiledObject.Name == name)
                {
                    return tiledObject;
                }
            }

            throw new Exception($"Object '{name}' does not exits.");
        }
    }

    public class TiledObject
    {
        public int Id;
        public Vector2 Position;
        public Vector2? Dimensions;
        public string Name;
        public TiledObjectType Type;

        public TiledObject(XmlNode node)
        {
            var attributes = node.Attributes;
            Id = int.Parse(attributes.GetNamedItem("id").Value);
            Name = attributes.GetNamedItem("name")?.Value;
            Position = new Vector2(
                float.Parse(attributes.GetNamedItem("x").Value),
                float.Parse(attributes.GetNamedItem("y").Value)
            );

            string strW = attributes.GetNamedItem("width")?.Value;
            string strH = attributes.GetNamedItem("height")?.Value;

            if (string.IsNullOrEmpty(strH) || string.IsNullOrEmpty(strW))
            {
                Dimensions = null;
                return;
            }

            Dimensions = new Vector2(float.Parse(strW), float.Parse(strH));


            if (node["ellipse"]?.Value != null)
            {
                Type = TiledObjectType.Ellipse;
            }
            else if (node["point"]?.Value != null)
            {
                Type = TiledObjectType.Point;
            }
            else
            {
                Type = TiledObjectType.Rect;
            }

        }
    }

    public enum TiledObjectType
    {
        Rect,
        Ellipse,
        Point,
    }
}
