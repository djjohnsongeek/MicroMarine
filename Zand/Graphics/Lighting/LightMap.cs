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

            // Multiply
            BlendState = new BlendState()
            {
                AlphaSourceBlend = Blend.DestinationAlpha,
                AlphaDestinationBlend = Blend.Zero,
                AlphaBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero,
                ColorBlendFunction = BlendFunction.Add
            };
            DarknessColor = new Color(0, 0, 0, 255);
            Lights = new List<SimpleLight>();
        }

        public void AddLight(SimpleLight light)
        {
            Lights.Add(light);
        }

        public void RemoveLight(int objectId)
        {
            for (int i = Lights.Count - 1; i >= 0; i--)
            {
                if (Lights[i].Obj.Id == objectId)
                {
                    Lights.RemoveAt(i);
                    return;
                }
            }
        }

        public void RenderLightMap(SpriteBatch spriteBatch)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(DarknessColor);
            spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: Camera.GetTransformation());
            for (int i = 0; i < Lights.Count; i++)
            {
                var light = Lights[i];
                spriteBatch.Draw(light.LightTexture, light.Obj.Position, null, light.Color, 0, light.Origin, light.Scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            Core._instance.GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState);
            spriteBatch.Draw(RenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
