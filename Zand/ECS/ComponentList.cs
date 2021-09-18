using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    class ComponentList
    {
        public Entity Entity;

        private List<Component> _componentsToAdd;
        private List<Component> _componentsToRemove;
        private List<IRenderable> _renderableComponents;
        private List<IUpdateable> _updatableComponents;

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
            foreach (var component in _updatableComponents)
            {
                component.Update();
            }
        }

        public void Draw()
        {
            foreach (var component in _renderableComponents)
            {
                component.Draw();
            }
        }

        private void HandleRemovals()
        {
            if (_componentsToRemove.Count > 0)
            {
                foreach (var component in _componentsToRemove)
                {
                    if (component is IUpdateable)
                    {
                        _updatableComponents.Remove(component as IUpdateable);
                    }

                    if (component is IRenderable)
                    {
                        _renderableComponents.Remove(component as IRenderable);
                    }

                    component.Entity = null;
                }

                _componentsToRemove.Clear();
            }
        }

        private void HandleAdditions()
        {
            if (_componentsToAdd.Count > 0)
            {
                foreach (var component in _componentsToAdd)
                {
                    component.Entity = Entity;

                    if (component is IUpdateable)
                    {
                        _updatableComponents.Add(component as IUpdateable);
                    }

                    if (component is IRenderable)
                    {
                        _renderableComponents.Add(component as IRenderable);
                    }
                }

                _componentsToAdd.Clear();
            }

            // will need to handle sorting at somepoint.
            // thing about an on added to entity method for components
        }

    }
}
