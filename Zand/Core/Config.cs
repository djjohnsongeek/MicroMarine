namespace Zand
{
    public class Config
    {
        // Physics
        public readonly float UnitRepelMangitude = 2.5f;

        // Group Movement
        public readonly int FollowLeaderBaseDistance = 255;
        public readonly float MatchFactor = 0.125F;
        public readonly float CohesionFactor = 0.5F;
        public readonly float ArrivalThreshold = 1.0F;
        public readonly float DestinationFactor = 100.0F;
        public readonly float CohesionVelocityLimit = 20F;
        public readonly float AllGroupingTimeLimit = 0.2F;
        public readonly float GroupingTimeLimit = 2.0F;
        public readonly float CirclePackingConst = 1.1026577908435840990226529966259F;

        // Camera
        public float CameraSpeed = 100.0F;
        public int CameraEdgeBuffer = 10;
    }
}
