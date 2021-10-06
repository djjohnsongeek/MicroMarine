using System;
using Microsoft.Xna.Framework;
using Zand.Utils;

namespace Zand
{
    public class Scene
    {
        private EntityList Entities;
        public ZandContentManager Content;

        public Scene()
        {
            Entities = new EntityList(this);
            Content = new ZandContentManager(Core._instance.Services, Core._instance.Content.RootDirectory);
        }

        public virtual void Load()
        {
            // TODO: use this.Content to load your game content here

        }

        // Create entity for this scene
        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name, position);
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

        public void Update()
        {
            Entities.Update();
        }

        public void Draw()
        {

            Entities.Draw();
            // Draw Effects
            // Draw UI
;
        }
    }
}
