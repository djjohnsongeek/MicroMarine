namespace Zand
{
    public interface IUpdateable
    {
        bool Enabled { get; }
        public void Update() { }
    }
}
