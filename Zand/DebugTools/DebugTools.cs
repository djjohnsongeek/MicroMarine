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
            _debugConsole.Update();
        }

        public void Draw()
        {
            SpriteBatch spriteBatch = new SpriteBatch(Core._instance.GraphicsDevice);
            spriteBatch.Begin();

            _debugConsole.Draw(spriteBatch);
            PhysicsManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Log(string message)
        {
            _debugConsole.AddMessage(message);
        }
    }
}
