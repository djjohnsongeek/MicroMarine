using Microsoft.Xna.Framework;
using Zand;
using MicroMarine.Components;
using Zand.Assets;
using Microsoft.Xna.Framework.Graphics;
using MicroMarine.Components.Units;
using Zand.Graphics.Lighting;
using MicroMarine.Ui;
using Zand.Components;

namespace MicroMarine.Scenes

{
    class SampleScene : Scene
    {
        public SampleScene() : base()
        {
            
        }

        public override void Initialize()
        {
            InitFromFile("settings.ini");
            SetMouseVisibility(false);
            Camera = new Camera(new Vector2(ScreenWidth / 2, ScreenHeight / 2), this, ScreenWidth, ScreenHeight);
            base.Initialize();
        }

        private void InitFromFile(string filename)
        {
            var settings = new FileSettings(filename);

            if (settings["DetectDeviceDisplayDimensions"] == 1)
            {
                SetWindowSize(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else
            {
                SetWindowSize(settings["Width"], settings["Height"]);
            }
            SetFullScreen(settings["Fullscreen"] == 1);
        }

        public override void Load()
        {
            base.Load();

            // Map
            var map = CreateEntity("sampleMap", Vector2.Zero, Point.Zero);
            TiledMap tiledMap = new TiledMap("Content/Maps/Simple.tmx");
            tiledMap.SetLayerRenderLayer("Background", 0, 0);
            tiledMap.SetLayerRenderLayer("Foreground", 0, .5f);
            tiledMap.SetLayerRenderLayer("Top", 5);
            map.AddComponent(tiledMap);

            // Textures...
            Content.LoadTexture("waypoint", "Content/waypoint.png");
            Content.LoadTexture("waypointAttack", "Content/waypoint_attack.png");
            Content.LoadTexture("mediumUnitShadow", "Content/medium_unit_shadow.png");
            Content.LoadTexture("smallUnitShadow", "Content/small_unit_shadow.png");
            Content.LoadTexture("smallUnitSelect", "Content/small_unit_select.png");
            Content.LoadTexture("marineSheet", "Content/marineSheet32_2.png");
            Content.LoadTexture("blantSheet", "Content/blant_sheet.png");
            Content.LoadTexture("scuttleSheet", "Content/scuttle_sheet.png");
            Content.LoadTexture("deadMarineSheet", "Content/dead_marine_sheet.png");
            Content.LoadTexture("deadBlant", "Content/blant_dead.png");
            Content.LoadTexture("dropShip", "Content/drop_ship.png");
            var lightTexture = Content.LoadTexture("light", "Content/light.png");
            var fireTexture = Content.LoadTexture("fire", "Content/fire_sheet.png");

            Content.LoadTexture("glowStick", "Content/glow_stick.png");
            Content.LoadTexture("tinyShadow", "Content/tiny_shadow.png");

            // Audio
            var sfxManager = new SoundEffectManager(this);

            sfxManager.AddSoundEffect("mReady", Content.LoadSoundEffect("readyBark1", "Content/Audio/Barks/ready_bark1.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mReady", Content.LoadSoundEffect("readyBark2", "Content/Audio/Barks/ready_bark2.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mReady", Content.LoadSoundEffect("readyBark3", "Content/Audio/Barks/ready_bark3.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mReady", Content.LoadSoundEffect("readyBark4", "Content/Audio/Barks/ready_bark4.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mReady", Content.LoadSoundEffect("readyBark5", "Content/Audio/Barks/ready_bark5.wav"), 1, .07f);

            sfxManager.AddSoundEffect("mAck", Content.LoadSoundEffect("ackBark1", "Content/Audio/Barks/ack_bark1.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mAck", Content.LoadSoundEffect("ackBark2", "Content/Audio/Barks/ack_bark2.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mAck", Content.LoadSoundEffect("ackBark3", "Content/Audio/Barks/ack_bark3.wav"), 1, .07f);
            sfxManager.AddSoundEffect("mAck", Content.LoadSoundEffect("ackBark4", "Content/Audio/Barks/ack_bark4.wav"), 1, .07f);

            sfxManager.AddSoundEffect("mDeath", Content.LoadSoundEffect("deathBark1", "Content/Audio/Barks/death_bark1.wav"), 1, .06f);
            sfxManager.AddSoundEffect("mDeath", Content.LoadSoundEffect("deathBark2", "Content/Audio/Barks/death_bark2.wav"), 1, .06f);

            sfxManager.AddSoundEffect("bDeath", Content.LoadSoundEffect("bDeathBark1", "Content/Audio/Barks/blant_death1.wav"), 1, .06f);
            sfxManager.AddSoundEffect("bDeath", Content.LoadSoundEffect("bDeathBark2", "Content/Audio/Barks/blant_death2.wav"), 1, .06f);
            sfxManager.AddSoundEffect("bDeath", Content.LoadSoundEffect("bDeathBark3", "Content/Audio/Barks/blant_death3.wav"), 1, .06f);
            sfxManager.AddSoundEffect("bDeath", Content.LoadSoundEffect("bDeathBark4", "Content/Audio/Barks/blant_death4.wav"), 1, .06f);
            sfxManager.AddSoundEffect("bDeath", Content.LoadSoundEffect("bDeathBark5", "Content/Audio/Barks/blant_death5.wav"), 1, .06f);

            sfxManager.AddSoundEffect("footstep", Content.LoadSoundEffect("footstep", "Content/Audio/FX/footstep0.wav"), 1, .015f);
            sfxManager.AddSoundEffect("footstep", Content.LoadSoundEffect("footstep", "Content/Audio/FX/footstep1.wav"), 1, .015f);
            sfxManager.AddSoundEffect("footstep", Content.LoadSoundEffect("footstep", "Content/Audio/FX/footstep2.wav"), 1, .015f);
            sfxManager.AddSoundEffect("footstep", Content.LoadSoundEffect("footstep", "Content/Audio/FX/footstep2.wav"), 1, .015f);

            sfxManager.AddSoundEffect("mShoot", Content.LoadSoundEffect("mFire", "Content/Audio/FX/marine_fire.wav"), 8, .04f);
            sfxManager.AddSoundEffect("error", Content.LoadSoundEffect("error", "Content/Audio/FX/error_1.wav"), 2, .5f);


            var music = Content.LoadSoundEffect("soundtrack", "Content/Audio/Music/tribute-to-mr-wick.wav").CreateInstance();
            //The following music was used for this media project:
            //Music: Tribute To Mr.Wick by Frank Schroeter
            //Free download: https://filmmusic.io/song/11087-tribute-to-mr-wick
            //License(CC BY 4.0): https://filmmusic.io/standard-license

            music.Volume = .03f;
            music.Play();


            // Add Scene Components
            SceneComponents.AddComponent(sfxManager);
            SceneComponents.AddComponent(new SelectedUnits(this));
            SceneComponents.AddComponent(new UnitGroupManager(this));
            var unitSelector = SceneComponents.AddComponent(new UnitSelector(this, 1)) as UnitSelector;


            var defaultCursorTexture = Content.LoadTexture("cursor", "Content/cursor.png");
            var attackTexture = Content.LoadTexture("attackCursor", "Content/cursor_attack.png");
            var attackMoveCursorTexture = Content.LoadTexture("attackMoveCursor", "Content/cursor_attack_move.png");
            var abilityCursorTexture = Content.LoadTexture("abilityCursor", "Content/cursor_ability.png");

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

            CursorData abilityCursor = new CursorData
            {
                OriginOffset = new Vector2(abilityCursorTexture.Width / 2, abilityCursorTexture.Height / 2),
                Texture = abilityCursorTexture,
                Type = CursorType.Ability,
            };

            var mouse = new MouseCursor(this, defaultCursor);
            mouse.AddCursor(attackCursor);
            mouse.AddCursor(attackMoveCursor);
            mouse.AddCursor(abilityCursor);
            UI.AddElement(mouse);



            //Fire

            var fireObject = tiledMap.GetObject("MapObjects", "small-fire");
            var fireSheet = new SpriteSheet(fireTexture, 49, 74);
            var fire = CreateEntity("fire", fireObject.Position, Point.Zero);
            fire.Origin = new Vector2(24, 74);
            var fireAnimation = new Animator();
            fireAnimation.AddAnimation("burn", new Animation(fireTexture, fireSheet.GetFrames(0, 18), 19, Animation.LoopMode.Loop));
            fireAnimation.Play("burn");
            fire.AddComponent(fireAnimation);
            fireAnimation.RenderLayer = 6;

            var light = new SimpleLight(fire, lightTexture, new Color(255, 230, 230), new Vector2(2, 2), flicker: false);
            Lighting.AddLight(light);


            // Place Marine
            var playerSpawn = tiledMap.GetObject("MapObjects", "player-spawn");
            Entity unit = CreateEntity("unit", playerSpawn.Position, new Point(32, 32));
            unit.AddComponent(new Marine(1));
            unitSelector.AddUnit(unit);
            Lighting.AddLight(new SimpleLight(unit, lightTexture, new Color(255, 255, 255, 255), new Vector2(.4f, .4f), new Vector2(0, -5f)));

            // Add Spawner

            var spawnerObject = tiledMap.GetObject("MapObjects", "enemySpawner");
            Entity enemySpawner = CreateEntity("unitSpawner", Vector2.Zero + new Vector2(-20, 60), Point.Zero);
            enemySpawner.AddComponent(
                new UnitSpawner<Scuttle>(spawnerObject.Position, totalSpawns: 10, unitPerWave: 5, waveDelay: 2, waveStep: 4)
            );

            // Center on Marines
            Camera.Position = tiledMap.Center;

            // DropShip
            var dropShipSpawn = tiledMap.GetObject("MapObjects", "first-dropship-spawn");
            var dropShip = CreateEntity("dropShip", dropShipSpawn.Position, new Point(86, 97));
            dropShip.AddComponent(new DropShip(600, 3000));

            //
            //var command = CreateEntity("marineCommand", Vector2.Zero);
            //command.AddComponent(new MarineCommand());

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
