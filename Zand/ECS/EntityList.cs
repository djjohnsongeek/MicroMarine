using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                _entities[i].Update();
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
            SpriteBatch spriteBatch = new SpriteBatch(Core.GraphicsDevice);
            spriteBatch.Begin();
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Draw(spriteBatch);
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
