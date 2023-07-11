using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zand;
namespace MicroMarine.Components.Units
{
    class DeadUnit : Decale, Zand.IUpdateable
    {
        private Rectangle? _srcRect;
        private float _duration;
        private double _elapsedTime;

        public DeadUnit(Texture2D texture, Vector2 entityOffset, Rectangle? frame) : base(texture, entityOffset)
        {
            _srcRect = frame;
            _duration = 30;
            _elapsedTime = 0;
        }

        public void Update()
        {
            _elapsedTime += Time.DeltaTime;
            if (_elapsedTime > _duration)
            {
                Entity.Destroy();
            }
        }

        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(
                texture,
                Entity.Position,
                _srcRect,
                Color.White,
                0,
                _entityOffset,
                1,
                SpriteEffects.None,
                _layer
             );
        }
    }
}
