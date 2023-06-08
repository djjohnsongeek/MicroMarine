using Microsoft.Xna.Framework;

namespace Zand
{
    public struct Circle
    {
        public Vector2 Center;
        public float Radius;
        public float Diameter => Radius * 2;

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}
