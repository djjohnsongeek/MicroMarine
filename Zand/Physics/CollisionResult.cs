namespace Zand.Physics
{
    public struct CollisionResult
    {
        public bool Collides;
        public float Distance;
        public float SafeDistance;
        public float OverlapDistance;
        public float RepelStrength;
        public float Angle;

        public void SetRepelStrength()
        {
            RepelStrength = SafeDistance + (Distance / SafeDistance);
            OverlapDistance = SafeDistance - Distance;
        }
    }
}
