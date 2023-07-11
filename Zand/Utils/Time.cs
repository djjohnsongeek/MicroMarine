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

        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            TweenHelper.UpdateSetup(gameTime);

            foreach (var timer in Timers)
            {
                timer.Update(gameTime);
            }
        }

        public static void AddTimer(double delay, Delegate action)
        {
            Timers.Add(new Timer(delay, action));
        }


    }
}
