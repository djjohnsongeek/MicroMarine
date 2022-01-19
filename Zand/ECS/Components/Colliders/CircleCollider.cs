using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Offset = offset;
        }


        public override void Draw(SpriteBatch sbatch)
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(Position);
            sbatch.Draw(_texture, screenPos, null, Color.White);
        }
    }
}
