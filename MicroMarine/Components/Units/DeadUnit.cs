using Apos.Tweens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
namespace MicroMarine.Components.Units
{
    class DeadUnit : Decale, Zand.IUpdateable
    {
        private Rectangle? _srcRect;
        private float _duration;
        private double _elapsedTime;
        private float _rotation;
        public FloatTween Transparency;

        public DeadUnit(Texture2D texture, Vector2 entityOffset, Rectangle? frame) : base(texture, entityOffset)
        {
            _srcRect = frame;
            _duration = 30;
            _elapsedTime = 0;
            Transparency = new FloatTween(255, 0, 30000, Easing.Linear);
        }

        public override void OnAddedToEntity()
        {
            float max = 6.28f;
            _rotation = (float)Entity.Scene.Rng.NextDouble() * max;

            //RemoveEntity action = new RemoveEntity(Entity.Destroy);

            //Time.AddTimer(_duration, action);
        }

        public void Update()
        {
            _elapsedTime += Time.DeltaTime;

            if (_elapsedTime > _duration)
            {
                Entity.Destroy();
            }
        }

        //public delegate void RemoveEntity();

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
                _layer
             );
        }
    }
}
