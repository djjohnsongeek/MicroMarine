using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Assets;
using Zand.Debug;
using Zand.ECS.Collections;
using Zand.ECS.Components;
using Zand.Physics;
using Zand.UI;
using Zand.Utils;

namespace Zand
{
    public class Scene
    {
        private int _idPool = 0;
        public EntityList Entities;
        public SceneComponentList SceneComponents;
        public ZandContentManager Content;
        public Camera Camera = null;
        public Texture2D DebugPixelTexture;
        public PhysicsManager Physics;
        public UserInterface UI;
        public Random Rng;

        // TEST

        private RenderTarget2D _lightMap;
        private Texture2D _light;
        private BlendState _lightMapBlendState;

        public bool GameIsActive => Core._instance.IsActive;


        public SpriteBatch SpriteBatch;

        public int ScreenWidth;
        public int ScreenHeight;

        public Scene()
        {
            Entities = new EntityList(this);
            Content = new ZandContentManager(Core._instance.Services, Core._instance.Content.RootDirectory);
            SpriteBatch = new SpriteBatch(Core._instance.GraphicsDevice);
            SceneComponents = new SceneComponentList(this);
            UI = new UserInterface();
            Rng = new Random();
        }

        public virtual void Initialize()
        {
            Physics = new PhysicsManager();

            _lightMap = new RenderTarget2D(Core._instance.GraphicsDevice, ScreenWidth, ScreenHeight);
            _lightMapBlendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };
            // Init logic goes here

        }

        public virtual void Load()
        {
            DebugPixelTexture = new Texture2D(Core._instance.GraphicsDevice, 1, 1);
            DebugPixelTexture.SetData(new Color[] { Color.White});
            _light = Content.LoadTexture("light", "Content/light.png");

            SpriteFont debugFont = Content.LoadFont("DebugFont", "Debug");
            DebugTools.SetUp(this, debugFont);
        }

        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name, position, new Point(32, 32));
            return AddEntity(entity);
        }

        public Entity AddEntity(Entity entity)
        {
            if (Entities.Contains(entity))
            {
                throw new ArgumentException("Attempted to add Entity to a scene in which it already exists.");
            }

            Entities.Add(entity);
            entity.Scene = this;
            entity.Id = _idPool;
            _idPool++;

            return entity;
        }

        public Entity FindEntity(string name)
        {
            return Entities.FindEntity(name);
        }

        public virtual void Update()
        {
            // Toggle Debug
            if (Input.KeyIsDown(Keys.LeftControl) && Input.KeyWasPressed(Keys.D))
            {
                DebugTools.Active = !DebugTools.Active;
            }
            Physics.Update();

            SceneComponents.Update();

            Entities.Update();

        }

        public virtual void Draw()
        {
            Core._instance.GraphicsDevice.SetRenderTarget(_lightMap);
            Core._instance.GraphicsDevice.Clear(new Color(0, 0, 0, 255));

            SpriteBatch.Begin(blendState: _lightMapBlendState, transformMatrix: Camera.GetTransformation());
            Vector2 lightOrigin = new Vector2(_light.Width / 2, _light.Height / 2);
            foreach (var entity in Entities.FindEntities("unit"))
            {
                SpriteBatch.Draw(_light, entity.Position, null, Color.White, 0, origin: lightOrigin, Vector2.One, SpriteEffects.None, 0);
            }
            


            //SpriteBatch.Draw(_light, Entities.FindEntity("tileMap").GetComponent<TileMap>().MapCenter.ToVector2(), Color.White);
            SpriteBatch.End();

            Core._instance.GraphicsDevice.SetRenderTarget(null);




            // Clear Screen
            Core._instance.GraphicsDevice.Clear(Color.CornflowerBlue);


            // Game Objects/ Entities
            Entities.Draw();

            // Draw lightmap
            SpriteBatch.Begin(); // transformMatrix: Camera.GetTransformation()
            SpriteBatch.Draw(_lightMap, Vector2.Zero, Color.White);
            SpriteBatch.End();


            SceneComponents.Draw();



            // Draw Effects
            // Draw UI

            UI.Draw(SpriteBatch);

            if (DebugTools.Active)
            {
                DebugTools.Draw(SpriteBatch);
            }
        }

        public T GetComponent<T>() where T : SceneComponent
        {
            return SceneComponents.GetSceneComponent<T>();
        }

        public void RegisterCollider(ICollider collider)
        {
            Physics.AddCollider(collider);
        }

        public void SetWindowSize(int width, int height)
        {
            Core.GraphicsManager.PreferredBackBufferHeight = height;
            ScreenHeight = height;

            Core.GraphicsManager.PreferredBackBufferWidth = width;
            ScreenWidth = width;

            Core.GraphicsManager.ApplyChanges();
        }

        public void SetFullScreen(bool value)
        {
            Core.GraphicsManager.IsFullScreen = value;
            Core.GraphicsManager.ApplyChanges();
        }

        public void SetMouseVisibility(bool value)
        {
            Core._instance.IsMouseVisible = value;
        }
    }
}
