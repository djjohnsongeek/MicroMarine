﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml;
using Zand.Colliders;

namespace Zand.Assets
{
    public class TiledMap : Component
    {
        public int Width;
        public int Height;
        public int TileWidth;
        public int TileHeight;
        public int NextLayerId;
        public int NextObjectId;
        public Vector2 Center => new Vector2(Width * TileWidth * .5f, Height * TileWidth * .5f);

        public List<TileSet> TileSets;
        public List<Layer> Layers;
        public List<ObjectGroup> ObjectGroups;

        public Dictionary<string, string> CustomProperties;


        public TiledMap(string filename)
        {
            CustomProperties = new Dictionary<string, string>();
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

            // Custom Properties
            var propertiesNode = doc.GetElementsByTagName("properties")[0];
            foreach (XmlNode property in propertiesNode.ChildNodes)
            {
                string propName = property.Attributes.GetNamedItem("name").Value;
                string propValue = property.Attributes.GetNamedItem("value").Value;
                CustomProperties[propName] = propValue;
            }
            var tileSetNodes = doc.GetElementsByTagName("tileset");

            // Parse Tilesets
            TileSets = new List<TileSet>();
            foreach (XmlNode node in tileSetNodes)
            {
                TileSets.Add(new TileSet(this, node));
            }

            var layerNodes = doc.GetElementsByTagName("layer");

            Layers = new List<Layer>();
            foreach (XmlNode layerNode in layerNodes)
            {
                var layer = new Layer(this, layerNode);
                Layers.Add(layer);
            }

            // load object layers
            ObjectGroups = new List<ObjectGroup>();
            foreach (XmlNode objGroupNode in doc.GetElementsByTagName("objectgroup"))
            {
                ObjectGroups.Add(new ObjectGroup(this, objGroupNode));
            }
        }

        public override void OnAddedToEntity()
        {
            // register renderables
            foreach (var layer in Layers)
            {
                Scene.RenderableComponents.Add(layer);
            }

            // register colliders
            foreach (var objectGroup in ObjectGroups)
            {
                if (CustomProperties["CollisionLayer"] == objectGroup.Name)
                {
                    foreach (var obj in objectGroup.Objects)
                    {
                        if (obj.Type == TiledObjectType.Rect)
                        {
                            var colliderEntity = Scene.CreateEntity("mapCollider", obj.Position, obj.Dimensions.Value.ToPoint());
                            var collider = new BoxCollider(new Rectangle(obj.Position.ToPoint(), obj.Dimensions.Value.ToPoint()), Vector2.Zero);
                            collider.Static = true;
                            colliderEntity.AddComponent(collider);
                            Scene.RegisterCollider(collider);
                        }
                    }
                }

            }
        }

        private Layer GetLayer(string name)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].Name == name)
                {
                    return Layers[i];
                }
            }

            return null;
        }

        public void SetLayerRenderLayer(string name, int renderLayer, float renderDepth = 0)
        {
            var layer = GetLayer(name);
            if (layer is null)
            {
                throw new NullReferenceException($"No layer with the name {name} could be found.");
            }

            layer.RenderLayer = renderLayer;
            layer.RenderDepth = renderDepth;
        }

        public TileSet GetGIdTileSet(int gId)
        {
            for (int i = 0; i < TileSets.Count; i++)
            {
                if (TileSets[i].Contains(gId))
                {
                    return TileSets[i];
                }
            }

            return null;
        }

        public TiledObject GetObject(string groupName, string objectName)
        {
            return GetObjectGroup(groupName).GetObject(objectName);
        }

        private ObjectGroup GetObjectGroup(string name)
        {
            for (int i = 0; i < ObjectGroups.Count; i++)
            {
                if (ObjectGroups[i].Name == name)
                {
                    return ObjectGroups[i];
                }
            }

            throw new Exception("Object Group not found.");
        }
    }
}
