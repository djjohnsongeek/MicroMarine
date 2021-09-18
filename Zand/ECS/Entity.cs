using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    class Entity : IUpdateable
    {
        private uint Id { get; }
        private bool Enabled { get; set; }
        public string Name { get; set; }
        private ComponentList Components { get; }

        public void Update() => Components.Update();

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }

    }
}
