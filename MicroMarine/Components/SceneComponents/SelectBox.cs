using Microsoft.Xna.Framework;
using Zand;

namespace MicroMarine.Components
{
    public class SelectBox
    {
        public Vector2 Origin;
        public Rectangle Rect;
        public bool Active { get; private set; }

        public SelectBox()
        {
            Origin = Vector2.Zero;
            Rect = Rectangle.Empty;
            Active = false;
        }

        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
            Active = true;
        }

        public void UpdateBounds()
        {
            Rect = new Rectangle(Origin.ToPoint(), GetBoxSize());

            Zand.Debug.DebugTools.Log($"W: {Rect.Width} H: {Rect.Height}");

            // Adjust box's coordinates based on mouse position
            if (Input.MouseScreenPosition.X < Origin.X)
            {
                Rect.X -= (int)(Origin.X - Input.MouseScreenPosition.X);
            }

            if (Input.MouseScreenPosition.Y < Origin.Y)
            {
                Rect.Y -= (int)(Origin.Y - Input.MouseScreenPosition.Y);
            }
        }

        public void Clear()
        {
            Rect = Rectangle.Empty;
            Origin = Vector2.Zero;
            Active = false;
        }

        public bool Intersects(Rectangle rect)
        {
            return Rect.Intersects(rect);
        }

        public bool IsTiny => Rect.Width < 3 && Rect.Height < 3;

        private Point GetBoxSize()
        {
            return Calc.AbsVector2(Origin - Input.MouseScreenPosition).ToPoint();
        }
    }
}
