using Microsoft.Xna.Framework;

namespace Zand.Components
{
    public class CoolDown : Component, IUpdateable
    {
        public bool Ready;
        public float Duration;
        public float Counter;

        public CoolDown(float duration, bool startReady = true)
        {
            Duration = duration;
            Ready = startReady;
        }

        public void Update()
        {
            if (!Ready)
            {
                Counter += (float)Time.DeltaTime;

                if (Counter >= Duration)
                {
                    Reset();
                }
            }
        }

        public void Start()
        {
            Reset();
            Ready = false;
        }

        public void Reset()
        {
            Ready = true;
            Counter = 0;
        }
    }
}
