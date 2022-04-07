using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    internal class Health : Component, Zand.IUpdateable, IRenderable
    {
        public ushort HP;
        public Rectangle HealthBar;
        public bool Visible;

        public Health(ushort hp)
        {
            ushort HP = hp;
        }

        public override void OnAddedToEntity()
        {
            HealthBar = new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y - 2, Entity.Dimensions.X, 3);
        }

        public void Update()
        {
            Vector2 screenPos = Entity.ScreenPosition;
            HealthBar.X = (int)screenPos.X;
            HealthBar.Y = (int)screenPos.Y;
        }

        public void Draw(SpriteBatch sbatch)
        {
            if (Visible)
            {
                Vector2 start = new Vector2(HealthBar.X, HealthBar.Y);
                Vector2 end = new Vector2(HealthBar.Right, HealthBar.Top);
                Shapes.DrawLine(sbatch, Scene.DebugPixelTexture, start, end, 2, Color.Green);
            }
        }
    }
}
