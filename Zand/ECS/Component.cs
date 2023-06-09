namespace Zand
{
    public class Component
    {
        public Entity Entity { get; set; }
        public Scene Scene { get; set; }
        private int UpdateOrder { get; set; }

        public bool Enabled { get; set; } = true;

        public virtual void OnAddedToEntity()
        {

        }

        public virtual void OnRemovedFromEntity()
        {
        }
    }
}
