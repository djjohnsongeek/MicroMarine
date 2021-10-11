using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            Texture2D texture = Content.LoadTexture("ball", "Content/ball.png");
            var sprite = new SimpleSprite(texture, ball);
            ball.AddComponent(sprite);
        }
    }
}
