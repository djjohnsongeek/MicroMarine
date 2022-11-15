using System;
using System.Collections.Generic;
using System.Linq;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Zand.Colliders;

namespace Zand.Physics
{
    public class PhysicsManager
    {
        private SpatialHash _spatialHash;
        private List<ICollider> _colliders;

        public PhysicsManager()
        {
            _spatialHash = new SpatialHash(64);
            _colliders = new List<ICollider>();
        }

        public void AddCollider(ICollider collider)
        {
            _colliders.Add(collider);
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
            for (int i = 0; i < _colliders.Count; i++)
            {
                _spatialHash.RemoveCollider(_colliders[i]);
                _spatialHash.AddCollider(_colliders[i]);
            }
        }
        private void ResolveCollisions()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                IReadOnlyCollection<ICollider> possibles = _spatialHash.GetNearby(_colliders[i].Center);
                for (int j = 0; j < possibles.Count; j++)
                {
                    // Don't Collide with self
                    if (possibles.ElementAt(j) == _colliders[i])
                    {
                        continue;
                    }

                    CollisionResult result = TestCollision(_colliders[i], possibles.ElementAt(j));

                    if (result.Collides)
                    {
                        ApplyRepel(_colliders[i].Entity, possibles.ElementAt(j).Entity, result);
                    }
                }
            }
        }

        private CollisionResult TestCollision(ICollider collider1, ICollider collider2)
        {
            if (collider1 is CircleCollider && collider2 is CircleCollider)
            {
                return Collisions.CircleToCircle(collider1 as CircleCollider, collider2 as CircleCollider);
            }
            else if (collider1 is BoxCollider && collider2 is BoxCollider)
            {
                return Collisions.BoxToBox(collider1 as BoxCollider, collider2 as BoxCollider);
            }
            else if (collider1 is BoxCollider)
            {
                return Collisions.BoxToCircle(collider1 as BoxCollider, collider2 as CircleCollider);
            }
            else
            {
                return Collisions.CircleToBox(collider1 as CircleCollider, collider2 as BoxCollider);
            }
        }


        private void ApplyRepel(Entity entity1, Entity entity2, CollisionResult collision)
        {
            var repelVelocity1 = new Vector2(
                GetRepelX(collision.Angle, collision.RepelStrength),
                GetRepelY(collision.Angle, collision.RepelStrength)
            );

            var repelVelocity2 = Vector2.Multiply(repelVelocity1, -1);


            if (entity1.GetComponent<Collider>().Static)
            {
                repelVelocity1 = Vector2.Zero;
            }

            if (entity2.GetComponent<Collider>().Static)
            {
                repelVelocity2 = Vector2.Zero;
            }

            var entity1Movement = entity1.GetComponent<Mover>();
            var entity2Movement = entity2.GetComponent<Mover>();

            // adjustments
            //if (entity1Movement.CurrentWayPoint == null && entity2Movement.CurrentWayPoint != null)
            //{
            //    repelVelocity1 = Vector2.Multiply(repelVelocity1, 3);
            //}

            //if (entity2Movement.CurrentWayPoint == null && entity1Movement.CurrentWayPoint != null)
            //{
            //    repelVelocity2 = Vector2.Multiply(repelVelocity2, 3);
            //}

            entity1Movement?.Nudge(repelVelocity1);
            entity2Movement?.Nudge(repelVelocity2);



            //if (entity1Movement.CurrentWayPoint == null && entity2Movement.CurrentWayPoint != null)
            //{
            //    entity1Movement.Nudge(repelVelocity1);
            //}
            //else if (entity2Movement.CurrentWayPoint == null && entity1Movement.CurrentWayPoint != null)
            //{
            //    entity2Movement.Nudge(repelVelocity2);
            //}
            //else if ((entity1Movement.CurrentWayPoint != null && entity2Movement.CurrentWayPoint != null)
            //    ||
            //        (entity1Movement.CurrentWayPoint == null && entity2Movement.CurrentWayPoint == null))
            //{
            //    entity1Movement.Nudge(repelVelocity1);
            //    entity2Movement.Nudge(repelVelocity2);
            //}


        }

        private float GetRepelX(double angle, float power)
        {
            return (float)Math.Cos(angle) * power * Config.UnitRepelMangitude;
        }

        private float GetRepelY(double angle, float power)
        {
            return (float)Math.Sin(angle) * power * Config.UnitRepelMangitude;
        }
    }
}
