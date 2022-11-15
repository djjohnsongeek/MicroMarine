﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Colliders;
using Zand.ECS.Components;

namespace Zand.Physics
{
    public static class Collisions
    {
        #region Circles

        public static bool CircleToPoint(CircleCollider circle, Vector2 point)
        {
            return Vector2.DistanceSquared(circle.Entity.Position, point) < circle.Radius * circle.Radius;
        }

        public static bool CircleToPoint(CircleCollider circle, Point point)
        {
            return Vector2.DistanceSquared(circle.Entity.Position, point.ToVector2()) > circle.Radius * circle.Radius;
        }

        #endregion

        public static CollisionResult CircleToCircle(CircleCollider collider1, CircleCollider collider2)
        {
            var result = new CollisionResult
            {
                SafeDistance = collider1.Radius + collider2.Radius,
                Distance = Vector2.Distance(collider1.Entity.ScreenPosition, collider2.Entity.ScreenPosition),
                Angle = Collisions.GetAngle(collider1, collider2)
            };

            result.Collides = result.Distance <= result.SafeDistance;
            result.SetRepelStrength();

            return result;
        }

        public static CollisionResult CircleToBox(CircleCollider circle, BoxCollider box)
        {
            CollisionResult result = new CollisionResult
            {
                SafeDistance = circle.Radius
            };

            float testX = circle.Center.X;
            float testY = circle.Center.Y;


            //If the circle is to the RIGHT of the square, check against the RIGHT edge.
            if (circle.Center.X > box.HitBox.Right)
            {
                testX = box.HitBox.Right;
            }

            //If the circle is to the LEFT of the square, check against the LEFT edge.
            if (circle.Center.X < box.HitBox.X)
            {
                testX = box.HitBox.Left;
            }

            //If the circle is ABOVE the square, check against the TOP edge.
            if (circle.Center.Y < box.HitBox.Top)
            {
                testY = box.HitBox.Top;
            }

            //If the circle is to the BELOW the square, check against the BOTTOM edge.
            if (circle.Center.Y > box.HitBox.Bottom)
            {
                testY = box.HitBox.Bottom;
            }

            float distanceX = circle.Center.X - testX;
            float distanceY = circle.Center.Y - testY;
            result.Distance = (float) Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            result.Angle = Collisions.GetAngle(circle, box);
            result.Collides = result.Distance < result.SafeDistance;
            result.SetRepelStrength();
            return result;
        }

        public static CollisionResult BoxToCircle(BoxCollider box, CircleCollider circle)
        {
            return CircleToBox(circle, box);
        }

        public static CollisionResult BoxToBox(BoxCollider box1, BoxCollider box2)
        {
            return new CollisionResult
            {
                Collides = false,
            };
        }

        #region Rectangles


        #endregion

        public static float GetAngle(Collider collider1, Collider collider2)
        {
            var angle = (float)Math.Atan2(
                collider1.Entity.Position.Y - collider2.Entity.Position.Y,
                collider1.Entity.Position.X - collider2.Entity.Position.X);

            return angle;
        }
    }
}
