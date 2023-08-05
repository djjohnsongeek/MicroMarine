using Microsoft.Xna.Framework.Graphics;
using Apos.Shapes;

namespace Zand.Debug
{
    public static class DebugTools
    {
        private static DebugConsole _debugConsole;
        private static Scene _scene;
        public static bool Active = false;
        private static ShapeBatch _shapeBatch;

        public static void SetUp(Scene scene, SpriteFont font)
        {
            _debugConsole = new DebugConsole(scene, font);
            _scene = scene;
        }

        public static void Update()
        {
            if (Active)
            {
                _debugConsole.Update();
            }
        }

        public static void Draw(ShapeBatch shapeBatch)
        {
            shapeBatch.Begin(view: _scene.Camera.GetTransformation());

            if (Active)
            {
                //_debugConsole.Draw(spriteBatch);
                _scene.Physics.Draw(shapeBatch);
            }

            shapeBatch.End();
        }

        public static void Log(string message)
        {
            _debugConsole.AddMessage(message);
        }
    }
}
