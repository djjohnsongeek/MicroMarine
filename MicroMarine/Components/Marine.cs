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
        IdleNorth,
        IdleSouth,
        IdleEast,
        IdleWest,
        WalkNorth,
        WalkSouth,
        WalkEast,
        WalkWest,
    }
    
    // Acts as 'Loading Component' for a Marine
    class Marine : Component, Zand.IUpdateable
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

            #region Setup Animation

            animator.AddAnimation(MarineAnimation.IdleNorth, new Animation(marineSheet, spriteSheet.GetFrames(0, 7), 8));
            animator.AddAnimation(MarineAnimation.IdleSouth, new Animation(marineSheet, spriteSheet.GetFrames(8, 15), 8));
            animator.AddAnimation(MarineAnimation.IdleEast, new Animation(marineSheet, spriteSheet.GetFrames(16, 23), 8));
            animator.AddAnimation(MarineAnimation.IdleWest, new Animation(marineSheet, spriteSheet.GetFrames(24, 31), 8));
            animator.AddAnimation(MarineAnimation.WalkNorth, new Animation(marineSheet, spriteSheet.GetFrames(32, 39)));
            animator.AddAnimation(MarineAnimation.WalkSouth, new Animation(marineSheet, spriteSheet.GetFrames(40, 47)));
            animator.AddAnimation(MarineAnimation.WalkEast, new Animation(marineSheet, spriteSheet.GetFrames(48, 55)));
            animator.AddAnimation(MarineAnimation.WalkWest, new Animation(marineSheet, spriteSheet.GetFrames(56, 63)));

            #endregion

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

        public void Update()
        {
            Animator animator = Entity.GetComponent<Animator>();
            Vector2 velocity = Entity.GetComponent<WaypointMovement>().Velocity;

            if (velocity != Vector2.Zero)
            {
                float dot = Vector2.Dot(Vector2.UnitX, velocity);
                Scene.Debug.Log($"dot {dot}, velocty {velocity.X}, {velocity.Y}");

                // close to zero, traveling up or down
                if (dot > -0.5F && dot < 0.5F)
                {
                    if (velocity.Y < 0)
                    {
                        if (!animator.AnimationIsRunning(MarineAnimation.WalkNorth))
                        {
                            animator.SetAnimation(MarineAnimation.WalkNorth);
                        }
;
                    }
                    else if (velocity.Y > 0)
                    {
                        if (!animator.AnimationIsRunning(MarineAnimation.WalkSouth))
                        {
                            animator.SetAnimation(MarineAnimation.WalkSouth);
                        }
                    }
                }
                // close to 1 traveling more horizontal
                if (dot < -0.5 || dot > 0.5F)
                {
                    if (velocity.X > 0)
                    {
                        if (!animator.AnimationIsRunning(MarineAnimation.WalkEast))
                        {
                            animator.SetAnimation(MarineAnimation.WalkEast);
                        }

                    }
                    else if (velocity.X < 0)
                    {
                        if (!animator.AnimationIsRunning(MarineAnimation.WalkWest))
                        {
                            animator.SetAnimation(MarineAnimation.WalkWest);
                        }
                    }
                }
            }
            else
            {
                if (!animator.AnimationIsRunning(MarineAnimation.IdleSouth))
                {
                    animator.SetAnimation(MarineAnimation.IdleSouth);
                }
            }
        }
    }
}
