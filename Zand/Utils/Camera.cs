using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Zand
{
    public static class Camera
    {
        public static Matrix Transform;
        public static Vector2 Position = new Vector2(1920 / 2, 1080 / 2);
        public static int Width = 960;
        public static int Height = 540;

        private static float _rotation = 0.0f;
        private static float _zoom = 1.0f;
        private static float _speed = 399.0f;
        private static int _edgeBuffer = 60;

        public static float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; }
        }

        public static float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public static void Move(Vector2 distance)
        {
            Position += distance;
        }

        public static void Update()
        {
            Move(getVelocity(Time.deltaTime));
        }

        public static Matrix GetTransformation()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Width * 0.5f, Height * 0.5f, 0));

            return Transform;
        }

        public static Vector2 GetWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(GetTransformation()));
        }

        public static Vector2 GetScreenLocation(Vector2 worldLocation)
        {
            return Vector2.Transform(worldLocation, GetTransformation());
        }

        private static Vector2 getVelocity(double dt)
        {
            double dx = 0f;
            double dy = 0f;

            // left scroll
            if (Input.mState.X < _edgeBuffer)
            {
                dx = -(_speed * dt);
            }

            // right scroll
            if (Input.mState.X > Width - _edgeBuffer)
            {
                dx = _speed * dt;
            }

            // down scroll
            if (Input.mState.Y > Height - _edgeBuffer)
            {
                dy = _speed * dt;
            }

            // scroll up
            if (Input.mState.Y < _edgeBuffer)
            {
                dy = -(_speed * dt);
            }

            return new Vector2((float)dx, (float)dy);
        }
            // go from world to screen space
            // Vector2.Transform(mouseLocation, Camera.TransformMatrix);
    }
}
