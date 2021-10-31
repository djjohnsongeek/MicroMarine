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
        #region Circles
        public static bool CircleToCircle(CircleCollider circle1, CircleCollider circle2)
        {
            float maxDistance = circle1.Radius + circle2.Radius;
            return Vector2.DistanceSquared(circle1.Position, circle2.Position) < maxDistance * maxDistance;
        }

        public static bool CircleToPoint(CircleCollider circle, Vector2 point)
        {
            return Vector2.DistanceSquared(circle.Position, point) < circle.Radius * circle.Radius;
        }

        public static bool CircleToPoint(CircleCollider circle, Point point)
        {
            return Vector2.DistanceSquared(circle.Position, point.ToVector2()) > circle.Radius * circle.Radius;
        }

        #endregion

        #region Rectangles

        public static bool RectangleToPoint(Rectangle rectangle, Vector2 point)
        {
            return
                point.X >= rectangle.X && point.X <= (rectangle.X + rectangle.Width)
                &&
                point.Y >= rectangle.Y && point.Y <= (rectangle.Y + rectangle.Height);
        }

        public static bool RectangleToPoint(Rectangle rectangle, Point point)
        {
            return RectangleToPoint(rectangle, point.ToVector2());
        }

        #endregion
    }
}
