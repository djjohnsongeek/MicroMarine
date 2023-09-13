using Zand;
using MicroMarine.Scenes;
using Microsoft.Xna.Framework;

namespace MicroMarine
{
    public partial class GameMain : Core
    {
        protected override void Initialize()
        {
            GeneratedInitializeEarly();
            CurrentScene = new SampleScene();
            GeneratedInitialize();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            GeneratedUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GeneratedDrawEarly(gameTime);
            GeneratedDraw(gameTime);
            base.Draw(gameTime);
        }


        partial void GeneratedInitializeEarly();
        partial void GeneratedInitialize();
        partial void GeneratedUpdate(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDrawEarly(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDraw(Microsoft.Xna.Framework.GameTime gameTime);
    }
}
