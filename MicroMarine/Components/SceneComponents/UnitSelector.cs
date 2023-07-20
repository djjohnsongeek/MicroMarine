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
        private SelectBox _selectBox;
        private SoundEffectManager _sfxManager;
        private SelectedUnits _selectedUnits;
        private UnitGroupManager _unitManager;

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
                if (!_selectBox.Active)
                {
                    _selectBox.Origin = Input.MouseScreenPosition;
                }

                _selectBox.SetRect();
            }

            // Clear and select units
            if (Input.LeftMouseWasReleased())
            {
                //if (_unitManager.AbilityPrimed)
                //{
                //    _selectedUnits.DeselectAll();
                //}

                _selectedUnits.DeselectAll();
                SelectBoxedUnits();
                _selectBox.Clear();
            }

            // this... probably shoudl not be here ...
            if (_selectedUnits.UnitsAreSelected)
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
            if (_selectBox.Active)
            {
                Shapes.DrawEmptyRect(
                    spriteBatch,
                    Scene.DebugPixelTexture,
                    _selectBox.Rect,
                    Color.WhiteSmoke
                );
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
    }

    public class SelectBox
    {
        public Vector2 Origin;
        public Rectangle Rect;
        public bool Active { get; private set; }

        public SelectBox()
        {
            Origin = Vector2.Zero;
            Rect = Rectangle.Empty;
            Active = false;
        }

        public void SetRect()
        {
            Rect = new Rectangle(Origin.ToPoint(), CalculateSelectBxSize());
            AdjustSelectBxPosition();
            Active = true;
        }

        public void Clear()
        {
            Rect = Rectangle.Empty;
            Origin = Vector2.Zero;
            Active = false;
        }

        public bool Intersects(Rectangle rect)
        {
            return Rect.Intersects(rect);
        }

        private Point CalculateSelectBxSize()
        {
            return AbsoluteVector(Origin - Input.MouseScreenPosition).ToPoint();
        }

        private Vector2 AbsoluteVector(Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        private void AdjustSelectBxPosition()
        {
            if (Input.MouseScreenPosition.X < Origin.X)
            {
                Rect.X -= (int)(Origin.X - Input.MouseScreenPosition.X);
            }

            if (Input.MouseScreenPosition.Y < Origin.Y)
            {
                Rect.Y -= (int)(Origin.Y - Input.MouseScreenPosition.Y);
            }
        }
    }
}