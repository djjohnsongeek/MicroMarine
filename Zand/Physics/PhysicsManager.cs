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
        private const float UnitRepelMangitude = .73F;
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

        private bool AreCircleColliders(Collider collider1, Collider collider2)
        {
            return collider1 is CircleCollider && collider2 is CircleCollider;
        }

        private CollisionResult CircleCollision(CircleCollider collider1, CircleCollider collider2)
        {
            CollisionResult result = new CollisionResult();

            result.Distance = Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition);
            result.SafeDistance = collider1.Radius + collider2.Radius;
            result.Collides = result.Distance <= result.SafeDistance;
            result.Angle = GetAngle(collider1, collider2);
            result.SetRepelPower();

            return result;
        }

        private double GetAngle(Collider collider1, Collider collider2)
        {
            return Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);
        }

        private float GetRepelPower(CircleCollider collider1, CircleCollider collider2, float distance)
        {
            return collider1.Radius + collider2.Radius + (distance / (collider1.Radius + collider2.Radius));
        }

        private void ApplyRepel(Entity entity1, Entity entity2, CollisionResult collision)
        {
            var repelVelocity1 = new Vector2(
                RepelX(collision.Angle, collision.RepelStrength),
                RepelY(collision.Angle, collision.RepelStrength)
            );

            var repelVelocity2 = Vector2.Multiply(repelVelocity1, -1);

            entity1.GetComponent<WaypointMovement>().Nudge(repelVelocity1);
            entity2.GetComponent<WaypointMovement>().Nudge(repelVelocity2);
        }

        private float RepelX(double angle, float power)
        {
            return (float)Math.Cos(angle) * power * UnitRepelMangitude;
        }

        private float RepelY(double angle, float power)
        {
            return (float)Math.Sin(angle) * power * UnitRepelMangitude;
        }
    }
}
