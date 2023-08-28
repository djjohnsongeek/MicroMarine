namespace MicroMarine
{
    static class BoidConfig
    {
        internal static float CohesionFactor = .25f;
        internal static float SeperationMinDistance = 3.5f;
        internal static float SeperationFactor = .8f;
        internal static float GroupAlignmentFactor = 5f;
        internal static float DestinationFactor = 0.339f;
        internal static float ArrivalDrag = 0;
        internal static float ArrivalSpeedLimit = 8;
        internal static int BoidCount = 50;
        internal static float MaxSpeed = 400;
        internal static float BoidVision = 100;
        internal static float BoidVisionSquared = 10000;
        internal static float AvoidOtherFactor = 1;
        internal static float AvoidOtherMinDist = 0;
    }
}
