using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Components;

namespace MicroMarine.Components.Units
{
    class SelectionIndicator : UnitShadow
    {
        public SelectionIndicator(Texture2D texture, Vector2 entityOffset) : base(texture, entityOffset)
        {
            _layer = 0.0000002f;
        }

        public override void Draw(SpriteBatch sbatch)
        {
            if (Entity.GetComponent<MouseSelectCollider>().Selected)
            {
                base.Draw(sbatch);
            }
        }
    }
}
