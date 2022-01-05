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

            Entity marine = CreateEntity("marine", Vector2.Zero);
            marine.AddComponent(new Marine());
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
