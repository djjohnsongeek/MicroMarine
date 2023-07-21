using Apos.Tweens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand.Utils;

namespace Zand
{
    public static class Time
    {
        public static double DeltaTime { get; private set; }
        private static List<Timer> Timers = new List<Timer>();
        private static List<int> _timersToRemove = new List<int>();

        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            TweenHelper.UpdateSetup(gameTime);
            UpdateTimers(gameTime);
            RemoveFinshedTimers();
        }

        public static void UpdateTimers(GameTime gameTime)
        {
            for (int i = 0; i < Timers.Count; i++)
            {
                Timers[i].Update(gameTime);
                if (Timers[i].Finished)
                {
                    _timersToRemove.Add(i);
                }
            }
        }

        public static void RemoveFinshedTimers()
        {
            _timersToRemove.Sort();
            for (int i = _timersToRemove.Count - 1; i >= 0; i--)
            {
                Timers.RemoveAt(_timersToRemove[i]);
            }

            _timersToRemove.Clear();
        }

        public static void AddTimer(double delay, Action action, bool loop = false)
        {
            Timers.Add(new Timer(delay, action, loop));
        }
    }
}
