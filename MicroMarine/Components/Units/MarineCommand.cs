using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Microsoft.Xna.Framework;

namespace MicroMarine.Components.Units
{
    class MarineCommand : Component, Zand.IUpdateable
    {


        public MarineCommand()
        {
            Time.AddTimer(5, CreateDropShip, loop: true);
        }


        public void Update()
        {

        }

        public void CreateDropShip()
        {
            Scene.CreateEntity("dropShip", Vector2.Zero + new Vector2(-80, -400));
            Entity.AddComponent(new DropShip(600));
        }
    }
}
