﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Assets;

namespace Zand.ECS.Components
{
    public class Animator : Component, IRenderable, IUpdateable
    {
        private Dictionary<Enum, Animation> _animations;
        private Animation _currentAnimation;
        private Rectangle _currentFrame;
        private int _currentIndex = 0;
        private int _finalIndex = 0;

        public Animator(Entity entity)
        {
            Entity = entity;
        }

        public void AddAnimation(Animation animation, Enum name)
        {
            _animations.Add(name, animation);
        }

        public void SetAnimation(Enum name)
        {
            if (!_animations.ContainsKey(name))
            {
                throw new IndexOutOfRangeException($"The animation {name.ToString()} does not exist!");
            }

            _currentAnimation = _animations[name];
            _finalIndex = _currentAnimation.Length - 1;
            _currentIndex = 0;
        }

        public void Update()
        {
            _currentFrame = _currentAnimation[_currentIndex];
            _currentIndex++;

            // looping animation
            if (_currentIndex == _finalIndex)
            {
                _currentIndex = 0;
            }
        }
    }
}
