using Microsoft.Xna.Framework;

namespace Zand
{
    public class Camera
    {
        public Matrix Transform;
        public Vector2 Position;
        public int Width;
        public int Height;

        private Scene _scene;
        private float _rotation = 0.0f;
        private float _zoom = 1.0f;
        private float _speed = 399.0f;
        private int _edgeBuffer = 60;

        public Camera(Vector2 position, Scene scene, int width = 960, int height = 540)
        {
            Height = height;
            Width = width;
            Position = position;
            _scene = scene;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move(Vector2 distance)
        {
            Position += distance;
        }

        public void Update()
        {
            Move(getVelocity(Time.DeltaTime));
        }

        public Matrix GetTransformation()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Width * 0.5f, Height * 0.5f, 0));

            return Transform;
        }

        public Vector2 GetWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(GetTransformation()));
        }

        public Vector2 GetScreenLocation(Vector2 worldLocation)
        {
            return Vector2.Transform(worldLocation, GetTransformation());
        }

        private Vector2 getVelocity(double dt)
        {
            double dx = 0f;
            double dy = 0f;

            // left scroll
            if (Input.MousePosition.X < _edgeBuffer)
            {
                dx = -(_speed * dt);
            }

            // right scroll
            if (Input.MousePosition.X > Width - _edgeBuffer)
            {
                dx = _speed * dt;
            }

            // down scroll
            if (Input.MousePosition.Y > Height - _edgeBuffer)
            {
                dy = _speed * dt;
            }

            // scroll up
            if (Input.MousePosition.Y < _edgeBuffer)
            {
                dy = -(_speed * dt);
            }

            return new Vector2((float)dx, (float)dy);
        }
            // go from world to screen space
            // Vector2.Transform(mouseLocation, Camera.TransformMatrix);
    }
}
