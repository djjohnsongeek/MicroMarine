using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zand.ECS.Collections
{
    public class RenderableComponentList: IUpdateable
    {
        public Scene Scene;
        public List<RenderableComponent> _components;
        private bool _needsSorting;
        private static readonly Comparer<RenderableComponent> RenderableComparer = new RenderableComparer();

        public bool Enabled => true;


        public void MarkAsUnSorted()
        {
            _needsSorting = true;
        }

        public RenderableComponentList(Scene scene)
        {
            _needsSorting = true;
            _components = new List<RenderableComponent>();
            Scene = scene;
        }

        public void Add(RenderableComponent c)
        {
            _components.Add(c);
            MarkAsUnSorted();
        }

        public void Remove(RenderableComponent c)
        {
            _components.Remove(c);
        }

        public void Update()
        {
            foreach (var c in _components)
            {
                c.RenderDepth = c.Entity.RenderDepth;
            }

            if (_needsSorting)
            {
                SortComponents();
            }
        }

        public void Draw()
        {
            Matrix? matrix = Scene.Camera?.GetTransformation();
            Scene.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                null,
                null,
                null,
                matrix
            );

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw(Scene.SpriteBatch);
            }

            Scene.SpriteBatch.End();
        }

        private void SortComponents()
        {
            _components.Sort(RenderableComparer);
            _needsSorting = false;
        }
    }

    public class RenderableComparer : Comparer<RenderableComponent>
    {
        public override int Compare(RenderableComponent x, RenderableComponent y)
        {
            // higher values need to be first
            var res = x.RenderLayer.CompareTo(y.RenderLayer);
            if (res == 0)
            {
                return x.RenderDepth.CompareTo(y.RenderDepth);
            }

            return res;
        }
    }
}
