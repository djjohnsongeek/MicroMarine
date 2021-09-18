using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    class Component
    {
        public Entity Entity { get; set; }
        private int UpdateOrder { get; set; }
        private bool Enabled { get; set; }
    }
}
