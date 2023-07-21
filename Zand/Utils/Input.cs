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

        public static Vector2 MouseScreenPosition;

        public static InputContext Context = InputContext.UnitControl;

        public static void Update()
        {
            _prevKeyBoardState = _keyBoardState;
            _prevMouseState = _mouseState;

            _mouseState = Mouse.GetState();
            _keyBoardState = Keyboard.GetState();

            MouseScreenPosition.X = _mouseState.X;
            MouseScreenPosition.Y = _mouseState.Y;
        }

        public static bool MouseScrolledUp()
        {
            return _mouseState.ScrollWheelValue > _prevMouseState.ScrollWheelValue;
        }

        public static bool MouseScrolledDown()
        {
            return _mouseState.ScrollWheelValue < _prevMouseState.ScrollWheelValue;
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

        public static bool LeftMouseWasReleased()
        {
            return _prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightMouseWasReleased()
        {
            return _prevMouseState.RightButton == ButtonState.Pressed && _mouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftMouseIsPressed()
        {
            return _mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightShiftClickOccured()
        {
            return RightMouseWasReleased() && KeyIsDown(Keys.LeftShift);
        }

        public static bool ControlGroupDigitWasPressed()
        {
            return
                KeyWasReleased(Keys.D1) ||
                KeyWasReleased(Keys.D2) ||
                KeyWasReleased(Keys.D3) ||
                KeyWasReleased(Keys.D4) ||
                KeyWasReleased(Keys.D5) ||
                KeyWasReleased(Keys.D6) ||
                KeyWasReleased(Keys.D7) ||
                KeyWasReleased(Keys.D8) ||
                KeyWasReleased(Keys.D9);
        }

        public static byte GetControlGroupNumer()
        {
            byte value = 0;

            if (KeyWasReleased(Keys.D1))
            {
                value = System.Convert.ToByte(Keys.D1);
            }
            if (KeyWasReleased(Keys.D2))
            {
                value = System.Convert.ToByte(Keys.D2);
            }
            if (KeyWasReleased(Keys.D3))
            {
                value = System.Convert.ToByte(Keys.D3);
            }
            if (KeyWasReleased(Keys.D4))
            {
                value = System.Convert.ToByte(Keys.D4);
            }
            if (KeyWasReleased(Keys.D5))
            {
                value = System.Convert.ToByte(Keys.D5);
            }
            if (KeyWasReleased(Keys.D6))
            {
                value = System.Convert.ToByte(Keys.D6);
            }
            if (KeyWasReleased(Keys.D7))
            {
                value = System.Convert.ToByte(Keys.D7);
            }
            if (KeyWasReleased(Keys.D8))
            {
                value = System.Convert.ToByte(Keys.D8);
            }
            if (KeyWasReleased(Keys.D9))
            {
                value = System.Convert.ToByte(Keys.D9);
            }

            return (byte)(value - 48);
        }
    }

    public enum InputContext
    {
        UnitControl,
        UnitAbilities,
    }
}
