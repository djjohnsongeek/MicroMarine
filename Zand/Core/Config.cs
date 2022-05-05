namespace Zand
{
    public static class Config
    {
        // Physics
        public const float UnitRepelMangitude = 3.5f;

        // Group Movement
        public const int FollowLeaderBaseDistance = 255;
        public const float MatchFactor = 0.125F;
        public const float CohesionFactor = 0.5F;
        public const float ArrivalThreshold = 1.0F;
        public const float DestinationFactor = 100.0F;
        public const float CohesionVelocityLimit = 20F;
        public const float AllGroupingTimeLimit = 0.2F;
        public const float GroupingTimeLimit = 2.0F;
        public const float CirclePackingConst = 1.1026577908435840990226529966259F;

        // Camera
        public const float ScrollSpeed = 300f;
        public const int CameraEdgeBuffer = 15;
    }
}
