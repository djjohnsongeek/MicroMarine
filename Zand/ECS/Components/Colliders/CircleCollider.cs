using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class CircleCollider : Collider
    {
        public float Radius;
        private Texture2D _texture;

        /// <summary>
        ///  Requires a 'Circle' Texture
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="radius"></param>
        public CircleCollider(Texture2D texture, float radius, Vector2 offset) : base()
        {
            Radius = radius;
            _texture = texture;
            Origin = new Vector2(radius, radius);
            Offset = offset;
        }


        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(_texture, Entity.Scene.Camera.GetScreenLocation(Center), null, Tint, 0, Origin, 1, SpriteEffects.None, 0);
        }
    }
}
