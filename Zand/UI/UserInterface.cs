using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zand.UI
{
    public class UserInterface : IRenderable
    {
        public List<IRenderable> Elements;

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

        public UserInterface()
        {
            _enabled = true;
            Elements = new List<IRenderable>();
        }

        public void AddElement(IRenderable uiElement)
        {
            Elements.Add(uiElement);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in Elements)
            {
                if (element.Enabled)
                {
                    element.Draw(spriteBatch);
                }

            }

            spriteBatch.End();
        }

    }
}
