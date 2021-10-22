using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Utils;

namespace Zand
{
    public class Core : Game
    {
        public static GraphicsDeviceManager GraphicsManager;
        //public static new GraphicsDevice GraphicsDevice;
        public static ZandContentManager GlobalContent;
        public Scene CurrentScene;

        internal static Core _instance;

        public Core()
        {
            GraphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _instance = this;
            GlobalContent = new ZandContentManager(Services, Content.RootDirectory);
        }

        #region Game Loop Overrides

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CurrentScene.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime.ElapsedGameTime.TotalSeconds);
            Input.Update();

            if (Input.KeyIsDown(Keys.Escape))
            {
                Exit();
            }
            CurrentScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            CurrentScene.Draw();

            base.Draw(gameTime);
        }
        #endregion
    }
}
