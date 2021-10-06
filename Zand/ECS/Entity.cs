using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    public class Entity : IUpdateable
    {
        private uint Id { get; }
        private bool Enabled { get; set; }
        public Vector2 Position;
        public string Name { get; set; }
        public Scene Scene;
        private ComponentList Components { get; }

        public Entity(string name, Vector2 position)
        {
            Name = name;
            Position = position;
            Enabled = true;
            Components = new ComponentList(this);
        }

        public void Update() => Components.Update();

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Components.Draw(spriteBatch);
        }

    }
}
