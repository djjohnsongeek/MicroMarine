using Zand;
using MicroMarine.Scenes;

namespace MicroMarine
{
    public class GameMain : Core
    {
        protected override void Initialize()
        {
            CurrentScene = new SampleScene();
            base.Initialize();
        }
    }
}
