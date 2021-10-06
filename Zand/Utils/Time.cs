namespace Zand
{
    public static class Time
    {
        public static double DeltaTime { get; private set; }

        public static void Update(double dt)
        {
            DeltaTime = dt;
        }

    }
}
