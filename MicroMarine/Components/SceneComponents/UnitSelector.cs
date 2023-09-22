using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zand;
using Zand.Components;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    class UnitSelector : SceneComponent
    {
        private List<Entity> _units;
        private SelectBox _selectBox;
        private SoundEffectManager _sfxManager;
        private SelectedUnits _selectedUnits;
        private UnitGroupManager _unitManager;
        public ControlGroupManager ControlGroups;


        public List<Entity> SelectableUnits => _units;

        public UnitAllegiance Allegiance { get; private set; }

        public UnitSelector(Scene scene, int allegianceId) : base(scene)
        {
            _units = new List<Entity>();
            _selectBox = new SelectBox();
            Allegiance = new UnitAllegiance(allegianceId);
            _selectedUnits = scene.GetComponent<SelectedUnits>();
            _unitManager = scene.GetComponent<UnitGroupManager>();
            _sfxManager = scene.GetComponent<SoundEffectManager>();
            ControlGroups = new ControlGroupManager();

            if (_selectedUnits is null || _unitManager is null || _sfxManager is null)
            {
                throw new NullReferenceException("Required Scene Components 'SelectedUnits', 'UnitGroupManager', 'SoundFXManger' not found.'");
            }
        }



        public override void Update()
        {
            // Define Select Box
            if (Input.LeftMouseIsPressed() && Input.Context == InputContext.UnitControl)
            {
                if (!_selectBox.Active)
                {
                    _selectBox.SetOrigin(Input.MouseScreenPosition);
                }

                _selectBox.UpdateBounds();
            }

            // Clear and select units
            if (Input.LeftMouseWasReleased())
            {
                _selectedUnits.DeselectAll();
                SelectUnits();
                _selectBox.Clear();
                Input.Context = InputContext.UnitControl;
            }


            // Select All Hotkey
            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.Tab) && Input.Context == InputContext.UnitControl)
            {
                _selectedUnits.SelectAll(_units);
            }

            // Defining Control Groups
            if (Input.KeyIsDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && Input.ControlGroupDigitWasPressed())
            {
                ControlGroups.AddToGroup(Input.GetControlGroupNumer(), _selectedUnits.Units);
            }

            // Selecting Control Groups
            if (Input.ControlGroupDigitWasPressed() && !Input.KeyIsDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                SelectControlGroup(Input.GetControlGroupNumer());
            }

        }
        
        public bool SameTeam(Entity entity)
        {
            return entity.GetComponent<UnitAllegiance>().Id == Allegiance.Id;
        }

        public void AddUnit(Entity entity)
        {
            _units.Add(entity);
        }

        public void RemoveUnit(Entity entity)
        {
            _units.Remove(entity);
            _selectedUnits.RemoveUnit(entity);
        }

        public override void Draw(SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            if (_selectBox.Active)
            {
                //shapeBatch.DrawRectangle(_selectBox.Origin, _selectBox.Rect.Size.ToVector2(), Color.Transparent, Color.White);
                Shapes.DrawEmptyRect(spriteBatch, Scene.DebugPixelTexture, _selectBox.Rect, Color.White);
            }


        }

        private void SelectControlGroup(byte key)
        {
            _selectedUnits.DeselectAll();

            foreach (Entity e in ControlGroups.RetrieveGroup(key))
            {
                _selectedUnits.SelectUnit(e);
            }
        }

        private void SelectUnits()
        {
            if (_selectBox.IsTiny)
            {
                SelectTopUnit();
            }
            else
            {
                SelectBoxedUnits();
            }

            if (_selectedUnits.UnitsAreSelected)
            {
                _sfxManager.PlaySoundEffect("mReady", limitPlayback: true, randomChoice: true);
            }
        }

        private void SelectBoxedUnits()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                MouseSelectCollider selectCollider = _units[i].GetComponent<MouseSelectCollider>();
                if (_selectBox.Intersects(selectCollider.GetScreenLocation()) && SameTeam(selectCollider.Entity))
                {
                    _selectedUnits.SelectUnit(_units[i]);
                }
            }
        }

        private void SelectTopUnit()
        {
            Entity topUnit = null;
            for (int i = 0; i < _units.Count; i++)
            {
                MouseSelectCollider selectCollider = _units[i].GetComponent<MouseSelectCollider>();
                if (_selectBox.Intersects(selectCollider.GetScreenLocation()) && SameTeam(selectCollider.Entity))
                {
                    if (topUnit is null || _units[i].Position.Y > topUnit.Position.Y)
                    {
                        topUnit = _units[i];
                    }
                }

            }

            if (topUnit != null)
            {
                _selectedUnits.SelectUnit(topUnit);
            }
        }
    }
}