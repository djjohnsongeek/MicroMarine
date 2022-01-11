using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
