using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Graphics;

namespace MicroMarine.Components
{
    internal class Health : Component, Zand.IUpdateable, IRenderable
    {
        public ushort HitPoints;
        public ushort MaxHitPoints;
        public Rectangle HealthBar;
        public Rectangle DamageBar;
        public bool Visible;
        private float _hitPointsPerPixel;

        public Health(ushort hp, ushort max = 100)
        {
            HitPoints = hp;
            MaxHitPoints = max;

        }

        public override void OnAddedToEntity()
        {
            HealthBar = new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y - 6, Entity.Dimensions.X, 4);
            DamageBar = new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y - 6, Entity.Dimensions.X, 4);
            _hitPointsPerPixel = (float)Entity.Dimensions.X / (float)MaxHitPoints;
        }

        public void Damage(ushort value)
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
            HealthBar.X = (int)screenPos.X;
            HealthBar.Y = (int)screenPos.Y;

            DamageBar.X = HealthBar.X;
            DamageBar.Y = HealthBar.Y;
        }

        public void Draw(SpriteBatch sbatch)
        {
            if (Visible || HitPoints < MaxHitPoints)
            {
                Vector2 start = new Vector2(DamageBar.X, DamageBar.Y);
                Vector2 end = new Vector2(DamageBar.Right, DamageBar.Top);


                int diff = MaxHitPoints - HitPoints;
                int pixelDamage = (int) (diff * _hitPointsPerPixel);


                Vector2 healthEnd = new Vector2(end.X - pixelDamage, end.Y);



                Shapes.DrawLine(sbatch, Scene.DebugPixelTexture, start, end, DamageBar.Height, Color.Red, .8f);
                Shapes.DrawLine(sbatch, Scene.DebugPixelTexture, start, healthEnd, HealthBar.Height, Color.Green, 1);
            }
        }
    }
}
