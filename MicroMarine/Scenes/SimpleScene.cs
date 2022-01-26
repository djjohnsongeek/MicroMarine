using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;

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
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this));

            //Add Entities
            for (int y = 0; y < 512; y += 32)
            {
                for (int x = 0; x < 512; x += 32)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, y));
                    marine.AddComponent(new Marine());
                    unitSelector.AddUnit(marine);
                }
            }
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
