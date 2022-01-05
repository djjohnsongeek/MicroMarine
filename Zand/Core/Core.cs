using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Zand.UI;
using Zand.Utils;

namespace Zand
{
    public class Core : Game
    {
        public static GraphicsDeviceManager GraphicsManager;
        //public static new GraphicsDevice GraphicsDevice;
        public static ZandContentManager GlobalContent;
        public Scene CurrentScene;

        public ushort FPS { get; private set; } = 0;
        private ulong _frameCount;
        private double _frameSeconds;
        private TextRenderer _fpsRenderer;

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
            IsFixedTimeStep = false;
            GraphicsManager.SynchronizeWithVerticalRetrace = false;
            GraphicsManager.ApplyChanges();

            if (CurrentScene == null)
            {
                throw new NullReferenceException("Scene Cannot be Null");
            }

            CurrentScene.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CurrentScene.Load();
            _fpsRenderer = new TextRenderer(CurrentScene.Content.GetContent<SpriteFont>("DebugFont"));
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
            UpdateFPS();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            CurrentScene.Draw();
            DrawFPS();

            base.Draw(gameTime);
        }
        #endregion

        #region Utilities
        private void UpdateFPS()
        {
            _frameCount++;
            _frameSeconds += Time.DeltaTime;

            if (_frameSeconds > 1)
            {
                FPS = (ushort)(_frameCount / _frameSeconds);
                _frameCount = 0;
                _frameSeconds = 0;
            }
        }
        private void DrawFPS()
        {
            Vector2 position = new Vector2(CurrentScene.ScreenWidth - 75, 0);

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            _fpsRenderer.DrawString(spriteBatch, $"FPS: {FPS}", position, Color.Yellow, 1, false); 
            spriteBatch.End();
        }
        #endregion
    }
}
