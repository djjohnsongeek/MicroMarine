using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand.Utils;

namespace Zand
{
    public class Core : Game
    {
        public static GraphicsDeviceManager GraphicsManager;
        public static new GraphicsDevice GraphicsDevice;
        public static ZandContentManager GlobalContent;

        private SpriteBatch _spriteBatch;
        public Scene CurrentScene;

        internal static Core _instance;

        public Core()
        {
            GraphicsManager = new GraphicsDeviceManager(this);
            GraphicsDevice = base.GraphicsDevice;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _instance = this;
            GlobalContent = new ZandContentManager(Services, Content.RootDirectory);
        }

        #region Game Overrides

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CurrentScene = new Scene();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(base.GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime.ElapsedGameTime.TotalSeconds);
            Input.Update();

            if (Input.KeyIsDown(Keys.Escape))
            {
                Exit();
            }
            Camera.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            CurrentScene.Draw();
            // Effects.Draw();
            // UI.Draw();

            base.Draw(gameTime);
        }
        #endregion
    }
}
