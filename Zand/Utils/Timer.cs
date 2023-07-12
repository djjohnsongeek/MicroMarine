using Microsoft.Xna.Framework;
using System;

namespace Zand.Utils
{
    public class Timer
    {
        private double _elapsedTime;
        private double _targetTime;
        private Action _action;
        private bool _finished;

        public Timer(double delay, Action action)
        {
            _elapsedTime = 0;
            _targetTime = delay;
            _action = action;
        }
        
        public void Update(GameTime gameTime)
        {
            if (!_finished)
            {
                _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime >= _targetTime)
                {
                    _action();
                    _finished = true;
                }
            }
        }

        public bool Finished => _finished;
    }
}
