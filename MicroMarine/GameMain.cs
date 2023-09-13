using Zand;
using MicroMarine.Scenes;
using Microsoft.Xna.Framework;
using FlatRedBall;
using FlatRedBall.Math.Geometry;

namespace MicroMarine
{
    public partial class GameMain : Core
    {
        protected override void Initialize()
        {
            GeneratedInitializeEarly();
            CurrentScene = new SampleScene();
            FlatRedBallServices.InitializeFlatRedBall(this, graphics);

            ShapeManager.AddCircle().Radius = 32;

            GeneratedInitialize();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            FlatRedBallServices.Update(gameTime);

            FlatRedBall.Screens.ScreenManager.Activity();
            GeneratedUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            FlatRedBall.Camera.Main.BackgroundColor = Color.Transparent;
            GeneratedDrawEarly(gameTime);
            FlatRedBallServices.Draw();

            GeneratedDraw(gameTime);
        }


        partial void GeneratedInitializeEarly();
        partial void GeneratedInitialize();
        partial void GeneratedUpdate(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDrawEarly(Microsoft.Xna.Framework.GameTime gameTime);
        partial void GeneratedDraw(Microsoft.Xna.Framework.GameTime gameTime);
    }
}
