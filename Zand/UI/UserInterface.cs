using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zand.UI
{
    public class UserInterface
    {
        public List<UIElement> Elements;
        private Scene _scene;

        private bool _enabled;
        public bool Enabled => _enabled;

        public T GetElement<T>()
        {
            foreach (var e in Elements)
            {
                if (e is T t)
                {
                    return t;
                }
            }

            return default(T);
        }

        public UserInterface(Scene scene)
        {
            _enabled = true;
            _scene = scene;
            Elements = new List<UIElement>();
        }

        public void AddElement(UIElement uiElement)
        {
            Elements.Add(uiElement);
        }

        public void Update()
        {
            for (int i = 0; i < Elements.Count; i++ )
            {
                Elements[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
