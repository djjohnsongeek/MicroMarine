using MicroMarine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Zand;
using Zand.UI;
using System;

namespace MicroMarine.Ui
{
    public class MouseCursor : UIElement
    {
        public Dictionary<CursorType, CursorData> Cursors;
        private CursorType CurrentCursor;
        private readonly SelectedUnits _selectedUnits;
        private readonly UnitGroupManager _unitManager;


        public MouseCursor(Scene scene, CursorData defaultCursor)
        {
            _enabled = true;
            _scene = scene;
            _selectedUnits = _scene.GetComponent<SelectedUnits>();
            _unitManager = _scene.GetComponent<UnitGroupManager>();


            if (_selectedUnits is null)
            {
                throw new NullReferenceException("Mouse Cursor must be instantiated after SelectedUnits is added to the Scene.");
            }
            if (_unitManager is null)
            {
                throw new NullReferenceException("Mouse Cursor must be instantiated after UnitGroupManager is added to the Scene.");
            }


            Cursors = new Dictionary<CursorType, CursorData>();
            AddCursor(defaultCursor);
            SetCursor(defaultCursor.Type);
        }

        public void AddCursor(CursorData data)
        {
            Cursors.Add(data.Type, data);
        }

        public void SetCursor(CursorType type)
        {
            CurrentCursor = type;
        }

        public override void Update()
        {
            
            if (Input.Context == InputContext.UnitControl)
            {
                SetCursor(CursorType.Default);
            }
            else if (Input.Context == InputContext.UnitAbilities)
            {
                SetCursor(CursorType.Ability);
            }

            if (_selectedUnits.UnitsAreSelected)
            {
                var entity = _scene.Physics.GetEntityAtPosition("unit", _scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
                if (entity != null)
                {
                    if (!_selectedUnits.SameTeam(entity))
                    {
                        SetCursor(CursorType.Attack);
                    }
                    return;
                }

                if (Input.KeyWasReleased(Microsoft.Xna.Framework.Input.Keys.F) && Input.Context == InputContext.UnitControl)
                {
                    SetCursor(CursorType.Ability);
                }

                if (Input.KeyIsDown(Microsoft.Xna.Framework.Input.Keys.A) && Input.Context == InputContext.UnitControl)
                {
                    SetCursor(CursorType.AttackMove);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Cursors[CurrentCursor].Texture,
                Input.MouseScreenPosition,
                null,
                Color.White,
                0,
                Cursors[CurrentCursor].OriginOffset,
                Vector2.One,
                SpriteEffects.None,
                1);
        }
    }

    public enum CursorType
    {
        Default,
        Attack,
        AttackMove,
        Ability,
    }

    public struct CursorData
    {
        public Vector2 OriginOffset;
        public Texture2D Texture;
        public CursorType Type;
    }
}
