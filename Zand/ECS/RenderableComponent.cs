using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    public class RenderableComponent : Component, IRenderable
    {
        protected float _prevRenderDepth;
        protected float _renderDepth;
        protected int _renderLayer;


        public float RenderDepth { 
            get => _renderDepth;
            set
            {
                _prevRenderDepth = _renderDepth;
                _renderDepth = value;

                if (_prevRenderDepth != _renderDepth)
                {
                    Entity.Scene.RenderableComponents.MarkAsUnSorted();
                }
            }
        }

        public int RenderLayer {
            get => _renderLayer;
            set
            {
                _renderLayer = value;
                Entity.Scene.RenderableComponents.MarkAsUnSorted();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
