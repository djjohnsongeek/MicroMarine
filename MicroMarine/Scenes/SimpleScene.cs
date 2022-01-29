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

            int entityCount = 1;

            // Add Scene Components
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this));

            //Add Entities
                for (int x = 0; x < entityCount; x += 32)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, 0));
                    marine.AddComponent(new Marine());
                    unitSelector.AddUnit(marine);
                }
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
