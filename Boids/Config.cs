using System.Numerics;

namespace Boids
{
    static class Config
    {
        internal static int BoundsMargin = 200; //200
        internal static float BoundsTurnFactor = 10; //1
        internal static float CohesionFactor = .5f; //.005
        internal static float AvoidaceMinDistance = 18f; //20
        internal static float AvoidanceFactor = .1f; //.05
        internal static float MatchVelocityFactor = .05f; //.05

        internal static int BoidCount = 50; //50
        internal static float MaxSpeed = 400; //200

        internal static int ScreenWidth = 1200; //1200
        internal static int ScreenHeight = 800; //800
        internal static float BoidVision = 100; // 75
        internal static float BoidVisionSquared = BoidVision * BoidVision;
    }
}
