using Microsoft.Xna.Framework;


namespace Boids
{
    class Boid
    {
        public int Id;
        public Vector2 Position;
        public Vector2 Velocity;
        public int Radius;
        private Color _color;
        public Color BorderColor;
        public bool Colliding = false;

        public Color Color
        {
            get
            {
                return Idle ? Color.White : Color.LawnGreen;
            }
            set
            {
                _color = value;
            }
        }
        public bool Idle;

        public Waypoint Waypoint;

        public Boid(int id, Vector2 position, Vector2 velocity)
        {
            Id = id;
            Position = position;
            Velocity = velocity;
            Radius = Calc.RandomInt(8, 12);
            Idle = true;
            Waypoint = null;

            Color = Color.LawnGreen;
            BorderColor = Color.Black;
        }
    }
}
