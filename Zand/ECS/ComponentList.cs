﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    class ComponentList
    {
        public Entity Entity;

        private List<Component> _components;
        private List<Component> _componentsToAdd;
        private List<Component> _componentsToRemove;
        private List<IRenderable> _renderableComponents;
        private List<IUpdateable> _updatableComponents;
        internal int Count
        {
            get => _components.Count;
        }

        public void Add(Component component)
        {
            _componentsToAdd.Add(component);
        }

        public void Remove(Component component)
        {
            _componentsToRemove.Remove(component);
        }

        public void UpdateComponentLists()
        {
            HandleRemovals();
            HandleAdditions();

            // sort list if needed
        }

        public void Update()
        {
            for (int i = 0; i < _updatableComponents.Count; i ++)
            {
                _updatableComponents[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _renderableComponents.Count; i++)
            {
                _renderableComponents[i].Draw(spriteBatch);
            }
        }

        private void HandleRemovals()
        {
            if (_componentsToRemove.Count > 0)
            {
                for (int i = 0; i <  _componentsToRemove.Count; i ++)
                {
                    if (_componentsToRemove[i] is IUpdateable)
                    {
                        _updatableComponents.Remove(_componentsToRemove[i] as IUpdateable);
                    }

                    if (_componentsToRemove[i] is IRenderable)
                    {
                        _renderableComponents.Remove(_componentsToRemove[i] as IRenderable);
                    }

                    _componentsToRemove[i].Entity = null;
                }

                _componentsToRemove.Clear();
            }
        }

        private void HandleAdditions()
        {
            if (_componentsToAdd.Count > 0)
            {
                for (int i = 0; i < _componentsToAdd.Count; i++)
                {
                    _componentsToAdd[i].Entity = Entity;

                    if (_componentsToAdd[i] is IUpdateable)
                    {
                        _updatableComponents.Add(_componentsToAdd[i] as IUpdateable);
                    }

                    if (_componentsToAdd[i] is IRenderable)
                    {
                        _renderableComponents.Add(_componentsToAdd[i] as IRenderable);
                    }
                }

                _componentsToAdd.Clear();
            }

            // will need to handle sorting at somepoint.
            // thing about an on added to entity method for components
        }

    }
}
