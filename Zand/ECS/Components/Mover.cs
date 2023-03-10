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
            Orientation = DetermineUnitDirection(Velocity);
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

        public static UnitDirection DetermineUnitDirection(Vector2 velocity)
        {
            return DetermineUnitDirection(Vector2.Zero, velocity);
        }

        public static UnitDirection DetermineUnitDirection(Vector2 agentPosition, Vector2 targetPosition)
        {
            var difference = targetPosition - agentPosition;
            difference.Normalize();

            float dot = Vector2.Dot(Vector2.UnitX, difference);
            UnitDirection orientation = UnitDirection.North;
            // close to zero, traveling up or down
            if (dot > -0.5F && dot < 0.5F)
            {
                if (difference.Y < 0)
                {
                    orientation = UnitDirection.North;
                }
                else if (difference.Y > 0)
                {
                    orientation = UnitDirection.South;
                }
            }
            // close to 1 traveling more horizontal
            if (dot < -0.5 || dot > 0.5F)
            {
                if (difference.X > 0)
                {
                    orientation = UnitDirection.East;

                }
                else if (difference.X < 0)
                {
                    orientation = UnitDirection.West;
                }
            }

            return orientation;
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
