using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMarine.Utils
{
    public class Config
    {
        // Physics
        public  float UnitRepelMangitude { get; set; }

        // Group Movement
        public  int FollowLeaderBaseDistance { get; set; }
        public  float MatchFactor { get; set; }
        public  float CohesionFactor { get; set; }
        public  float ArrivalThreshold { get; set; }
        public  float DestinationFactor { get; set; }
        public  float CohesionVelocityLimit { get; set; }
        public  float AllGroupingTimeLimit { get; set; }
        public  float GroupingTimeLimit { get; set; }
        public  float CirclePackingConst { get; set; }

        // Camera
        public  float CameraSpeed { get; set; }
        public  int CameraEdgeBuffer { get; set; }
    }
}
