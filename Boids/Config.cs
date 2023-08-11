using Microsoft.Xna.Framework;

namespace Boids
{
    static class Config
    {
        internal static float BoundsMargin = 60; //200
        internal static float BoundRepelFactor = 50; //1
        internal static float CohesionFactor = .25f; //.005
        internal static float AvoidanceMinDist = 3.5f; //20
        internal static float AvoidanceFactor = .8f; //.05
        internal static float GroupAlignmentFactor = 5f; //.05
        internal static float DestinationFactor = 0.069f; // 1
        internal static float ArrivalDrag = .1f;
        internal static float ArrivalSpeedLimit = 8f;
        internal static int BoidCount = 50; //50
        internal static float MaxSpeed = 400; //200
        internal static int ScreenWidth = 1200; //1200
        internal static int ScreenHeight = 800; //800
        internal static float BoidVision = 100; // 75
        internal static bool CollisionsEnabled = true;
        internal static float BoidVisionSquared = BoidVision * BoidVision;
        internal static Vector2 BoundsOrigin => new Vector2(BoundsMargin, BoundsMargin);
        internal static Vector2 BoundsSize => new Vector2(ScreenWidth - BoundsMargin * 2, ScreenHeight - BoundsMargin * 2);
    }
}
