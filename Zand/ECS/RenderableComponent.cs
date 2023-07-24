using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS
{
    public class RenderableComponent : Component, IRenderable
    {
        protected float _renderDepth;
        protected int _renderLayer;


        public float RenderDepth { 
            get => _renderDepth;
            set => _renderDepth = value; // TODO set that sort is needed flag
        }
        public int RenderLayer {
            get => _renderLayer;
            set => _renderLayer = value; // TODO set sort needed flag
        }
    }
}
