namespace Zand
{
    public class Component
    {
        public Entity Entity { get; set; }
        private int UpdateOrder { get; set; }
        public bool Enabled
        {
            get => _enabled;
            set => SetEnabled(value);
        }
        private bool _enabled = true;

        public void Disable()
        {
            SetEnabled(false);
        }
        public void Enable()
        {
            SetEnabled(true);
        }

        private void SetEnabled(bool value)
        {
            _enabled = value;
        }
    }
}
