using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Assets;
using Zand.ECS.Components;

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

            MouseSelector mouseCollider = new MouseSelector(new Rectangle(Entity.Position.ToPoint(), new Point(32, 32)));
            Entity.AddComponent(mouseCollider);
            Scene.RegisterCollider(mouseCollider);

            Entity.AddComponent(new Health(100));
        }
    }
}
