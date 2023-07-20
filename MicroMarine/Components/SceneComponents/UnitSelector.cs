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
        private List<Entity> _units;
        private Rectangle selectBox;
        private Vector2 SelectBoxOrigin;
        private SoundEffectManager _sfxManager;
        private SelectedUnits _selectedUnits;
        private UnitGroupManager _unitManager;

        public List<Entity> SelectableUnits => _units;

        public UnitAllegiance Allegiance { get; private set; }

        public UnitSelector(Scene scene, int allegianceId) : base(scene)
        {
            _units = new List<Entity>();
            selectBox = Rectangle.Empty;
            SelectBoxOrigin = Vector2.Zero;
            Allegiance = new UnitAllegiance(allegianceId);
            _selectedUnits = scene.GetComponent<SelectedUnits>();
            _unitManager = scene.GetComponent<UnitGroupManager>();
            _sfxManager = scene.GetComponent<SoundEffectManager>();

            if (_selectedUnits is null || _unitManager is null || _sfxManager is null)
            {
                throw new NullReferenceException("Required Scene Components 'SelectedUnits', 'UnitGroupManager', 'SoundFXManger' now found.'");
            }
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
                //if (!_unitManager.AbilityPrimed)
                //{
                //    _selectedUnits.DeselectAll();
                //}

                for (int i = 0; i < _units.Count; i++)
                {
                    MouseSelectCollider selectCollider = _units[i].GetComponent<MouseSelectCollider>();
                    if (selectBox.Intersects(selectCollider.GetScreenLocation()) && SameTeam(selectCollider.Entity))
                    {
                        _selectedUnits.SelectUnit(_units[i]);
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
                _selectedUnits.SelectAll(_units);
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Shapes.DrawEmptyRect(
                spriteBatch,
                Scene.DebugPixelTexture,
                selectBox,
                Color.WhiteSmoke
            );
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