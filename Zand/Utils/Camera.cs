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
            if (_scene.GameIsActive)
            {
                Move(GetCameraVelocity(Time.DeltaTime));

                if (Input.MouseScrolledUp())
                {
                    Zoom += .1f;
                }
                else if (Input.MouseScrolledDown())
                {
                    Zoom -= .1f;
                }
            }
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

        public Rectangle GetScreenLocation(Rectangle worldRect)
        {
            Vector2 screenLocation = Vector2.Transform(new Vector2(worldRect.X, worldRect.Y), GetTransformation());
            return new Rectangle((int)screenLocation.X, (int)screenLocation.Y, worldRect.Width, worldRect.Height);
        }

        private Vector2 GetCameraVelocity(double dt)
        {
            double dx = 0f;
            double dy = 0f;

            // left scroll
            if (Input.MouseScreenPosition.X < Config.CameraEdgeBuffer)
            {
                dx = -(Config.ScrollSpeed * dt);
            }

            // right scroll
            if (Input.MouseScreenPosition.X > Width - Config.CameraEdgeBuffer)
            {
                dx = Config.ScrollSpeed * dt;
            }

            // down scroll
            if (Input.MouseScreenPosition.Y > Height - Config.CameraEdgeBuffer)
            {
                dy = Config.ScrollSpeed * dt;
            }

            // scroll up
            if (Input.MouseScreenPosition.Y < Config.CameraEdgeBuffer)
            {
                dy = -(Config.ScrollSpeed * dt);
            }

            return new Vector2((float)dx, (float)dy);
        }
            // go from world to screen space
            // Vector2.Transform(mouseLocation, Camera.TransformMatrix);
    }
}
