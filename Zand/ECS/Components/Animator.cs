using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zand.Assets;

namespace Zand.Components
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

        public void Play(string animationName)
        {
            // Invalid AnimationName
            if (!_animations.ContainsKey(animationName))
            {
                throw new IndexOutOfRangeException($"The animation {animationName} does not exist!");
            }

            // Don't Interrupt Current animation
            if (AnimationAlreadyRunning(animationName))
            {
                return;
            }

            // Finish current Animation
            if (_currentAnimation != null)
            {
                _currentAnimation.State = Animation.AnimationState.None;
            }

            // Setup next animation
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _currentAnimation.Texture,
                Entity.Position,
                _currentFrame,
                GetEntityColor(),
                0,
                Entity.Origin,
                1,
                SpriteEffects.None,
                Entity.layerDepth
           );
        }

        private Color GetEntityColor()
        {
            Color entityColor = Color.White;
            UnitAllegiance allegiance = Entity.GetComponent<UnitAllegiance>();

            if (allegiance is not null)
            {
                entityColor = allegiance.Color;
            }

            return entityColor;
        }

        private bool AnimationAlreadyRunning(string name)
        {
            bool isCurrentAnimation = _animations[name] == _currentAnimation;
            return isCurrentAnimation && _currentAnimation.State == Animation.AnimationState.Running;
        }
    }
}
