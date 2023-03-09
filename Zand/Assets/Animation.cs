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
        public enum AnimationState
        {
            None,
            Running,
            Paused,
            Completed,
        }

        public enum LoopMode
        {
            Loop,
            Once,
        }

        // todo define a framerate, optional loop delay, etc
        private Texture2D _textureAtlas;
        public Texture2D Texture
        {
            get => _textureAtlas;
        }
        public int FrameRate;
        public LoopMode Mode;
        public AnimationState State;


        private Rectangle[] _frames;
        public int Length
        {
            get => _frames.Length;
        }

        public Animation(Texture2D texture, Rectangle[] frames, int frameRate, LoopMode mode)
        {
            _textureAtlas = texture;
            _frames = frames;
            FrameRate = frameRate;
            Mode = mode;
            State = AnimationState.None;
        }

        public Rectangle this[int index]
        {
            get => _frames[index];
        }

        public bool IsSuspended()
        {
            return State == AnimationState.Completed || State == AnimationState.Paused || State == AnimationState.None;
        }
    }
}
