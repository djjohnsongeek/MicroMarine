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
            if (Input.LeftMouseWasReleased() && Input.Context == InputContext.UnitControl)
            {
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
            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.Tab) && Input.Context == InputContext.UnitControl)
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

        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
            Active = true;
        }

        public void UpdateBounds()
        {
            Rect = new Rectangle(Origin.ToPoint(), GetBoxSize());

            // Adjust box's coordinates based on mouse position
            if (Input.MouseScreenPosition.X < Origin.X)
            {
                Rect.X -= (int)(Origin.X - Input.MouseScreenPosition.X);
            }

            if (Input.MouseScreenPosition.Y < Origin.Y)
            {
                Rect.Y -= (int)(Origin.Y - Input.MouseScreenPosition.Y);
            }
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

        private Point GetBoxSize()
        {
            return Calc.AbsVector2(Origin - Input.MouseScreenPosition).ToPoint();
        }
    }
}