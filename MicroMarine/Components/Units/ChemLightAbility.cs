using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Components;
using Microsoft.Xna.Framework.Input;
using Zand.Graphics.Lighting;
using Microsoft.Xna.Framework;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class ChemLightAbility : UnitAbility
    {
        private MouseSelectCollider _selection;
        private Texture2D _texture;
        private Texture2D _lightTexture;
        private Color _glowColor;

        public ChemLightAbility(Color color, float cooldownDuration) : base(cooldownDuration)
        {
            _glowColor = color;
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
            base.OnRemovedFromEntity();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void ExecuteAbility()
        {
            Vector2 entityScreenPosition = Scene.Camera.GetScreenLocation(Entity.Position);
            var diff = Input.MouseScreenPosition - entityScreenPosition;
            diff.Normalize();

            var chemLight = Entity.Scene.CreateEntity("chemLight", Entity.Position);


            var startVelocity = new Vector3(diff * Entity.GetComponent<Marine>().AttackRange * .75f, 100);


            chemLight.AddComponent(
                new BouncingSprite(
                    startVelocity,
                    20,
                    _texture,
                    Scene.Content.GetContent<Texture2D>("tinyShadow"))
            );

            var light = new SimpleLight(chemLight, _lightTexture, _glowColor, new Vector2(1.5f, 1.5f));
            Entity.Scene.Lighting.AddLight(light);

            base.ExecuteAbility();
        }
    }
}
