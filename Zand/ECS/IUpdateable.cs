namespace Zand
{
    interface IUpdateable
    {
        bool Enabled { get; }
        public void Update() { }
    }
}
