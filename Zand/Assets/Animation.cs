using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zand.ECS.Components;

namespace Zand.Assets
{
    public class Animation
    {
        // todo define a framerate, option loop delay, etc
        private Texture2D _textureAtlas;
        public Texture2D Texture
        {
            get => _textureAtlas;
        }
        private Rectangle[] _frames;
        public int Length
        {
            get => _frames.Length;
        }

        public Animation(Texture2D texture, Rectangle[] frames)
        {
            _textureAtlas = texture;
            _frames = frames;
        }

        public Rectangle this[int index]
        {
            get => _frames[index];
        }
    }
}
