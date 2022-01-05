using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Debug;
using Zand.ECS.Components;
using Zand.Physics;
using Zand.Utils;

namespace Zand
{
    public class Scene
    {
        private EntityList Entities;
        public ZandContentManager Content;
        public Camera Camera = null;
        public Texture2D DebugPixelTexture;
        public bool Debug = false;
        public DebugTools DebugTools;

        public SpriteBatch SpriteBatch;

        public int ScreenWidth;
        public int ScreenHeight;

        public Scene()
        {
            Entities = new EntityList(this);
            Content = new ZandContentManager(Core._instance.Services, Core._instance.Content.RootDirectory);
            SpriteBatch = new SpriteBatch(Core._instance.GraphicsDevice);
        }

        public virtual void Initialize()
        {

            // Init logic goes here
            
        }

        public virtual void Load()
        {
            DebugPixelTexture = new Texture2D(Core._instance.GraphicsDevice, 1, 1);
            DebugPixelTexture.SetData(new Color[] { Color.White});
            SpriteFont debugFont = Content.LoadFont("DebugFont", "Debug");
            DebugTools = new DebugTools(this, debugFont);
        }

        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name, position, new Point(32, 32));
            DebugTools.Log("Created Entity");
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

            return entity;
        }

        public virtual void Update()
        {
            Entities.Update();
            DebugTools.Update();

            // Toggle Debug
            if (Input.KeyIsDown(Keys.LeftControl) && Input.KeyWasPressed(Keys.D))
            {
                Debug = !Debug;
            }
        }

        public void Draw()
        {
            Entities.Draw();
            // Draw Effects
            // Draw UI
            DebugTools.Draw();
        }

        public void RegisterCollider(Collider collider)
        {
            PhysicsManager.AddCollider(collider);
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
