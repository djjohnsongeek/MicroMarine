using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    internal class Health : Component, Zand.IUpdateable, IRenderable
    {
        public ushort HitPoints;
        public ushort MaxHitPoints;
        public Rectangle HealthRect;
        public Rectangle DamageRect;
        private float _hitPointsPerPixel;
        private Color _damageColor = new Color(30, 0, 0);
        private int _bottomMargin = 5;

        public Health(ushort hp, ushort max = 100)
        {
            HitPoints = hp;
            MaxHitPoints = max;
        }

        public override void OnAddedToEntity()
        {
            HealthRect = new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y - 6, Entity.Dimensions.X, 4);
            DamageRect = new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y - 6, Entity.Dimensions.X, 4);
            _hitPointsPerPixel = (float)Entity.Dimensions.X / (float)MaxHitPoints;
        }

        public void ApplyDamage(ushort value)
        {
            if (value >= HitPoints)
            {
                HitPoints = 0;
                Entity.Destroy();
                Entity.Scene.GetComponent<UnitSelector>().RemoveUnit(Entity);
            }

            HitPoints -= value;
        }

        public void Update()
        {
            Vector2 screenPos = Entity.ScreenPosition;
            HealthRect.X = (int)screenPos.X;
            HealthRect.Y = (int)screenPos.Y - _bottomMargin;

            DamageRect.X = HealthRect.X;
            DamageRect.Y = HealthRect.Y;
        }

        public void Draw(SpriteBatch sbatch)
        {
            if (HitPoints < MaxHitPoints)
            {
                Vector2 start = new Vector2(DamageRect.X, DamageRect.Y);
                Vector2 end = new Vector2(DamageRect.Right, DamageRect.Top);


                int diff = MaxHitPoints - HitPoints;
                int pixelDamage = (int) (diff * _hitPointsPerPixel);


                Vector2 healthEnd = new Vector2(end.X - pixelDamage, end.Y);

                var color = Entity.GetComponent<UnitAllegiance>().Color;



                Shapes.DrawLine(sbatch, Scene.DebugPixelTexture, start, end, DamageRect.Height, _damageColor, .8f);
                Shapes.DrawLine(sbatch, Scene.DebugPixelTexture, start, healthEnd, HealthRect.Height, color, 1);
            }
        }
    }
}
