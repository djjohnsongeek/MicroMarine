using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Physics
{
    public interface ICollider
    {
        public Vector2 Center { get; }
        public Vector2 TopRight { get; }
        public Vector2 TopLeft { get; }
        public Vector2 BottomLeft { get; }
        public Vector2 BottomRight { get; }

        public Entity Entity { get; }

        public void Draw(SpriteBatch sBatch) { }
    }
}
