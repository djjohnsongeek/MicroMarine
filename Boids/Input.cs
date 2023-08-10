using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boids
{
    static class Input
    {
        private static KeyboardState _previousState;
        private static KeyboardState _currentState;

        public static void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public static bool KeyWasPressed(Keys key)
        {
            return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }

    }
}
