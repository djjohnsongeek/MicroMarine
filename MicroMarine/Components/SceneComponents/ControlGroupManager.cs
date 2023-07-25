using System.Collections.Generic;
using Zand;

namespace MicroMarine.Components
{
    class ControlGroupManager
    {
        private Dictionary<byte, List<Entity>> _groups;

        public ControlGroupManager()
        {
            _groups = new Dictionary<byte, List<Entity>>();
        }

        public void AddToGroup(byte key, IReadOnlyList<Entity> entities)
        {
            // Clear other groups associated with this key
            if (_groups.ContainsKey(key) && _groups[key].Count > 0)
            {
                ClearGroup(key);
            }

            // Remove enties from their current group
            RemoveEntites(entities);

            // Add Entities to their new group
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].GetComponent<ControlGroup>().SetControlGroup(key);
                AddToGroup(key, entities[i]);
            }
        }

        public void ClearGroup(byte key)
        {
            if (_groups.ContainsKey(key))
            {
                for (int i = 0; i < _groups[key].Count; i++)
                {
                    _groups[key][i].GetComponent<ControlGroup>().SetControlGroup(0);
                }

                _groups[key].Clear();
            }
        }

        public IReadOnlyList<Entity> RetrieveGroup(byte key)
        {
            EnsureKeyHasValue(key);
            return _groups[key];
        }

        public void RemoveEntites(IReadOnlyList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                RemoveEntity(entities[i]);
            }
        }

        public void RemoveEntity(Entity e)
        {
            var controlGroup = e.GetComponent<ControlGroup>();
            EnsureKeyHasValue(controlGroup.Id);
            _groups[controlGroup.Id].Remove(e);
            controlGroup.SetControlGroup(0);
        }

        private void AddToGroup(byte key, Entity entity)
        {
            EnsureKeyHasValue(key);
            _groups[key].Add(entity);
        }

        private void EnsureKeyHasValue(byte key)
        {
            if (!_groups.ContainsKey(key))
            {
                _groups[key] = new List<Entity>();
            }
        }
    }
}
