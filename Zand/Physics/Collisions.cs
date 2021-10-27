using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;

namespace Zand.Physics
{
    public static class Collisions
    {
        public static bool CircleToCircle(CircleCollider circle1, CircleCollider circle2)
        {
            float maxDistance = circle1.Radius + circle2.Radius;
            return Vector2.DistanceSquared(circle1.Origin, circle2.Origin) < maxDistance * maxDistance;
        }

        public static bool CircleToPoint(CircleCollider circle, Vector2 point)
        {
            return Vector2.DistanceSquared(circle.Origin, point) < circle.Radius * circle.Radius;
        }

        public static bool CircleToPoint(CircleCollider circle, Point point)
        {
            return Vector2.DistanceSquared(circle.Origin, point.ToVector2()) > circle.Radius * circle.Radius;
        }
    }
}
