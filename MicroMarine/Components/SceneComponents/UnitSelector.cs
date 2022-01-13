using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
            // Define Select Box
            if (Input.LeftMouseIsPressed())
            {
                if (SelectBoxOrigin == Vector2.Zero)
                {
                    SelectBoxOrigin = Input.MouseScreenPosition;
                }

                selectBox = new Rectangle(SelectBoxOrigin.ToPoint(), CalculateSelectBxSize());
                AdjustSelectBxPosition(ref selectBox);
            }

            // Clear Select Box and Select Units
            if (Input.LeftMouseWasPressed() && selectBox != Rectangle.Empty)
            {
                DeselectAll();
                for (int i = 0; i < _units.Count; i++)
                {
                    MouseSelector unitSelector = _units[i].GetComponent<MouseSelector>();
                    if (selectBox.Intersects(unitSelector.GetScreenLocation()))
                    {
                        unitSelector.Selected = true;
                        _selectedUnits.Add(_units[i]);
                        _units[i].GetComponent<Health>().Visible = true;
                    }
                }

                selectBox = Rectangle.Empty;
                SelectBoxOrigin = Vector2.Zero;
            }
            // Select Single Unit with click
            else if (Input.LeftMouseWasPressed())
            {
                for (int i = 0; i < _units.Count; i++)
                {
                    DeselectAll();
                    MouseSelector selector = _units[i].GetComponent<MouseSelector>();

                    if (Collisions.RectangleToPoint(selector.GetScreenLocation(), Input.MouseScreenPosition.ToPoint()))
                    {
                        selector.Selected = true;
                        _selectedUnits.Add(_units[i]);
                        _units[i].GetComponent<Health>().Visible = true;
                        break;
                    }
                }
            }

            // Select All Hotkey
            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                SelectAll();
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

        private void SelectAll()
        {
            DeselectAll();

            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].GetComponent<MouseSelector>().Selected = true;
                _units[i].GetComponent<Health>().Visible = true;
                _selectedUnits.Add(_units[i]);
            }
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
                Color.WhiteSmoke);
        }

        private Point CalculateSelectBxSize()
        {
            return AbsoluteVector((SelectBoxOrigin - Input.MouseScreenPosition)).ToPoint();
        }

        private Vector2 AbsoluteVector(Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        private void AdjustSelectBxPosition(ref Rectangle selectBox)
        {
            if (Input.MouseScreenPosition.X < SelectBoxOrigin.X)
            {
                selectBox.X -= (int)(SelectBoxOrigin.X - Input.MouseScreenPosition.X);
            }

            if (Input.MouseScreenPosition.Y < SelectBoxOrigin.Y)
            {
                selectBox.Y -= (int)(SelectBoxOrigin.Y - Input.MouseScreenPosition.Y);
            }
        }
    }
}