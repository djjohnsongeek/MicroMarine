using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Graphics.Lighting
{
    public class LightMap
    {
        public List<SimpleLight> Lights;
        public BlendState BlendState;
        public RenderTarget2D RenderTarget;
        public GraphicsDevice GraphicsDevice;
        public Color DarknessColor;
        public Camera Camera;


        public LightMap(GraphicsDevice graphicsDevice, int width, int height, Camera camera)
        {
            GraphicsDevice = graphicsDevice;
            RenderTarget = new RenderTarget2D(graphicsDevice, width, height);
            Camera = camera;
            BlendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };
            DarknessColor = new Color(25, 25, 25, 255);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(DarknessColor);

            spriteBatch.Begin(blendState: BlendState, transformMatrix: Camera.GetTransformation());
            //Vector2 lightOrigin = new Vector2(_light.Width / 2, _light.Height / 2);

            //SpriteBatch.Draw(_light, Camera.Position, null, Color.White, 0, lightOrigin, new Vector2(5, 5), SpriteEffects.None, 0);

            //foreach (var entity in Entities.FindEntities("unit"))
            //{
            //    SpriteBatch.Draw(_light, entity.Position, null, Color.White, 0, origin: lightOrigin, Vector2.One, SpriteEffects.None, 0);
            //}



            ////SpriteBatch.Draw(_light, Entities.FindEntity("tileMap").GetComponent<TileMap>().MapCenter.ToVector2(), Color.White);
            //SpriteBatch.End();

            //Core._instance.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
