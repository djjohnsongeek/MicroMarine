using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Physics;

namespace Zand.ECS.Components
{
    public abstract class Collider : Component, IUpdateable
    {
        public bool Static = false;
        public Vector2 Origin;
        public Vector2 Offset;
        public Vector2 Center => Entity.Position + Offset;
        public Color Tint = Color.White;

        public virtual void Update()
        {
        }

        // colliders are added as components
        // but then registerd to the phyics sustem by the scene/ entity later
        // Physics handles the collisions, component handles update logic

        public virtual void Draw(SpriteBatch sBatch)
        {
        }

    }
}
