using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zand;
using Zand.Assets;

namespace SandboxGame
{

    public class Game1 : Core
    {
        protected override void Initialize()
        {
            CurrentScene = new TestScene();
            base.Initialize();
        }
    }
}
