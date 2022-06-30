using System.Collections.Generic;

namespace Zand
{
    public static class Config
    {
        // Physics
        public const float UnitRepelMangitude = 4f;

        // Group Movement
        public const int FollowLeaderBaseDistance = 255;
        public const float MatchFactor = 0.125F;
        public const float CohesionFactor = 0.5F;
        public const float ArrivalThreshold = 1.0F;
        public const float DestinationFactor = 100.0F;
        public const float CohesionVelocityLimit = 20F;
        public const float AllGroupingTimeLimit = 0.2F;
        public const float GroupingTimeLimit = 2.0F;
        public const float CirclePackingConst = 1.10265779F;
        public const int UnitGroupIdLength = 500;
        public const int MinimumStopDistance = 19;
        public const int MaxGroupingSize = 3;

        // Camera
        public const float ScrollSpeed = 300f;
        public const int CameraEdgeBuffer = 15;

        // Map
        public const int MapWidth = 64;
        public const int MapHeight = 64;
        public static HashSet<int> StaticTiles = new HashSet<int> { 63 };
    }
}
