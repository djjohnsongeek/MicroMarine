using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Graphics.Lighting
{
    public class SimpleLight
    {
        public GameObject Obj;
        public Color Color;
        public Texture2D LightTexture;
        public Vector2 Origin;
        public Vector2 Scale;

        public SimpleLight(GameObject obj, Texture2D lightTexture, Color color, Vector2 scale)
        {
            Obj = obj;
            Color = color;
            LightTexture = lightTexture;
            Origin = new Vector2(lightTexture.Width / 2, lightTexture.Height / 2);
            Scale = scale;
        }
    }
}
