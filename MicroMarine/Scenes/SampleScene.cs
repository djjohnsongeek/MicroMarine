using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using Zand.ECS.Components.EntityComponents;
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

            int marineRows = 20;
            int marineCols = 20;
            int spacing = 32;

            // Add Scene Components
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this));
            SceneComponents.AddComponent(new UnitGroupManager(this));

            //Add Entities
            for (int y = 10; y < marineRows * spacing; y += spacing)
            {
                for (int x = 10; x < marineCols * spacing; x += spacing)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, y));
                    marine.AddComponent(new Marine());
                    unitSelector.AddUnit(marine);
                }
            }

            Entity tileMapEntity = CreateEntity("tileMap", Vector2.Zero);
            Texture2D mapTexture = this.Content.LoadTexture("mapSheet", "Content/grassSheet32.png");
            var mapSpriteSheet = new SpriteSheet(mapTexture, 32, 32);
            var map = new TileMap(32, new Point(60, 60), mapSpriteSheet);
            map.GenerateMap();
            tileMapEntity.AddComponent(new TileMapRenderer(map));
        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }
    }
}
