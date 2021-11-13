using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Debug;
using Zand.Utils;

namespace Zand
{
    public class Scene
    {
        private EntityList Entities;
        public ZandContentManager Content;
        public Camera Camera = null;
        internal DebugConsole DebugConsole;

        public int ScreenWidth;
        public int ScreenHeight;

        public Scene()
        {
            Entities = new EntityList(this);
            Content = new ZandContentManager(Core._instance.Services, Core._instance.Content.RootDirectory);
        }

        public virtual void Initialize()
        {
            
            // Init logic goes here
        }

        public virtual void Load()
        {
            // TODO: use this.Content to load your game content here
            SpriteFont debugFont = Content.LoadFont("DebugFont", "Debug");
            DebugConsole = new DebugConsole(this, debugFont);
        }

        // Create entity for this scene
        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name, position);
            DebugConsole.AddMessage("Created Entity");
            return AddEntity(entity);
        }

        public Entity AddEntity(Entity entity)
        {
            if (Entities.Contains(entity))
            {
                throw new ArgumentException("Attempt to add Entity to a scene in which it already exists.");
            }

            Entities.Add(entity);
            entity.Scene = this;

            return entity;
        }

        public virtual void Update()
        {
            Entities.Update();

            if (DebugConsole.Enabled)
            {
                DebugConsole.Update();
            }
        }

        public void Draw()
        {

            Entities.Draw();
            // Draw Effects
            // Draw UI
            if (DebugConsole.Enabled)
            {
                DebugConsole.Draw();
            }
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
