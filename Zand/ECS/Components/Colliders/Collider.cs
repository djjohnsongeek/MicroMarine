using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    public class Collider : Component, IUpdateable
    {
        public Vector2 Position;
        public Vector2 Offset;

        public virtual void Update()
        {
            Position = Entity.ScreenPosition + Offset;
        }

        // colliders are added as components
        // but then registerd to the phyics sustem by the scene/ entity later
        // Physics handles the collisions, component handles update logic

        public virtual void Draw(SpriteBatch sBatch)
        {

        }

    }
}
