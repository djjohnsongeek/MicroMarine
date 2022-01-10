using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.ECS.Components;
using Zand.Physics;

namespace MicroMarine.Components
{
    class UnitSelector : SceneComponent
    {
        private List<Entity> _selectedUnits;
        private List<Entity> _units;

        public UnitSelector(Scene scene) : base(scene)
        {
            _units = new List<Entity>();
            _selectedUnits = new List<Entity>();
        }

        public override void Update()
        {
            if (Input.LeftMouseWasPressed())
            {
                for (int i = 0; i < _units.Count; i++)
                {
                    DeselectAll();
                    MouseSelector selector = _units[i].GetComponent<MouseSelector>();

                    if (Collisions.RectangleToPoint(selector.GetScreenLocation(), Input.MousePosition.ToPoint()))
                    {
                        selector.Selected = true;
                        _selectedUnits.Add(_units[i]);
                        _units[i].GetComponent<Health>().Visible = true;
                        break;
                    }
                }
            }
        }

        private void DeselectAll()
        {
            for (int i = 0; i < _selectedUnits.Count; i ++)
            {
                _selectedUnits[i].GetComponent<MouseSelector>().Selected = false;
                _selectedUnits[i].GetComponent<Health>().Visible = false;
            }

            _selectedUnits.Clear();
        }

        public void AddUnit(Entity entity)
        {
            _units.Add(entity);
        }
    }
}
