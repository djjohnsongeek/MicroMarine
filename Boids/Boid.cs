using Microsoft.Xna.Framework;


namespace Boids
{
    class Boid
    {
        public int Id;
        public Vector2 Position;
        public Vector2 Velocity;
        public int Radius;

        public Boid(int id, Vector2 position, Vector2 velocity)
        {
            Id = id;
            Position = position;
            Velocity = velocity;
            Radius = 8;
        }
    }
}
