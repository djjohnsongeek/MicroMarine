﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.ECS.Components;
using Zand.Assets;

namespace MicroMarine.Scenes

{
    class SimpleScene : Scene
    {
        public enum MarineAnimation
        {
            IdleSouth,
        }

        public SimpleScene() : base()
        {
            
        }

        public override void Initialize()
        {
            SetWindowSize(1280, 720);
            Camera = new Camera(new Vector2(ScreenWidth / 2, ScreenHeight / 2), this, ScreenWidth, ScreenHeight);

            base.Initialize();
        }

        public override void Load()
        {
            base.Load();

            Entity marine = CreateEntity("marine", Vector2.Zero);
            Texture2D marineSheet = Content.LoadTexture("marineSheet", "Content/marineSheet32.png");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);
            var animator = new Animator();
            // Add this logic to a "marine" entity

            Rectangle[] frames = new Rectangle[]
            {
                spriteSheet[8],
                spriteSheet[9],
                spriteSheet[10],
                spriteSheet[11],
                spriteSheet[12],
                spriteSheet[13],
                spriteSheet[14],
                spriteSheet[15],
            };
            var idleSouthAnimation = new Animation(marineSheet, frames, 8);

            animator.AddAnimation(MarineAnimation.IdleSouth, idleSouthAnimation);
            animator.SetAnimation(MarineAnimation.IdleSouth);
            marine.AddComponent(animator);
            MouseSelector mouseCollider = new MouseSelector(new Rectangle(marine.Position.ToPoint(), new Point(32, 32)));
            marine.AddComponent(mouseCollider);
            RegisterCollider(mouseCollider);
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
