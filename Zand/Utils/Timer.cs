using Microsoft.Xna.Framework;
using System;

namespace Zand.Utils
{
    public class Timer
    {
        private double _elapsedTime;
        private double _targetTime;
        private Delegate _action;
        private bool _done;

        public Timer(double delay, Delegate action)
        {
            _elapsedTime = 0;
            _targetTime = delay;
            _action = action;
        }
        
        public void Update(GameTime gameTime)
        {
            if (!_done)
            {
                _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime >= _targetTime)
                {
                    _done = true;
                }
            }
        }
    }
}
