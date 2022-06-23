using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using Zand.Assets;
using Microsoft.Xna.Framework.Graphics;

namespace MicroMarine.Scenes

{
    class SampleScene : Scene
    {
        public SampleScene() : base()
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
            SceneComponents.AddComponent(new UnitGroupManager(this));

            // Add Marine Entities
            int marineRows = 1;
            int marineCols = 1;
            int spacing = 32;
            for (int y = 10; y < marineRows * spacing; y += spacing)
            {
                for (int x = 10; x < marineCols * spacing; x += spacing)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, y));
                    marine.AddComponent(new Marine());
                    unitSelector.AddUnit(marine);
                }
            }

            // initiate tile map
            Entity tileMapEntity = CreateEntity("tileMap", Vector2.Zero);
            Texture2D mapTexture = this.Content.LoadTexture("mapSheet", "Content/grassSheet32.png");
            var mapSpriteSheet = new SpriteSheet(mapTexture, 32, 32);
            var map = new TileMap(32, new Point(Config.MapWidth, Config.MapHeight), mapSpriteSheet);
            tileMapEntity.AddComponent(map);
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
