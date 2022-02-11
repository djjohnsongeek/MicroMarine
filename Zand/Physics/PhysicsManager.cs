using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zand.Physics
{
    public  class PhysicsManager
    {
        private Scene _scene;
        private SpatialHash _spatialHash;
        private const float UnitRepelMangitude = 2F;
        private List<Collider> _colliders;
        private List<CircleCollider> _circleColliders;

        public PhysicsManager(Scene scene)
        {
            _scene = scene;
            _spatialHash = new SpatialHash(_scene, 64);
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
            UpdateCircleCollidersState();

            for (int i = 0; i < _circleColliders.Count; i++)
            {
                IReadOnlyCollection<CircleCollider> possibles = _spatialHash.GetNearby(_circleColliders[i].Center);

                for (int j = 0; j < possibles.Count; j++)
                {
                    if (possibles.ElementAt(j) == _circleColliders[i])
                    {
                        continue;
                    }

                    CollisionResult collision = CircleCollision(_circleColliders[i], possibles.ElementAt(j));

                    if (collision.Collides)
                    {
                        _circleColliders[i].Tint = Color.Red;
                        possibles.ElementAt(j).Tint = Color.Red;
                        ApplyRepel(_circleColliders[i].Entity, possibles.ElementAt(j).Entity, collision);
                    }
                }
            }
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

        private void UpdateCircleCollidersState()
        {
            _spatialHash.Reset();
            for (int i = 0; i < _circleColliders.Count; i++)
            {
                _spatialHash.AddCollider(_circleColliders[i]);
                _colliders[i].Tint = Color.White;
            }
        }

        private static CollisionResult CircleCollision(CircleCollider collider1, CircleCollider collider2)
        {
            var result = new CollisionResult
            {
                SafeDistance = collider1.Radius + collider2.Radius,
                Distance = Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition),
                Angle = GetAngle(collider1, collider2)
            };

            result.Collides = result.Distance <= result.SafeDistance;
            result.SetRepelPower();

            return result;
        }

        private static float GetAngle(Collider collider1, Collider collider2)
        {
            return (float)Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);
        }

        private static void ApplyRepel(Entity entity1, Entity entity2, CollisionResult collision)
        {
            var repelVelocity1 = new Vector2(
                RepelX(collision.Angle, collision.RepelStrength),
                RepelY(collision.Angle, collision.RepelStrength)
            );

            var repelVelocity2 = Vector2.Multiply(repelVelocity1, -1);

            // adjust
            var entityMovement = entity1.GetComponent<WaypointMovement>();
            var entity2Movement = entity1.GetComponent<WaypointMovement>();

            if (entityMovement.Velocity != Vector2.Zero && entity2Movement.Velocity != Vector2.Zero)
            {
              //  repelVelocity1 = Vector2.Multiply(repelVelocity1, 1.5F);
                repelVelocity2 = Vector2.Multiply(repelVelocity2, 1.5F);
            }

            entity1.GetComponent<WaypointMovement>().Nudge(repelVelocity1);
            entity2.GetComponent<WaypointMovement>().Nudge(repelVelocity2);
        }

        private static float RepelX(double angle, float power)
        {
            return (float)Math.Cos(angle) * power * UnitRepelMangitude;
        }

        private static float RepelY(double angle, float power)
        {
            return (float)Math.Sin(angle) * power * UnitRepelMangitude;
        }
    }
}
