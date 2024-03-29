﻿using Apos.Tweens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
namespace MicroMarine.Components.Units
{
    class DeadUnit : SimpleSprite
    {
        private Rectangle? _srcRect;
        private float _duration;
        private float _rotation;
        public FloatTween Transparency;

        /// <summary>
        /// Simple decal that fades out over time.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="entityOffset"></param>
        /// <param name="frame"></param>
        /// <param name="fadeDuration"> In seconds</param>

        public DeadUnit(Texture2D texture, Vector2 entityOffset, Rectangle? frame, float fadeDuration) : base(texture, entityOffset)
        {
            _srcRect = frame;
            _duration = fadeDuration;
            Transparency = new FloatTween(255, 0, (long)_duration * 1000, Easing.SineIn);
        }

        public override void OnAddedToEntity()
        {
            Entity.Dimensions = new Point(texture.Width, texture.Height);
            float max = 6.28f;
            _rotation = (float)Entity.Scene.Rng.NextDouble() * max;
            Time.AddTimer(_duration, Entity.Destroy);
            RenderLayer = 1;
            RenderDepth = Calc.CalculateRenderDepth(Entity.Position.Y, Entity.Dimensions.Y);
        }

        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(
                texture,
                Entity.Position,
                _srcRect,
                new Color(255, 255, 255, (int)Transparency.Value),
                _rotation,
                _entityOffset,
                1,
                SpriteEffects.None,
                0 // ignored now
             );
        }
    }
}
