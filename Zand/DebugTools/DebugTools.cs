using Microsoft.Xna.Framework.Graphics;


namespace Zand.Debug
{
    public static class DebugTools
    {
        private static DebugConsole _debugConsole;
        private static Scene _scene;
        public static bool ShowDebug = false;

        public static void SetUp(Scene scene, SpriteFont font)
        {
            _debugConsole = new DebugConsole(scene, font);
            _scene = scene;
        }

        public static void Update()
        {
            if (ShowDebug)
            {
                _debugConsole.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null, null
            );

            if (ShowDebug)
            {
                _debugConsole.Draw(spriteBatch);
                _scene.Physics.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public static void Log(string message)
        {
            _debugConsole.AddMessage(message);
        }
    }
}
