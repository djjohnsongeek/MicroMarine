using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    MouseSelectCollider selectCollider = _units[i].GetComponent<MouseSelectCollider>();
                    if (selectBox.Intersects(selectCollider.GetScreenLocation()))
                    {
                        SelectUnit(_units[i], selectCollider);
                    }
                }

                selectBox = Rectangle.Empty;
                SelectBoxOrigin = Vector2.Zero;
            }

            // Select All Hotkey
            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                SelectAll();
            }
        }

        private void DeselectAll()
        {
            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                // lets try and eliminate the need to do this
                _selectedUnits[i].GetComponent<MouseSelectCollider>().Selected = false;
                _selectedUnits[i].GetComponent<Health>().Visible = false;
            }

            _selectedUnits.Clear();
        }

        private void SelectAll()
        {
            DeselectAll();
            for (int i = 0; i < _units.Count; i++)
            {
                MouseSelectCollider selector = _units[i].GetComponent<MouseSelectCollider>();
                SelectUnit(_units[i], selector);
            }
        }

        private void SelectUnit(Entity entity, MouseSelectCollider selector)
        {
            selector.Selected = true;
            _selectedUnits.Add(entity);
            entity.GetComponent<Health>().Visible = true;
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
                Color.WhiteSmoke
            );
        }

        public List<Entity> GetSelectedUnits()
        {
            return _selectedUnits.GetRange(0, _selectedUnits.Count);
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