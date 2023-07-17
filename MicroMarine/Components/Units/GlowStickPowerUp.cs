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

namespace MicroMarine.Components.Units
{
    class GlowStickPowerUp : Component, Zand.IUpdateable
    {
        private MouseSelectCollider _selection;
        private Texture2D _texture;
        private Texture2D _lightTexture;

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
                glowStick.AddComponent(new SimpleSprite(_texture));
                var light = new SimpleLight(glowStick, _lightTexture, new Color(240, 255, 255, 240), Vector2.One);
                Entity.Scene.Lighting.AddLight(light);
            }

            Entity.layerDepth = MathUtil.CalculateLayerDepth(Entity.Scene.Camera.GetScreenLocation(Entity.Position).Y, Entity.Dimensions.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Entity.Position,
                null,
                Color.White,
                1,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                Entity.layerDepth);
        }
    }
}
