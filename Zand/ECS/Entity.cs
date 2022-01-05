using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    public class Entity : IUpdateable
    {
        private uint Id { get; }
        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set => SetEnabled(value);
        }

        public Vector2 Position;
        public Point Dimensions;

        public string Name { get; set; }
        public Scene Scene;
        private ComponentList Components { get; }

        public Entity(string name, Vector2 position, Point dimensions)
        {
            Name = name;
            Position = position;
            Dimensions = dimensions;
            Components = new ComponentList(this);
        }

        public void Update() => Components.Update();

        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.Entity = this;
            component.Scene = Scene;
            component.OnAddedToEntity();
        }

        public T GetComponent<T>() where T: Component
        {
            return Components.GetComponent<T>();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Components.Draw(spriteBatch);
        }

        #region Helpers, Getters, Settings

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

     
        #endregion

    }
}
