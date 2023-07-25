using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Components;

namespace MicroMarine.Components.Units
{
    class SelectionIndicator : Decale
    {
        public SelectionIndicator(Texture2D texture, Vector2 entityOffset) : base(texture, entityOffset)
        {
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
