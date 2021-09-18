using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zand
{
    public static class Input
    {
        public static MouseState mState;
        public static KeyboardState kState;

        public static MouseState prevMState;
        public static KeyboardState prevKState;

        public static void Update()
        {
            prevKState = kState;
            prevMState = mState;

            mState = Mouse.GetState();
            kState = Keyboard.GetState();
        }

        public static bool KeyWasReleased(Keys key)
        {
            return prevKState.IsKeyDown(key) && kState.IsKeyUp(key);
        }

        public static bool KeyWasPressed(Keys key)
        {
            return prevKState.IsKeyUp(key) && kState.IsKeyDown(key);
        }

        public static bool KeyIsDown(Keys key)
        {
            return kState.IsKeyDown(key);
        }

        public static bool LeftMouseWasPressed()
        {
            return prevMState.LeftButton == ButtonState.Pressed && mState.LeftButton == ButtonState.Released;
        }

        public static bool RightMouseWasPressed()
        {
            return prevMState.RightButton == ButtonState.Pressed && mState.RightButton == ButtonState.Released;
        }

        public static bool LeftMouseIsPressed()
        {
            return mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightClick()
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
