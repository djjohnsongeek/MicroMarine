using System.Collections.Generic;

namespace Zand.ECS.Collections
{
    public class RenderableComponentList: IUpdateable
    {
        public List<RenderableComponent> _components;
        private bool _needsSorting;
        private static readonly Comparer<RenderableComponent> RenderableComparer = new RenderableComparer();

        public bool Enabled => true;

        public RenderableComponentList()
        {
            _needsSorting = true;
        }


        public void Update()
        {
            if (_needsSorting)
            {
                _components.Sort(RenderableComparer);
            }
        }
    }

    public class RenderableComparer : Comparer<RenderableComponent>
    {
        public override int Compare(RenderableComponent x, RenderableComponent y)
        {
            // higher values need to be first
            // x - y
            var diff = x.RenderLayer - y.RenderLayer;
            if (diff == 0)
            {
                float depthDiff = x.RenderDepth - x.RenderLayer;
                return Calc.CompareFloat(x.RenderDepth, y.RenderDepth);
            }
            else
            {
                return diff;
            }
        }
    }
}
