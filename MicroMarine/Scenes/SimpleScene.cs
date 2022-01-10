using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using System.Collections.Generic;
using Zand.ECS.Components;

namespace MicroMarine.Scenes

{
    class SimpleScene : Scene
    {
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

            // Add Scene Components
            var selector = (UnitSelector) AddSceneComponent(new UnitSelector(this));

            // Add Entities
            for (int i = 0; i < 320; i += 32)
            {
                Entity marine = CreateEntity("marine", new Vector2(i, 5));
                marine.AddComponent(new Marine());
                selector.AddUnit(marine);
            }
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
