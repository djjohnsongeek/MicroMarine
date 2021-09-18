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


        public void UpdateEntityLists()
        {
            HandleRemovals();
            HandleAdditions();
        }

        public void Update()
        {
            foreach (var entity in _entities)
            {
                entity.Update();
            }
        }

        private void HandleRemovals()
        {
            if (_entitiesToRemove.Count > 0)
            {
                foreach (var entity in _entitiesToRemove)
                {
                    _entities.Remove(entity);
                }
            }

            _entitiesToRemove.Clear();

            // call on entity removed

        }

        private void HandleAdditions()
        {
            if (_entitiesToAdd.Count > 0)
            {
                foreach(var entity in _entitiesToAdd)
                {
                    _entities.Add(entity);
                }
            }

            _entitiesToAdd.Clear();

            // call on entity added?
        }
    }
}
