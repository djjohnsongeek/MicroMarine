using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;

namespace MicroMarine.Components
{
    class UnitMovement : Component, Zand.IUpdateable
    {
        private int _speed;
        public Vector2 Velocity;
        public UnitMovement(int speed)
        {
            _speed = speed;
        }

        public void Update()
        {

        }
    }
}
