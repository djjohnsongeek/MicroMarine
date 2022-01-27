using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Assets;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    public enum MarineAnimation
    {
        IdleSouth,
    }
    
    // Acts as 'Loading Component' for a Marine
    class Marine : Component
    {
        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            Entity.AddComponent(new Health(100));
            Entity.AddComponent(new WaypointNav());
            Entity.AddComponent(new WaypointMovement(100));

            Texture2D marineSheet = Scene.Content.LoadTexture("marineSheet", "Content/marineSheet32.png");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);
            var animator = new Animator();

            Rectangle[] frames = new Rectangle[]
            {
                spriteSheet[8],
                spriteSheet[9],
                spriteSheet[10],
                spriteSheet[11],
                spriteSheet[12],
                spriteSheet[13],
                spriteSheet[14],
                spriteSheet[15],
            };
            var idleSouthAnimation = new Animation(marineSheet, frames, 8);

            animator.AddAnimation(MarineAnimation.IdleSouth, idleSouthAnimation);
            animator.SetAnimation(MarineAnimation.IdleSouth);
            Entity.AddComponent(animator);

            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(19, 26)), new Vector2(-9, -13)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);
            Scene.RegisterCollider(mouseCollider);

            Texture2D circleTex = Shapes.CreateCircleTexture(18);

            CircleCollider circle = new CircleCollider(circleTex, 9, new Vector2(0, 6));
            Entity.AddComponent(circle);
            Scene.RegisterCollider(circle);
        }
    }
}
