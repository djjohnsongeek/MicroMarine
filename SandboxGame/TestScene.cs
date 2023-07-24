using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.Assets;

namespace SandboxGame
{
    class TestScene : Scene
    {
        public override void Load()
        {
            base.Load();

            var tileMap = new TiledMap("Content/Simple.tmx");

        }
    }
}
