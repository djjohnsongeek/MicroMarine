using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Graphics.Lighting
{
    public class SimpleLight
    {
        public GameObject Obj;
        public Color Color;
        public Texture2D LightTexture;

        public SimpleLight(GameObject obj, Texture2D lightTexture, Color color)
        {
            Obj = obj;
            Color = color;
            LightTexture = lightTexture;
        }
    }
}
