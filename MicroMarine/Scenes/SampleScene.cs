using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using Zand.Assets;
using Microsoft.Xna.Framework.Graphics;
using Zand.UI;
using MicroMarine.Components.Units;
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

            // Audio
            Content.LoadSoundEffect("readyBark1", "Content/Audio/Barks/ready_bark1.wav");
            Content.LoadSoundEffect("readyBark2", "Content/Audio/Barks/ready_bark2.wav");
            Content.LoadSoundEffect("readyBark3", "Content/Audio/Barks/ready_bark3.wav");
            Content.LoadSoundEffect("readyBark4", "Content/Audio/Barks/ready_bark4.wav");
            Content.LoadSoundEffect("readyBark5", "Content/Audio/Barks/ready_bark5.wav");

            Content.LoadSoundEffect("ackBark1", "Content/Audio/Barks/ack_bark1.wav");
            Content.LoadSoundEffect("ackBark2", "Content/Audio/Barks/ack_bark2.wav");
            Content.LoadSoundEffect("ackBark3", "Content/Audio/Barks/ack_bark3.wav");
            Content.LoadSoundEffect("ackBark4", "Content/Audio/Barks/ack_bark4.wav");

            Content.LoadSoundEffect("deathBark1", "Content/Audio/Barks/death_bark1.wav");
            Content.LoadSoundEffect("deathBark2", "Content/Audio/Barks/death_bark2.wav");

            Content.LoadSoundEffect("marineAttack", "Content/Audio/FX/marine_fire.wav");

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

            var mouse = new MouseCursor(defaultCursor);
            mouse.AddCursor(attackCursor);
            mouse.AddCursor(attackMoveCursor);
            UI.AddElement(mouse);


            // Add Scene Components
            var unitSelector = (UnitSelector) SceneComponents.AddComponent(new UnitSelector(this, 1));
            SceneComponents.AddComponent(new UnitGroupManager(this));
            SceneComponents.AddComponent(new SoundEffectManager(this));

            // Initiate tile map
            Entity tileMapEntity = CreateEntity("tileMap", Vector2.Zero);
            Texture2D mapTexture = this.Content.LoadTexture("mapSheet", "Content/grassSheet32.png");
            var mapSpriteSheet = new SpriteSheet(mapTexture, 32, 32);
            var map = new TileMap(32, new Point(Config.MapWidth, Config.MapHeight), mapSpriteSheet);
            tileMapEntity.AddComponent(map);

            // Place Marines
            for (int i = 0; i < 2; i++)
            {
                Entity unit = CreateEntity("unit", RandomPosition(map.MapCenter.ToVector2(), 60));
                unit.AddComponent(new Marine(1));
                unitSelector.AddUnit(unit);
            }

            // Add Blant Spawner
            Entity blantSpawner = CreateEntity("unitSpawner", map.MapCenter.ToVector2());
            blantSpawner.AddComponent(new UnitSpawner<Blant>(map.MapCenter.ToVector2(), 1, 1, 1));

            // Center on Marines
            Camera.Position = map.MapCenter.ToVector2();

            // Units Barks
            UnitBarks barks = new UnitBarks(this);
            barks.AddBark(Content.GetContent<SoundEffect>("deathBark1").CreateInstance(), BarkType.Death);
            barks.AddBark(Content.GetContent<SoundEffect>("deathBark2").CreateInstance(), BarkType.Death);

            barks.AddBark(Content.GetContent<SoundEffect>("ackBark1").CreateInstance(), BarkType.ACK);
            barks.AddBark(Content.GetContent<SoundEffect>("ackBark2").CreateInstance(), BarkType.ACK);
            barks.AddBark(Content.GetContent<SoundEffect>("ackBark3").CreateInstance(), BarkType.ACK);
            barks.AddBark(Content.GetContent<SoundEffect>("ackBark4").CreateInstance(), BarkType.ACK);

            barks.AddBark(Content.GetContent<SoundEffect>("readyBark1").CreateInstance(), BarkType.Ready);
            barks.AddBark(Content.GetContent<SoundEffect>("readyBark2").CreateInstance(), BarkType.Ready);
            barks.AddBark(Content.GetContent<SoundEffect>("readyBark3").CreateInstance(), BarkType.Ready);
            barks.AddBark(Content.GetContent<SoundEffect>("readyBark4").CreateInstance(), BarkType.Ready);
            barks.AddBark(Content.GetContent<SoundEffect>("readyBark5").CreateInstance(), BarkType.Ready);

            SceneComponents.AddComponent(barks);

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
