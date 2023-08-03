﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Zand.Colliders;
using Zand.Debug;
using Zand.Components;
using Zand.Graphics;

namespace Zand.Physics
{
    public class PhysicsManager
    {
        private SpatialHash _spatialHash;
        private List<Collider> _colliders;

        public PhysicsManager()
        {
            _spatialHash = new SpatialHash(64);
            _colliders = new List<Collider>();
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void RemoveCollider(Collider collider)
        {
            _spatialHash.RemoveCollider(collider);
            _colliders.Remove(collider);
        }

        public void Update()
        {
            UpdateSpatialHash();
            ResolveCollisions();
        }

        public void Draw(SpriteBatch sBatch)
        {
            if (DebugTools.Active)
            {
                foreach (var collider in _colliders)
                {
                    collider.Draw(sBatch);
                }
            }
        }

        public Entity GetEntityAtPosition(string entityName, Vector2 position)
        {
            var entities = GetNearbyEntities(entityName, position);
            foreach (var entity in entities)
            {
                var mouseSelectCollider = entity.GetComponent<MouseSelectCollider>(onlyInitialized: false);
                if (mouseSelectCollider.HitBox.Contains(position))
                {
                    return entity;
                }
            }

            return null;
        }

        public List<Entity> GetNearbyEntities(string entityName, Vector2 position)
        {
            var colliders = _spatialHash.GetNearby(position);
            List<Entity> entities = new List<Entity>(colliders.Count);
            foreach (var collider in colliders)
            {
                if (collider.Entity.Name == entityName)
                {
                    entities.Add(collider.Entity);
                }
            }
            entities.TrimExcess();
            return entities;
        }

        public List<Entity> GetEntitiesWithin(Vector2 position, float distance)
        {
            var entities = new List<Entity>();
            var colliders = _spatialHash.GetWithin(position, distance);
            foreach(var collider in colliders)
            {
                entities.Add(collider.Entity);
            }

            return entities;
        }

        private void UpdateSpatialHash()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                if (_colliders[i].Dirty)
                {
                    _spatialHash.RemoveCollider(_colliders[i]);
                    _spatialHash.AddCollider(_colliders[i]);
                }
            }
        }
        private void ResolveCollisions()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].Tint = Color.White;
                IReadOnlyCollection<Collider> possibles = _spatialHash.GetNearby(_colliders[i].Center);
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
                        ApplyRepel(_colliders[i], possibles.ElementAt(j), result);
                        _colliders[i].Tint = Color.Red;
                    }
                }
            }
        }

        private CollisionResult TestCollision(Collider collider1, Collider collider2)
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


        private void ApplyRepel(Collider collider1, Collider collider2, CollisionResult collision)
        {

            var entity1 = collider1.Entity;
            var entity2 = collider2.Entity;

            var entity1Movement = entity1.GetComponent<Mover>();
            var entity2Movement = entity2.GetComponent<Mover>();

            var mapCollider = GetCollider("mapCollider", collider1, collider2);
            var unitCollider = GetCollider("unit", collider1, collider2);

            if (mapCollider != null && unitCollider != null)
            {

                Vector2 newPosition = unitCollider.Entity.Position;

                if (unitCollider.Center.Y < mapCollider.Top || unitCollider.Center.Y > mapCollider.Bottom)
                {
                    //If the circle is ABOVE the square, check against the TOP edge.
                    if (unitCollider.Center.Y < mapCollider.Top)
                    {
                        newPosition.Y = mapCollider.Top - 15;
                    }

                    //If the circle is to the BELOW the square, check against the BOTTOM edge.
                    if (unitCollider.Center.Y > mapCollider.Bottom)
                    {
                        newPosition.Y = mapCollider.Bottom + 3;
                    }
                }
                else
                {
                    //If the circle is to the RIGHT of the square, check against the RIGHT edge.
                    if (unitCollider.Center.X > mapCollider.Right)
                    {
                        newPosition.X = mapCollider.Right + 11;
                    }

                    //If the circle is to the LEFT of the square, check against the LEFT edge.
                    if (unitCollider.Center.X < mapCollider.Left)
                    {
                        newPosition.X = mapCollider.Left - 11;
                    }
                }

                var mover = unitCollider.Entity.GetComponent<Mover>();
                mover.SetPosition(newPosition);
                return;
            }

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

        private Collider GetCollider(string filter, Collider c1, Collider c2)
        {
            if (c1.Entity.Name == filter)
            {
                return c1;
            }

            if (c2.Entity.Name == filter)
            {
                return c2;
            }

            return null;
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
