using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zand.UI
{
    public class MouseCursor : IRenderable
    {
        public Dictionary<CursorType, CursorData> Cursors;
        private bool _enabled;
        private CursorType CurrentCursor;

        bool IRenderable.Enabled => _enabled;

        public MouseCursor(CursorData defaultCursor)
        {
            _enabled = true;
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

        public void Draw(SpriteBatch spriteBatch)
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
        Follow,
        AttackMove,
    }

    public struct CursorData
    {
        public Vector2 OriginOffset;
        public Texture2D Texture;
        public CursorType Type;
    }
}
