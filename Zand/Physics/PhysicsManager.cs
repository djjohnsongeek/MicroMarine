using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            for (int i = 0; i < _colliders.Count; i++)
            {
                for (int j = 0; j < _colliders.Count; j++)
                {

                }
            }

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
