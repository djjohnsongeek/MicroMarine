namespace Zand
{
    public class Component
    {
        public Entity Entity { get; set; }
        private int UpdateOrder { get; set; }
        private bool Enabled { get; set; }
    }
}
