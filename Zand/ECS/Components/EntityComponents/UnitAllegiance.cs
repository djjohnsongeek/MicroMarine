using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    public class UnitAllegiance: Component
    {
        public int Id { get; }
        public Color Color { get; private set; }

        public UnitAllegiance(int id)
        {
            Id = id;
            SetColor();
        }

        private void SetColor()
        {
            switch(Id)
            {
                case 1:
                    Color = new Color(78, 113, 253);
                    break;
                case 2:
                    Color = new Color(252, 78, 78);
                    break;
                case 3:
                    Color = new Color(91, 178, 83);
                    break;
                case 4:
                    Color = new Color(238, 253, 116);
                    break;
                default:
                    throw new ArgumentException("Invalid Allegiance Id");
            }
        }
    }
}
