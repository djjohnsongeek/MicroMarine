using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Utils;
using Zand.Assets;
using Zand.Debug;

namespace Zand.ECS.Components
{
    public class Animator : Component, IRenderable, IUpdateable
    {
        private Dictionary<string, Animation> _animations;
        private Animation _currentAnimation;
        private Rectangle _currentFrame;
        private int _currentIndex = 0;
        private int _finalIndex = 0;
        private double _updateTarget;
        private double _elapsedTime;
        private bool _suppressUpdate = false;
        private float _suppressDuration = 0;


        public Animator()
        {
            _animations = new Dictionary<string, Animation>();
            _elapsedTime = 0d;
        }

        public void AddAnimation(string name, Animation animation)
        {
            _animations.Add(name, animation);
        }

        public void SetAnimation(string name)
        {
            if (!_animations.ContainsKey(name))
            {
                throw new IndexOutOfRangeException($"The animation {name} does not exist!");
            }

            _currentAnimation = _animations[name];
            _finalIndex = _currentAnimation.Length - 1;
            _currentIndex = 0;
            _updateTarget = 1d / _currentAnimation.FrameRate;
        }

        public void Update()
        {
            _elapsedTime += Time.DeltaTime;

            // loop delay
            if (_suppressUpdate)
            {
                if (_elapsedTime < _suppressDuration)
                {
                    return;
                }

                _elapsedTime = 0d;
                _suppressUpdate = false;
                _suppressDuration = 0f;
            }

            // Main animation update
            _currentFrame = _currentAnimation[_currentIndex];
            if (_elapsedTime >= _updateTarget)
            {
                _currentIndex++;
                _elapsedTime = 0d;
            }

            // looping animation
            if (_currentIndex == _finalIndex)
            {
                _currentIndex = 0;
                _elapsedTime = 0d;

                if (_currentAnimation.LoopDelay > 0)
                {
                    SuppressUpdate(_currentAnimation.LoopDelay);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _currentAnimation.Texture,
                Entity.Position,
                _currentFrame,
                Color.White,
                0,
                Entity.Origin,
                1,
                SpriteEffects.None,
                Entity.layerDepth
           );
        }

        private void SuppressUpdate(float timeLength)
        {
            _suppressDuration = timeLength;
            _suppressUpdate = true;
        }

        public bool AnimationIsRunning(string name)
        {
            return  _currentAnimation == _animations[name];
        }
    }
}
