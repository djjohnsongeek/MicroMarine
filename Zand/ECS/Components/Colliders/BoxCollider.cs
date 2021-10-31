using Microsoft.Xna.Framework;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider
    {
        public Rectangle HitBox { get; private set; }

        public BoxCollider(Rectangle hitBox)
        {
            HitBox = hitBox;
        }

        public BoxCollider(Vector2 position, int width, int height)
        {
            var rect = new Rectangle((int)position.X, (int)position.Y, width, height);
        }
    }
}
