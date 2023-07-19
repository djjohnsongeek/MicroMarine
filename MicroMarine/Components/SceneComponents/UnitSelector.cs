using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zand;
using Zand.Components;
using Zand.ECS.Components;
using Zand.Graphics;
using Zand.UI;

namespace MicroMarine.Components
{
    class UnitSelector : SceneComponent
    {
        private List<Entity> _selectedUnits;
        private List<Entity> _units;
        private Rectangle selectBox;
        private Vector2 SelectBoxOrigin;
        private SoundEffectManager _sfxManager;


        public List<Entity> SelectableUnits => _units;

        public UnitAllegiance Allegiance { get; private set; }

        public UnitSelector(Scene scene, int allegianceId) : base(scene)
        {
            _units = new List<Entity>();
            _selectedUnits = new List<Entity>();
            selectBox = Rectangle.Empty;
            SelectBoxOrigin = Vector2.Zero;
            Allegiance = new UnitAllegiance(allegianceId);
            _sfxManager = scene.GetComponent<SoundEffectManager>();
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
            bool unitsSelected = false;
            if (Input.LeftMouseWasPressed() && selectBox != Rectangle.Empty)
            {
                DeselectAll();
                for (int i = 0; i < _units.Count; i++)
                {
                    MouseSelectCollider selectCollider = _units[i].GetComponent<MouseSelectCollider>();
                    // 
                    if (selectBox.Intersects(selectCollider.GetScreenLocation()) && SameTeam(selectCollider.Entity))
                    {
                        SelectUnit(_units[i], selectCollider);
                        unitsSelected = true;
                    }
                }

                selectBox = Rectangle.Empty;
                SelectBoxOrigin = Vector2.Zero;
            }

            if (unitsSelected)
            {
                _sfxManager.PlaySoundEffect("mReady", limitPlayback: true, randomChoice: true);
            }

            // Select All Hotkey
            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                SelectAll();
            }

            // Update Cursors
            var cursor = Scene.UI.GetElement<MouseCursor>();
            cursor.SetCursor(CursorType.Default);
            if (_selectedUnits.Count > 0)
            {
                var entity = Scene.Physics.GetEntityAtPosition("unit", Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
                if (entity != null)
                {
                    if (!SameTeam(entity))
                    {
                        cursor.SetCursor(CursorType.Attack);
                    }
                    return;
                }

                if (Input.KeyWasReleased(Microsoft.Xna.Framework.Input.Keys.F))
                {
                    cursor.SetCursor(CursorType.Ability);
                }

                if (Input.KeyIsDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    cursor.SetCursor(CursorType.AttackMove);
                }

            }
        }
        
        private bool SameTeam(Entity entity)
        {
            return entity.GetComponent<UnitAllegiance>().Id == Allegiance.Id;
        }

        private void DeselectAll()
        {
            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                // lets try and eliminate the need to do this
                _selectedUnits[i].GetComponent<MouseSelectCollider>().Selected = false;
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
            if (SameTeam(entity))
            {
                selector.Selected = true;
                _selectedUnits.Add(entity);
            }
        }

        public void AddUnit(Entity entity)
        {
            _units.Add(entity);
        }

        public void RemoveUnit(Entity entity)
        {
            _units.Remove(entity);
            _selectedUnits.Remove(entity);
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
            return _selectedUnits;
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