using Apos.Shapes;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class SceneComponent
    {
        public Scene Scene;

        public SceneComponent(Scene scene)
        {
            Scene = scene;
        }

        public virtual void Update()
        {

        }

        public virtual void OnAddedToScene()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
        }
    }
}
