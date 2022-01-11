using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using Zand.Graphics;
using Zand.Physics;

namespace MicroMarine.Components
{
    class UnitSelector : SceneComponent
    {
        private List<Entity> _selectedUnits;
        private List<Entity> _units;

        private Rectangle selectBox;
        private Vector2 SelectBoxOrigin;

        public UnitSelector(Scene scene) : base(scene)
        {
            _units = new List<Entity>();
            _selectedUnits = new List<Entity>();
            selectBox = Rectangle.Empty;
            SelectBoxOrigin = Vector2.Zero;
        }

        public override void Update()
        {
            if (Input.LeftMouseIsPressed())
            {
                if (SelectBoxOrigin == Vector2.Zero)
                {
                    SelectBoxOrigin = Input.MousePosition;
                }

                selectBox = new Rectangle(SelectBoxOrigin.ToPoint(), (SelectBoxOrigin - Input.MousePosition).ToPoint());
            }


            if (Input.LeftMouseWasPressed())
            {
                selectBox = Rectangle.Empty;
                SelectBoxOrigin = Vector2.Zero;

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Shapes.DrawEmptyRect(
                spriteBatch,
                Scene.DebugPixelTexture,
                selectBox,
                Color.Green);
        }
    }
}
