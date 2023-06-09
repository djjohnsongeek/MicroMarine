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

            // Textures...
            this.Content.LoadTexture("waypoint", "Content/waypoint.png");
            this.Content.LoadTexture("smallUnitShadow", "Content/small_unit_shadow.png");

            // Add Scene Components
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this, 5));
            SceneComponents.AddComponent(new UnitGroupManager(this));

            // Initiate tile map
            Entity tileMapEntity = CreateEntity("tileMap", Vector2.Zero);
            Texture2D mapTexture = this.Content.LoadTexture("mapSheet", "Content/grassSheet32.png");
            var mapSpriteSheet = new SpriteSheet(mapTexture, 32, 32);
            var map = new TileMap(32, new Point(Config.MapWidth, Config.MapHeight), mapSpriteSheet);
            tileMapEntity.AddComponent(map);

            // Add Marine Entities
            int marineRows = 4;
            int marineCols = 4;
            int spacing = 32;
            for (int y = 10; y < marineRows * spacing; y += spacing)
            {
                for (int x = 10; x < marineCols * spacing; x += spacing)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, y));
                    marine.AddComponent(new Marine(2));
                    unitSelector.AddUnit(marine);
                }
            }



            for (int y = 250; y < 400; y += spacing)
            {
                for (int x = 250; x < 400; x += spacing)
                {
                    Entity marine = CreateEntity("marine", new Vector2(x, y));
                    marine.AddComponent(new Marine(5));
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
