using Microsoft.Xna.Framework;

namespace Zand.AI
{
    public class Destination
    {
        private const float DefaultStartRadius = 9f;
        public float Radius;
        public Vector2 Position;

        public Destination(Vector2 position)
        {
            Position = position;
            Radius = DefaultStartRadius;
        }
    }
}
