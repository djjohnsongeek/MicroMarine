using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Zand.Graphics.Lighting
{
    public class LightMap
    {
        public List<SimpleLight> Lights;
        public List<SimpleLight> LightsToRemove;
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
            LightsToRemove = new List<SimpleLight>();
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
                    LightsToRemove.Add(Lights[i]);
                    return;
                }
            }
        }

        public void Update()
        {
            for (int i = LightsToRemove.Count - 1; i >= 0; i--)
            {
                if (LightsToRemove[i].Color.A <= 0)
                {
                    Lights.Remove(LightsToRemove[i]);
                    LightsToRemove.RemoveAt(i);
                    continue;
                }
                LightsToRemove[i].Color.A -= 1;
            }
        }

        public void RenderLightMap(SpriteBatch spriteBatch)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(DarknessColor);
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied, transformMatrix: Camera.GetTransformation());
            for (int i = 0; i < Lights.Count; i++)
            {
                var light = Lights[i];
                spriteBatch.Draw(light.LightTexture, light.Obj.Position + light.Offset, null, light.Color, 0, light.Origin, light.Scale, SpriteEffects.None, 0);
                //spriteBatch.Draw(light.LightTexture, light.Obj.Position + light.Offset, null, light.Color, 0, light.Origin, new Vector2(.2f, .2f), SpriteEffects.None, 0);
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
