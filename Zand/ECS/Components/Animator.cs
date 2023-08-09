using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zand.Assets;
using Zand.ECS;

namespace Zand.Components
{
    public class Animator : RenderableComponent, IUpdateable
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
        private Color _colorFilter;

        public void SetColorFilter(Color color)
        {
            _colorFilter = color;
        }

        public Animation CurrentAnimation => _currentAnimation;

        public Animator()
        {
            _animations = new Dictionary<string, Animation>();
            _elapsedTime = 0d;
            _colorFilter = Color.White;
        }

        public Animator(Color colorFilter) : this()
        {
            _colorFilter = colorFilter;
        }

        public void AddAnimation(string name, Animation animation)
        {
            _animations.Add(name, animation);
        }

        public void Play(string animationName)
        {
            // Invalid AnimationName
            if (!_animations.ContainsKey(animationName))
            {
                throw new IndexOutOfRangeException($"The animation {animationName} does not exist!");
            }

            // Don't interrupt if same animation
            if (AnimationAlreadyRunning(animationName))
            {
                return;
            }

            // Finish current Animation
            if (_currentAnimation != null)
            {
                _currentAnimation.State = Animation.AnimationState.None;
            }

            _currentAnimation = _animations[animationName];
            _currentAnimation.State = Animation.AnimationState.Running;
            _finalIndex = _currentAnimation.Length - 1;
            _currentIndex = 0;
            _updateTarget = 1d / _currentAnimation.FrameRate;
        }

        public void Update()
        {
            if (_currentAnimation.IsSuspended() || _currentAnimation == null)
            {
                return;
            }

            _elapsedTime += Time.DeltaTime;

            // Main animation update
            _currentFrame = _currentAnimation[_currentIndex];
            if (_elapsedTime >= _updateTarget)
            {
                _currentIndex++;
                _elapsedTime = 0d;
            }

            // Handle the looping
            if (_currentIndex == _finalIndex)
            {
                // Complete Animation so it stops playing
                if (_currentAnimation.Mode == Animation.LoopMode.Once)
                {
                    _currentAnimation.State = Animation.AnimationState.Completed;
                }
                _elapsedTime = 0d;
                _currentIndex = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _currentAnimation.Texture,
                Entity.Position,
                _currentFrame,
                _colorFilter,
                0,
                Entity.Origin,
                1,
                SpriteEffects.None,
                0 // ignored now
           );
        }

        private bool AnimationAlreadyRunning(string name)
        {
            bool isCurrentAnimation = _animations[name] == _currentAnimation;
            return isCurrentAnimation && _currentAnimation.State == Animation.AnimationState.Running;
        }

        public override void OnRemovedFromEntity()
        {
            _currentAnimation.Dispose();
            foreach (KeyValuePair<string, Animation> pair in _animations)
            {
                pair.Value.Dispose();
            }

            _animations.Clear();
            _animations = null;
            _currentAnimation = null;
            base.OnRemovedFromEntity();
        }
    }
}
