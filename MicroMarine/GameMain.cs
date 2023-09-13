using Zand;
using MicroMarine.Scenes;

namespace MicroMarine
{
    public partial class GameMain : Core
    {
        protected override void Initialize()
        {
            CurrentScene = new SampleScene();
            base.Initialize();
        }

        partial void GeneratedInitializeEarly();
        partial void GeneratedInitialize();
        partial void GeneratedUpdate(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDrawEarly(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDraw(Microsoft.Xna.Framework.GameTime gameTime);
    }
}
