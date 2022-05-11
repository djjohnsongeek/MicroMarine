using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Assets;

namespace Zand.ECS.Components.EntityComponents
{
    public class TileMapRenderer : Component, IRenderable
    {
        private readonly TileMap _map;

        public TileMapRenderer(TileMap map)
        {
            _map = map;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
        }
    }
}
