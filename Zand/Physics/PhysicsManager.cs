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
        private const float UnitRepelMangitude = .63F;
        private List<Collider> _colliders = new List<Collider>();

        public PhysicsManager(Scene scene)
        {
            _scene = scene;
            _spatialHash = new SpatialHash(_scene.ScreenWidth, _scene.ScreenHeight, 64);
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void Update()
        {
            ResetCollisionColors();

            // we are collider with our "self"
            for (int i = 0; i < _colliders.Count; i++)
            {
                for (int j = 0; j < _colliders.Count; j++)
                {
                    // skip collision check with self
                    if (j == i) continue;

                    if (AreCircleColliders(_colliders[i], _colliders[j]))
                    {
                        if (CircleCollision(_colliders[i] as CircleCollider, _colliders[j] as CircleCollider))
                        {
                            _colliders[i].Tint = Color.Red;
                            _colliders[j].Tint = Color.Red;

                            var angle = GetAngle(_colliders[i], _colliders[j]);
                            float repelPower = GetRepelPower(_colliders[i] as CircleCollider, _colliders[j] as CircleCollider);

                            ApplyRepel(_colliders[i].Entity, _colliders[j].Entity, angle, repelPower);
                        }
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

        private void ResetCollisionColors()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                _spatialHash.AddCollider(_colliders[i]);
                _colliders[i].Tint = Color.White;
            }
        }

        private bool AreCircleColliders(Collider collider1, Collider collider2)
        {
            return collider1 is CircleCollider && collider2 is CircleCollider;
        }

        private bool CircleCollision(CircleCollider collider1, CircleCollider collider2)
        {
            return Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition) <= collider1.Radius + collider2.Radius;
        }

        private double GetAngle(Collider collider1, Collider collider2)
        {
            return Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);
        }

        private float GetRepelPower(CircleCollider collider1, CircleCollider collider2)
        {
            return collider1.Radius + collider2.Radius + Vector2.Distance(collider1.Entity.Position, collider2.Entity.Position) / collider1.Radius + collider2.Radius;
        }

        private void ApplyRepel(Entity entity1, Entity entity2, double angle, float power)
        {
            var repelVelocity1 = new Vector2(RepelX(angle, power), RepelY(angle, power));
            var repelVelocity2 = Vector2.Multiply(repelVelocity1, -1);

            entity1.GetComponent<WaypointMovement>().Nudge(repelVelocity1);
            entity2.GetComponent<WaypointMovement>().Nudge(repelVelocity2);


            //entity1.Position.X += (float)Math.Cos(angle) * power * UnitRepelMangitude;
            //entity1.Position.Y += (float)Math.Sin(angle) * power * UnitRepelMangitude;
            //entity2.Position.X -= (float)Math.Cos(angle) * power * UnitRepelMangitude;
            //entity2.Position.Y -= (float)Math.Sin(angle) * power * UnitRepelMangitude;
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
