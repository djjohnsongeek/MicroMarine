using System;
using System.Collections.Generic;
using System.Linq;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zand.Physics
{
    public  class PhysicsManager
    {
        private SpatialHash _spatialHash;
        private const float UnitRepelMangitude = 2.5F;
        private List<Collider> _colliders;
        private List<CircleCollider> _circleColliders;

        public PhysicsManager()
        {
            _spatialHash = new SpatialHash(64);
            _colliders = new List<Collider>();
            _circleColliders = new List<CircleCollider>();
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
            if (collider is CircleCollider)
            {
                _circleColliders.Add(collider as CircleCollider);
            }
        }

        public void Update()
        {
            UpdateSpatialHash();
            ResolveCollisions();
        }

        public void Draw(SpriteBatch sBatch)
        {
            if (Core._instance.CurrentScene.ShowDebug)
            {
                foreach (var collider in _colliders)
                {
                    collider.Draw(sBatch);
                }
            }
        }

        private void UpdateSpatialHash()
        {
            // Further optimization: only reset colliders that are in motion
            for (int i = 0; i < _circleColliders.Count; i++)
            {
                _spatialHash.RemoveCollider(_circleColliders[i]);
                _spatialHash.AddCollider(_circleColliders[i]);
                _colliders[i].Tint = Color.White;
            }
        }
        private void ResolveCollisions()
        {
            for (int i = 0; i < _circleColliders.Count; i++)
            {
                IReadOnlyCollection<CircleCollider> possibles = _spatialHash.GetNearby(_circleColliders[i].Center);
                for (int j = 0; j < possibles.Count; j++)
                {
                    // Don't Collide with self
                    if (possibles.ElementAt(j) == _circleColliders[i])
                    {
                        continue;
                    }

                    CollisionResult result = ResolveCollision(_circleColliders[i], possibles.ElementAt(j));

                    if (result.Collides)
                    {
                        ApplyRepel(_circleColliders[i].Entity, possibles.ElementAt(j).Entity, result);
                    }
                }
            }
        }

        private CollisionResult ResolveCollision(CircleCollider collider1, CircleCollider collider2)
        {
            var result = new CollisionResult
            {
                SafeDistance = collider1.Radius + collider2.Radius,
                Distance = Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition),
                Angle = GetAngle(collider1, collider2)
            };

            result.Collides = result.Distance <= result.SafeDistance;
            result.SetRepelStrength();

            return result;
        }

        private float GetAngle(Collider collider1, Collider collider2)
        {
            var angle =  (float)Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);

            return angle;
        }

        private void ApplyRepel(Entity entity1, Entity entity2, CollisionResult collision)
        {
            var repelVelocity1 = new Vector2(
                GetRepelX(collision.Angle, collision.RepelStrength),
                GetRepelY(collision.Angle, collision.RepelStrength)
            );

            var repelVelocity2 = Vector2.Multiply(repelVelocity1, -1);

            var entity1Movement = entity1.GetComponent<WaypointMovement>();
            var entity2Movement = entity2.GetComponent<WaypointMovement>();

            if (entity1Movement.CurrentWayPoint == null && entity2Movement.CurrentWayPoint != null)
            {
                entity1Movement.Nudge(repelVelocity1);
            }
            else if (entity2Movement.CurrentWayPoint == null && entity1Movement.CurrentWayPoint != null)
            {
                entity2Movement.Nudge(repelVelocity2);
            }
            else if ((entity1Movement.CurrentWayPoint != null && entity2Movement.CurrentWayPoint != null)
                ||
                    (entity1Movement.CurrentWayPoint == null && entity2Movement.CurrentWayPoint == null))
            {
                entity1Movement.Nudge(repelVelocity1);
                entity2Movement.Nudge(repelVelocity2);
            }
        }

        private float GetRepelX(double angle, float power)
        {
            return (float)Math.Cos(angle) * power * UnitRepelMangitude;
        }

        private float GetRepelY(double angle, float power)
        {
            return (float)Math.Sin(angle) * power * UnitRepelMangitude;
        }
    }
}
