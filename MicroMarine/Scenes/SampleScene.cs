using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using Zand.Assets;
using Microsoft.Xna.Framework.Graphics;
using Zand.UI;
using MicroMarine.Components.Units;
using System;
using Microsoft.Xna.Framework.Audio;

namespace MicroMarine.Scenes

{
    class SampleScene : Scene
    {
        public SampleScene() : base()
        {
            
        }

        public override void Initialize()
        {
            // SetWindowSize(2560, 1440);
            SetWindowSize(1280, 720);
            // SetWindowSize(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            SetFullScreen(false);
            SetMouseVisibility(false);
            Camera = new Camera(new Vector2(ScreenWidth / 2, ScreenHeight / 2), this, ScreenWidth, ScreenHeight);
            base.Initialize();
        }

        public override void Load()
        {
            base.Load();

            // Textures...
            Content.LoadTexture("waypoint", "Content/waypoint.png");
            Content.LoadTexture("waypointAttack", "Content/waypoint_attack.png");
            Content.LoadTexture("mediumUnitShadow", "Content/medium_unit_shadow.png");
            Content.LoadTexture("smallUnitShadow", "Content/small_unit_shadow.png");
            Content.LoadTexture("smallUnitSelect", "Content/small_unit_select.png");
            Content.LoadTexture("marineSheet", "Content/marineSheet32.png");
            Content.LoadTexture("blantSheet", "Content/blant_sheet.png");
            Content.LoadTexture("deadMarineSheet", "Content/dead_marine_sheet.png");
            Content.LoadTexture("deadBlant", "Content/blant_dead.png");

            Content.LoadSoundEffect("selectBark1", "Content/Audio/Barks/select_bark1.wav");
            Content.LoadSoundEffect("selectBark2", "Content/Audio/Barks/select_bark2.wav");
            Content.LoadSoundEffect("selectBark3", "Content/Audio/Barks/select_bark3.wav");
            Content.LoadSoundEffect("selectBark4", "Content/Audio/Barks/select_bark4.wav");
            Content.LoadSoundEffect("selectBark5", "Content/Audio/Barks/select_bark5.wav");


            Content.LoadSoundEffect("commandBark1", "Content/Audio/Barks/command_bark1.wav");
            Content.LoadSoundEffect("commandBark2", "Content/Audio/Barks/command_bark2.wav");
            Content.LoadSoundEffect("commandBark3", "Content/Audio/Barks/command_bark3.wav");
            Content.LoadSoundEffect("commandBark4", "Content/Audio/Barks/command_bark4.wav");

            Content.LoadSoundEffect("deathBark1", "Content/Audio/Barks/death_bark1.wav");
            Content.LoadSoundEffect("deathBark2", "Content/Audio/Barks/death_bark2.wav");

            var music = Content.LoadSoundEffect("soundtrack", "Content/Audio/Music/tribute-to-mr-wick.wav").CreateInstance();

            //The following music was used for this media project:
            //Music: Tribute To Mr.Wick by Frank Schroeter
            //Free download: https://filmmusic.io/song/11087-tribute-to-mr-wick
            //License(CC BY 4.0): https://filmmusic.io/standard-license


            music.Volume = .1f;
            music.Play();






            var defaultCursorTexture = Content.LoadTexture("cursor", "Content/cursor.png");
            var attackTexture = Content.LoadTexture("attackCursor", "Content/cursor_attack.png");
            var attackMoveCursorTexture = Content.LoadTexture("attackMoveCursor", "Content/cursor_attack_move.png");
            var followCursorTexture = Content.LoadTexture("followCursor", "Content/cursor_follow.png");

            // Setup Mouse Cursors
            CursorData defaultCursor = new CursorData
            {
                OriginOffset = Vector2.Zero,
                Texture = defaultCursorTexture,
                Type = CursorType.Default,
            };

            CursorData attackCursor = new CursorData
            {
                OriginOffset = new Vector2(attackTexture.Width / 2, attackTexture.Height / 2),
                Texture = attackTexture,
                Type = CursorType.Attack,
            };

            CursorData attackMoveCursor = new CursorData
            {
                OriginOffset = Vector2.Zero,
                Texture = attackMoveCursorTexture,
                Type = CursorType.AttackMove,
            };

            CursorData followCursor = new CursorData
            {
                OriginOffset = new Vector2(attackTexture.Width / 2, attackTexture.Height / 2),
                Texture = followCursorTexture,
                Type = CursorType.Follow,
            };

            var mouse = new MouseCursor(defaultCursor);
            mouse.AddCursor(attackCursor);
            mouse.AddCursor(followCursor);
            mouse.AddCursor(attackMoveCursor);
            UI.AddElement(mouse);


            // Add Scene Components
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this, 1));
            SceneComponents.AddComponent(new UnitGroupManager(this));

            // Initiate tile map
            Entity tileMapEntity = CreateEntity("tileMap", Vector2.Zero);
            Texture2D mapTexture = this.Content.LoadTexture("mapSheet", "Content/grassSheet32.png");
            var mapSpriteSheet = new SpriteSheet(mapTexture, 32, 32);
            var map = new TileMap(32, new Point(Config.MapWidth, Config.MapHeight), mapSpriteSheet);
            tileMapEntity.AddComponent(map);

            // Place Marines
            for (int i = 0; i < 15; i++)
            {
                Entity unit = CreateEntity("unit", RandomPosition(map.MapCenter.ToVector2(), 60));
                unit.AddComponent(new Marine(1));
                unitSelector.AddUnit(unit);
            }

            // Add Blant Spawner
            Entity blantSpawner = CreateEntity("unitSpawner", map.MapCenter.ToVector2());
            blantSpawner.AddComponent(new UnitSpawner<Blant>(map.MapCenter.ToVector2(), 2, 5));

            // Center on Marines
            Camera.Position = map.MapCenter.ToVector2();

        }

        public override void Update()
        {
            Camera.Update();
            base.Update();
        }

        public Vector2 RandomPosition(Vector2 origin, int maxVariation)
        {
            var x = Rng.Next((int)origin.X, (int)origin.X + maxVariation);
            int y = Rng.Next((int)origin.Y, (int)origin.Y + maxVariation);
            return new Vector2(x, y);
        }
    }
}
