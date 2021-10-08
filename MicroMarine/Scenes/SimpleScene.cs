using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Scenes

{
    class SimpleScene : Scene
    {
        public override void Load()
        {
            base.Load();

            Entity ball = CreateEntity("ball", Vector2.Zero);
            var texture = Content.LoadTexture("ball", "Content/ball.png");
            var sprite = new SimpleSprite(texture, ball);
            ball.AddComponent(sprite);
        }
    }
}
