using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zand
{
    public static class Input
    {
        private static MouseState _mouseState;
        private static KeyboardState _keyBoardState;

        private static MouseState _prevMouseState;
        private static KeyboardState _prevKeyBoardState;

        public static Vector2 MousePosition;

        public static void Update()
        {
            _prevKeyBoardState = _keyBoardState;
            _prevMouseState = _mouseState;

            _mouseState = Mouse.GetState();
            _keyBoardState = Keyboard.GetState();

            MousePosition.X = _mouseState.X;
            MousePosition.Y = _mouseState.Y;
        }

        public static bool KeyWasReleased(Keys key)
        {
            return _prevKeyBoardState.IsKeyDown(key) && _keyBoardState.IsKeyUp(key);
        }

        public static bool KeyWasPressed(Keys key)
        {
            return _prevKeyBoardState.IsKeyUp(key) && _keyBoardState.IsKeyDown(key);
        }

        public static bool KeyIsDown(Keys key)
        {
            return _keyBoardState.IsKeyDown(key);
        }

        public static bool LeftMouseWasPressed()
        {
            return _prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightMouseWasPressed()
        {
            return _prevMouseState.RightButton == ButtonState.Pressed && _mouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftMouseIsPressed()
        {
            return _mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightClickOccured()
        {
            return RightMouseWasPressed() && !KeyIsDown(Keys.LeftShift);
        }

        //public static Vector2 GetMouseWorldPos()
        //{
        //    return Camera.GetWorldLocation(new Vector2(mState.X, mState.Y));
        //}

        //public static bool MouseCollides(Entity entity)
        //{
        //    Vector2 unitPosition = unit.GetPosition();
        //    int unitHeight = unit.GetHeight();
        //    int unitWidth = unit.GetWidth();

        //    Vector2 mousePos = GetMouseWorldPos();

        //    return (mousePos.X >= (unitPosition.X - unitWidth / 2)) &&
        //           (mousePos.X <= (unitPosition.X + unitWidth / 2)) &&
        //           (mousePos.Y >= (unitPosition.Y - unitHeight / 2)) &&
        //           (mousePos.Y <= (unitPosition.Y + unitHeight / 2));
        //}
    }
}
