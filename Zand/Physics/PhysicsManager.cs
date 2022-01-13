using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
namespace Zand.Physics
{
    public static class PhysicsManager
    {
        private static List<Collider> _colliders = new List<Collider>();

        public static void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public static void Update()
        {

        }

        public static void Draw(SpriteBatch sBatch)
        {
            if (Core._instance.CurrentScene.ShowDebug)
            {
                foreach (var collider in _colliders)
                {
                    collider.Draw(sBatch);
                }
            }
        }
    }
}
