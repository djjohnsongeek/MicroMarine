using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.Components;
using Microsoft.Xna.Framework.Input;
using Zand.Graphics.Lighting;
using Microsoft.Xna.Framework;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class GlowStickPowerUp : Component, Zand.IUpdateable
    {
        private MouseSelectCollider _selection;
        private Texture2D _texture;
        private Texture2D _lightTexture;
        private Color _glowColor;

        public GlowStickPowerUp(Color color)
        {
            _glowColor = color;
        }

        public override void OnAddedToEntity()
        {
            _selection = Entity.GetComponent<MouseSelectCollider>();
            _texture = Entity.Scene.Content.GetContent<Texture2D>("glowStick");
            _lightTexture = Entity.Scene.Content.GetContent<Texture2D>("light");
        }


        public void Update()
        {
            if (_selection.Selected && Input.KeyWasReleased(Keys.F))
            {
                var glowStick = Entity.Scene.CreateEntity("glowStick", Entity.Position);
                glowStick.AddComponent(
                    new BouncingSprite(new Vector2(80, -80), 28, _texture, Scene.Content.GetContent<Texture2D>("tinyShadow"))
                );

                var light = new SimpleLight(glowStick, _lightTexture, _glowColor, new Vector2(1.5f, 1.5f));
                Entity.Scene.Lighting.AddLight(light);
            }

            Entity.layerDepth = MathUtil.CalculateLayerDepth(Entity.Scene.Camera.GetScreenLocation(Entity.Position).Y, Entity.Dimensions.Y);
        }

    }
}
