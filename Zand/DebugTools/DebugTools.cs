using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;
using Zand.Physics;

namespace Zand.Debug
{
    public class DebugTools : SceneComponent
    {
        private DebugConsole _debugConsole;
        private Scene _scene;

        public DebugTools (Scene scene, SpriteFont font) : base (scene)
        {
            _debugConsole = new DebugConsole(scene, font);
            _scene = scene;
        }

        public override void Update()
        {
            if (_scene.ShowDebug)
            {
                _debugConsole.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_scene.ShowDebug)
            {
                _debugConsole.Draw(spriteBatch);
                PhysicsManager.Draw(spriteBatch);
            }
        }

        public void Log(string message)
        {
            _debugConsole.AddMessage(message);
        }
    }
}
