using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Physics;

namespace Zand.Debug
{
    public class DebugTools
    {
        private DebugConsole _debugConsole;
        private Scene _scene;

        public DebugTools (Scene scene, SpriteFont font)
        {
            _debugConsole = new DebugConsole(scene, font);
            _scene = scene;
        }

        public void Update()
        {
            if (_scene.Debug)
            {
                _debugConsole.Update();
            }
        }

        public void Draw()
        {
            if (_scene.Debug)
            {
                _scene.SpriteBatch.Begin();
                _debugConsole.Draw(_scene.SpriteBatch);
                PhysicsManager.Draw(_scene.SpriteBatch);
                _scene.SpriteBatch.End();
            }
        }

        public void Log(string message)
        {
            _debugConsole.AddMessage(message);
        }
    }
}
