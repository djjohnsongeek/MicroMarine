using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Debug;
using Zand.Physics;

namespace Zand.Colliders
{
    public abstract class Collider : Component, IUpdateable, ICollider
    {
        public bool Static = false;
        public Vector2 Origin;
        public Vector2 Offset;
        public Vector2 Center => Entity.Position + Offset;
        public Color Tint = Color.White;

        public virtual float Right { get; }
        public virtual float Left { get;  }
        public virtual float Top { get;  }
        public virtual float Bottom { get; }

        public virtual Vector2 TopLeft { get; }
        public virtual Vector2 TopRight { get; }
        public virtual Vector2 BottomLeft { get; }
        public virtual Vector2 BottomRight { get; }

        public virtual Vector2 TopCenter { get; }
        public virtual Vector2 RightCenter  { get; }
        public virtual Vector2 BottomCenter { get; }
        public virtual Vector2 LeftCenter { get; }

        public virtual void Update()
        {
        }

        // colliders are added as components
        // but then registerd to the phyics sustem by the scene/ entity later
        // Physics handles the collisions, component handles update logic

        public virtual void Draw(SpriteBatch sBatch)
        {
        }

        public override void OnRemovedFromEntity()
        {
            Scene.Physics.RemoveCollider(this);
            base.OnRemovedFromEntity();
        }

    }
}
