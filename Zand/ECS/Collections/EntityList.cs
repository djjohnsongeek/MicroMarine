using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zand
{
    class EntityList
    {
        private List<Entity> _entities;
        private List<Entity> _entitiesToAdd;
        private List<Entity> _entitiesToRemove;
        public Scene Scene;
        public int Count
        {
            get => _entities.Count;
        }

        internal EntityList(Scene scene)
        {
            Scene = scene;
            _entities = new List<Entity>();
            _entitiesToAdd = new List<Entity>();
            _entitiesToRemove = new List<Entity>();
        }

        public void Update()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Enabled)
                {
                    _entities[i].Update();
                }
            }

            UpdateEntityLists();
        }

        public void Add(Entity entity)
        {
            _entitiesToAdd.Add(entity);
        }

        public bool Contains(Entity entity)
        {
            return _entities.Contains(entity) || _entitiesToAdd.Contains(entity);
        }

        internal void Draw()
        {
            SpriteBatch spriteBatch = new SpriteBatch(Core._instance.GraphicsDevice);
            Matrix? matrix = Scene.Camera == null ? null : Scene.Camera.GetTransformation();
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                matrix
            );
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Enabled)
                {
                    _entities[i].Draw(spriteBatch);
                }

            }
            spriteBatch.End();
        }

        internal void UpdateEntityLists()
        {
            HandleRemovals();
            HandleAdditions();
        }

        private void HandleRemovals()
        {
            if (_entitiesToRemove.Count > 0)
            {
                for (int i = 0; i < _entitiesToRemove.Count; i++)
                {
                    _entities.Remove(_entitiesToRemove[i]);
                }
            }

            _entitiesToRemove.Clear();

            // call on entity removed

        }

        private void HandleAdditions()
        {
            if (_entitiesToAdd.Count > 0)
            {
                for (int i = 0; i < _entitiesToAdd.Count; i++)
                {
                    _entities.Add(_entitiesToAdd[i]);
                }
            }

            _entitiesToAdd.Clear();

            // call on entity added?
        }
    }
}
