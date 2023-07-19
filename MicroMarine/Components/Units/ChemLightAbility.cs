using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Components;
using Microsoft.Xna.Framework.Input;
using Zand.Graphics.Lighting;
using Microsoft.Xna.Framework;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class ChemLightAbility : Component, Zand.IUpdateable
    {
        private MouseSelectCollider _selection;
        private Texture2D _texture;
        private Texture2D _lightTexture;
        private Color _glowColor;
        private CoolDown _coolDown;
        
        public ChemLightAbility(Color color, float cooldownDuration)
        {
            _glowColor = color;
            _coolDown = new CoolDown(cooldownDuration);
        }

        public override void OnAddedToEntity()
        {
            _selection = Entity.GetComponent<MouseSelectCollider>();
            _texture = Entity.Scene.Content.GetContent<Texture2D>("glowStick");
            _lightTexture = Entity.Scene.Content.GetContent<Texture2D>("light");
        }

        public override void OnRemovedFromEntity()
        {
            _selection = null;
            _texture = null;
            _lightTexture = null;
            _coolDown = null;
        }

        public bool OnCoolDown => !_coolDown.Ready;

        public void Update()
        {
            _coolDown.Update();
        }

        public void SpawnChemLight()
        {
            var glowStick = Entity.Scene.CreateEntity("glowStick", Entity.Position);
            glowStick.AddComponent(
                new BouncingSprite(
                    new Vector2(80, -80),
                    28,
                    _texture,
                    Scene.Content.GetContent<Texture2D>("tinyShadow"))
            );

            var light = new SimpleLight(glowStick, _lightTexture, _glowColor, new Vector2(1.5f, 1.5f));
            Entity.Scene.Lighting.AddLight(light);

            _coolDown.Start();
        }
    }
}
