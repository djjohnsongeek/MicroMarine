using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Assets;
using Zand.Debug;
using Zand.ECS.Collections;
using Zand.ECS.Components;
using Zand.Graphics.Lighting;
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
        public LightMap Lighting;
        public RenderableComponentList RenderableComponents;


        public bool GameIsActive => Core._instance.IsActive;


        public SpriteBatch SpriteBatch;

        public int ScreenWidth;
        public int ScreenHeight;

        public Scene()
        {
            SetWindowSize(800, 400);
            Entities = new EntityList(this);
            Content = new ZandContentManager(Core._instance.Services, Core._instance.Content.RootDirectory);
            SpriteBatch = new SpriteBatch(Core._instance.GraphicsDevice);
            SceneComponents = new SceneComponentList(this);
            RenderableComponents = new RenderableComponentList(this);
            UI = new UserInterface(this);
            Rng = new Random();
        }

        public virtual void Initialize()
        {
            Physics = new PhysicsManager();
            Lighting = new LightMap(Core._instance.GraphicsDevice, ScreenWidth, ScreenHeight, Camera);
        }

        public virtual void Load()
        {
            DebugPixelTexture = new Texture2D(Core._instance.GraphicsDevice, 1, 1);
            DebugPixelTexture.SetData(new Color[] { Color.White});
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
            UI.Update();
            Lighting.Update();

            RenderableComponents.Update();
        }

        public virtual void Draw()
        {
            Lighting.RenderLightMap(SpriteBatch);

            // Clear Screen
            Core._instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Game Objects/ Entities
            RenderableComponents.Draw();

            // Draw lightmap
            Lighting.Draw(SpriteBatch);
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
