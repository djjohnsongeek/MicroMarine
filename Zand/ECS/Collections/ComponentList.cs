using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Zand.ECS;

namespace Zand
{
    class ComponentList
    {
        public Entity Entity;

        private List<Component> _components;
        private List<Component> _componentsToAdd;
        private List<Component> _componentsToRemove;
        private List<IUpdateable> _updatableComponents;

        internal int Count
        {
            get => _components.Count;
        }

        internal ComponentList(Entity entity)
        {
            _components = new List<Component>();
            _componentsToAdd = new List<Component>();
            _componentsToRemove = new List<Component>();
            _updatableComponents = new List<IUpdateable>();
            Entity = entity;
        }

        public void Add(Component component)
        {
            _componentsToAdd.Add(component);
        }

        public void Remove(Component component)
        {
            _componentsToRemove.Add(component);
        }

        public void RemoveAll()
        {
            foreach (var component in _components)
            {
                if (component is RenderableComponent)
                {
                    Entity.Scene.RenderableComponents.Remove(component as RenderableComponent);
                }

                HandleRemoval(component);
            }
            _componentsToAdd.Clear();
            _componentsToRemove.Clear();
            _components.Clear();
            _updatableComponents.Clear();
            Entity = null;
        }

        private void HandleRemoval(Component component)
        {
            component.OnRemovedFromEntity();
        }

        public T GetComponent<T>(bool onlyInitialized = true) where T : Component
        {
            for (int i = 0; i < _components.Count; i ++)
            {
                if (_components[i] is T)
                {
                    return _components[i] as T;
                }
            }

            if (!onlyInitialized)
            {
                for(int i = 0; i < _componentsToAdd.Count; i++)
                {
                    if (_componentsToAdd[i] is T)
                    {
                        return _componentsToAdd[i] as T;
                    }
                }
            }

            return null;
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
                if(_updatableComponents[i].Enabled)
                {
                    _updatableComponents[i].Update();
                }

            }

            UpdateComponentLists();
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

                    if (_componentsToRemove[i] is RenderableComponent)
                    {
                        Entity.Scene.RenderableComponents.Remove(_componentsToRemove[i] as RenderableComponent);
                    }

                    _componentsToRemove[i].OnRemovedFromEntity();
                    _components.Remove(_componentsToRemove[i]);
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

                    if (_componentsToAdd[i] is RenderableComponent)
                    {
                        Entity.Scene.RenderableComponents.Add(_componentsToAdd[i] as RenderableComponent);
                    }

                    _components.Add(_componentsToAdd[i]);
                }

                _componentsToAdd.Clear();
            }

            // will need to handle sorting at somepoint.
            // thing about an on added to entity method for components
        }

    }
}
