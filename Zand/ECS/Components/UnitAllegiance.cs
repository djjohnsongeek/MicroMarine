using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Components
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
                    Color = Color.White;
                    break;
                case 2:
                    Color = Color.White;
                    break;
                case 3:
                    Color = Color.White;
                    break;
                case 4:
                    Color = Color.White;
                    break;
                case 5:
                    Color = Color.White;
                    break;
                default:
                    throw new ArgumentException("Invalid Allegiance Id");
            }
        }
    }
}
