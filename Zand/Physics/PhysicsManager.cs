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
    public static class PhysicsManager
    {
        private static List<Collider> _colliders = new List<Collider>();

        public static void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public static void Update()
        {
            ResetCollisionColors();

            // we are collider with our "self"
            for (int i = 0; i < _colliders.Count; i++)
            {
                for (int j = 0; j < _colliders.Count; j++)
                {
                    if (j == i) continue;
                    if (_colliders[i] is CircleCollider && _colliders[j] is CircleCollider)
                    {
                        if (OverLaps(_colliders[i] as CircleCollider, _colliders[j] as CircleCollider))
                        {
                            _colliders[i].Tint = Color.Red;
                            var angle = GetAngle(_colliders[i], _colliders[j]);
                            float force = 0.1f;
                            float repelPower = GetRepelPower(_colliders[i] as CircleCollider, _colliders[j] as CircleCollider);
                            ApplyRepel(_colliders[i].Entity, _colliders[j].Entity, angle, force, repelPower);
                        }
                    }
                }
            }
        }

        public static void Draw(SpriteBatch sBatch)
        {
            if (Core._instance.CurrentScene.ShowDebug)
            {
                foreach (var collider in _colliders)
                {
                    collider.Draw(sBatch);
                }
            }
        }

        private static void ResetCollisionColors()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].Tint = Color.White;
            }
        }

        private static bool IsCloseEnough(Collider collider1, Collider collider2)
        {
            return Math.Abs(collider1.Entity.ScreenPosition.X - collider2.Entity.ScreenPosition.X) <= 45
                && Math.Abs(collider1.Entity.ScreenPosition.Y - collider2.Entity.ScreenPosition.Y) <= 45;
        }

        private static bool OverLaps(CircleCollider collider1, CircleCollider collider2)
        {
            return Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition) <= (collider1.Radius + collider2.Radius);
        }

        private static double GetAngle(Collider collider1, Collider collider2)
        {
            return Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);
        }

        private static float GetRepelPower(CircleCollider collider1, CircleCollider collider2)
        {
            return collider1.Radius + collider2.Radius + Vector2.Distance(collider1.Entity.Position, collider2.Entity.Position) / collider1.Radius + collider2.Radius;
        }

        private static void ApplyRepel(Entity entity1, Entity entity2, double angle, float force, float power)
        {
            entity1.Position.X += (float) Math.Cos(angle) * power * force;
            entity1.Position.Y += (float) Math.Sin(angle) * power * force;
            entity2.Position.X -= (float) Math.Cos(angle) * power * force;
            entity2.Position.Y -= (float) Math.Sin(angle) * power * force;
        }
    }
}
