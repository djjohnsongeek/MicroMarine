using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.UI;

namespace Zand.Debug
{
    class DebugConsole : IUpdateable
    {
        SolidRectangle _background;
        private Scene _scene;
        private Queue<string> _messages;
        private TextRenderer _textRenderer;
        private Vector2 _feedPosition;
        private float _messageLife = 2f;

        public bool Enabled { get; set; }

        public DebugConsole(Scene scene, SpriteFont font)
        {
            _scene = scene;
            _messages = new Queue<string>();
            _textRenderer = new TextRenderer(font);
            _background = new SolidRectangle(
                _scene.DebugPixelTexture,
                new Point(0, _scene.ScreenHeight - 100),
                new Point(_scene.ScreenWidth, 100),
                new Color(45, 45, 45, 180)
            );
            _feedPosition = _background.Position;
        }

        public void AddMessage(string msg)
        {
            _messages.Enqueue(msg);
            _messageLife = 2f;

            if (_messages.Count > 6)
            {
                _messages.Dequeue();
            }
        }

        public void Update()
        {
            _messageLife -= (float)Time.DeltaTime;

            if (_messageLife <= 0 && _messages.Count > 0)
            {
                _messages.Dequeue();
                _messageLife = 2f;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Background
            _background.Draw(spriteBatch);

            // Draw Messages
            Vector2 linePosition = _feedPosition;
            foreach (string msg in _messages)
            {
                _textRenderer.DrawString(
                    spriteBatch,
                    msg,
                    linePosition,
                    Color.White,
                    1,
                    false
                );

                linePosition.Y += 16;
            }
        }
    }
}
