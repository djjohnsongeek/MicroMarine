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
        private bool _loop;

        public Timer(double delay, Action action, bool loop = false)
        {
            _elapsedTime = 0;
            _targetTime = delay;
            _action = action;
            _loop = loop;
        }
        
        public void Update(GameTime gameTime)
        {
            if (!_finished)
            {
                _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime >= _targetTime)
                {
                    _action();
                    if (_loop)
                    {
                        _elapsedTime = 0;
                    }
                    else
                    {
                        _finished = true;
                    }

                }
            }
        }

        public bool Finished => _finished;
    }
}
