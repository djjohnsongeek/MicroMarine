using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Debug;
using Zand.ECS.Collections;
using Zand.ECS.Components;
using Zand.Physics;
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
        }

        public virtual void Initialize()
        {
            Physics = new PhysicsManager();
            // Init logic goes here

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
        }

        public virtual void Draw()
        {
            // Draw UI


            // Game Objects/ Entities
            Entities.Draw();
            SceneComponents.Draw();

            if (DebugTools.Active)
            {
                DebugTools.Draw(SpriteBatch);
            }

            // Draw Effects
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
    }
}
