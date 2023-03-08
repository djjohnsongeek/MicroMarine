using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.Debug;

namespace Zand.ECS.Components
{
    public class Mover : Component, IUpdateable
    {
        private float maxSpeed;
        public Vector2 Velocity = Vector2.Zero;
        private TileMap _tileMap = null;
        public UnitDirection Orientation { get; private set; }

        public Mover (float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
            Orientation = UnitDirection.South;
        }

        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime;
            UpdateOrientation();
            GetTileMap().ResolveMapCollisions(Entity.GetComponent<CircleCollider>());
            UpdateEntityLayerDepth();
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += velocity * (float)Time.DeltaTime;
        }

        private void UpdateEntityLayerDepth()
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(Entity.Position);
            Entity.layerDepth = MathUtil.CalculateLayerDepth(screenPos.Y, Entity.Dimensions.Y);
        }

        private TileMap GetTileMap()
        {
            if (_tileMap != null)
            {
                return _tileMap;
            }

            _tileMap = Scene.FindEntity("tileMap").GetComponent<TileMap>();
            return _tileMap;
        }

        private void UpdateOrientation()
        {
            Vector2 currentVelocity = Velocity;
            currentVelocity.Normalize();
            if (currentVelocity != Vector2.Zero)
            {
                float dot = Vector2.Dot(Vector2.UnitX, currentVelocity);
                // close to zero, traveling up or down
                if (dot > -0.5F && dot < 0.5F)
                {

                    if (currentVelocity.Y < 0)
                    {
                        Orientation = UnitDirection.North;
                    }
                    else if (currentVelocity.Y > 0)
                    {
                        Orientation = UnitDirection.South;
                    }
                }
                // close to 1 traveling more horizontal
                if (dot < -0.5 || dot > 0.5F)
                {
                    if (currentVelocity.X > 0)
                    {
                        Orientation = UnitDirection.East;

                    }
                    else if (currentVelocity.X < 0)
                    {
                        Orientation = UnitDirection.West;
                    }
                }
            }
        }
    }

    public enum UnitDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
}
