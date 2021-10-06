using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zand.ECS.Components;

namespace Zand
{
    class SimpleScene : Scene
    {
        public override void Load()
        {
            base.Load();

            Entity ball = CreateEntity("ball", Vector2.Zero);
            var texture = Content.LoadTexture("ball", "Content/ball.png");
            var sprite = new SpriteComponent(texture, Vector2.Zero);
            ball.AddComponent(sprite);
        }
    }
}
