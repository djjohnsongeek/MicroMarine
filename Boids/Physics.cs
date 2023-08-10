using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Boids
{
    class Physics
    {
        public static void ResolveCollisions(List<Boid> boids)
        {
            foreach (var boid in boids)
            {
                foreach (var possible in boids)
                {
                    if (boid.Id == possible.Id)
                    {
                        continue;
                    }

                    float safeDistance = boid.Radius + possible.Radius;
                    float distance = Vector2.Distance(boid.Position, possible.Position);
                    if (distance < safeDistance)
                    {
                        float power = safeDistance + (distance / safeDistance);
                        float angle = GetAngle(boid.Position, possible.Position);
                        float overlap = safeDistance - distance;
                        var repelVelocity = GetRepelVelocity(angle, power, overlap);
                        boid.Position += repelVelocity;
                        //possible.Position -= repelVelocity / 2;
                    }
                }
            }
        }

        private static Vector2 GetRepelVelocity(float angle, float power, float overlap)
        {
            var repelV = new Vector2(
                GetRepelX(angle, power),
                GetRepelY(angle, power)
            );

            repelV.Normalize();

            return repelV * overlap;
        }

        private static float GetRepelX(double angle, float power)
        {
            return (float)Math.Cos(angle) * power;
        }

        private static float GetRepelY(double angle, float power)
        {
            return (float)Math.Sin(angle) * power;
        }

        public static float GetAngle(Vector2 position1, Vector2 position2)
        {
            var angle = (float)Math.Atan2(position1.Y - position2.Y, position1.X - position2.X);
            return angle;
        }
    }
}
