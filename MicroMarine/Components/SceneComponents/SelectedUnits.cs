using System.Collections.Generic;
using Zand;
using Zand.Components;
using Zand.ECS.Components;
using System;

namespace MicroMarine.Components
{ 
    internal class SelectedUnits : SceneComponent
    {
        private List<Entity> _selectedUnits;

        public IReadOnlyList<Entity> Units => _selectedUnits;

        public SelectedUnits(Scene scene) : base(scene)
        {
            _selectedUnits = new List<Entity>();
        }

        public void DeselectAll()
        {
            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                _selectedUnits[i].GetComponent<MouseSelectCollider>().Selected = false;
            }
            _selectedUnits.Clear();
        }

        public void SelectAll(List<Entity> units)
        {
            DeselectAll();
            for (int i = 0; i < units.Count; i++)
            {
                SelectUnit(units[i]);
            }
        }

        public void RemoveUnit(Entity entity)
        {
            _selectedUnits.Remove(entity);
        }

        public void SelectUnit(Entity entity)
        {
            MouseSelectCollider selector = entity.GetComponent<MouseSelectCollider>();
            selector.Selected = true;
            _selectedUnits.Add(entity);
        }

        public List<Entity> Selected => _selectedUnits;
        public bool UnitsAreSelected => _selectedUnits.Count > 0;

        public bool SameTeam(Entity unit)
        {
            if (!UnitsAreSelected)
            {
                throw new IndexOutOfRangeException("Team cannot be determined when no units are selected");
            }

            var unitAllegiance = unit.GetComponent<UnitAllegiance>().Id;
            var selectedUnitsAllegiance = _selectedUnits[0].GetComponent<UnitAllegiance>().Id;
            return unitAllegiance == selectedUnitsAllegiance;
        }
    }
}
