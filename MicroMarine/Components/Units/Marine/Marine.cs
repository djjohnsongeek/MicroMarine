using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Assets;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    // Acts as 'Loading Component' for a Marine
    class Marine : Component, Zand.IUpdateable
    {
        // TODO add a Group property and move logic away from UnitGroup's states and into Marine?
        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            Entity.AddComponent(new Health(100));
            //Entity.AddComponent(new WaypointNav());
            //Entity.AddComponent(new WaypointMovement(100));
            Entity.AddComponent(new Mover(100));

            Texture2D marineSheet = Scene.Content.LoadTexture("marineSheet", "Content/marineSheet32.png");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);
            var animator = new Animator();

            #region Setup Animation

            animator.AddAnimation("IdleNorth", new Animation(marineSheet, spriteSheet.GetFrames(0, 7), 8));
            animator.AddAnimation("IdleSouth", new Animation(marineSheet, spriteSheet.GetFrames(8, 15), 8));
            animator.AddAnimation("IdleEast", new Animation(marineSheet, spriteSheet.GetFrames(16, 23), 8));
            animator.AddAnimation("IdleWest", new Animation(marineSheet, spriteSheet.GetFrames(24, 31), 8));
            animator.AddAnimation("WalkNorth", new Animation(marineSheet, spriteSheet.GetFrames(32, 39)));
            animator.AddAnimation("WalkSouth", new Animation(marineSheet, spriteSheet.GetFrames(40, 47)));
            animator.AddAnimation("WalkEast", new Animation(marineSheet, spriteSheet.GetFrames(48, 55)));
            animator.AddAnimation("WalkWest", new Animation(marineSheet, spriteSheet.GetFrames(56, 63)));

            #endregion

            animator.SetAnimation("IdleSouth");
            Entity.AddComponent(animator);

            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(19, 26)), new Vector2(-9, -13)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);
            // Scene.RegisterCollider(mouseCollider);

            Texture2D circleTex = Shapes.CreateCircleTexture(18);

            CircleCollider circle = new CircleCollider(circleTex, 9, new Vector2(0, 6));
            Entity.AddComponent(circle);
            Scene.RegisterCollider(circle);

            Entity.AddComponent(new UnitState(UnitStates.Idle));
        }

        public void Update()
        {
            UpdateMarineAnimation();
        }

        private void UpdateMarineAnimation()
        {
            Animator animator = Entity.GetComponent<Animator>();
            Vector2 velocity = Entity.GetComponent<Mover>().Velocity;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                float dot = Vector2.Dot(Vector2.UnitX, velocity);

                // close to zero, traveling up or down
                if (dot > -0.5F && dot < 0.5F)
                {
                    if (velocity.Y < 0)
                    {
                        if (!animator.AnimationIsRunning("WalkNorth"))
                        {
                            animator.SetAnimation("WalkNorth");
                        }
                    }
                    else if (velocity.Y > 0)
                    {
                        if (!animator.AnimationIsRunning("WalkSouth"))
                        {
                            animator.SetAnimation("WalkSouth");
                        }
                    }
                }
                // close to 1 traveling more horizontal
                if (dot < -0.5 || dot > 0.5F)
                {
                    if (velocity.X > 0)
                    {
                        if (!animator.AnimationIsRunning("WalkEast"))
                        {
                            animator.SetAnimation("WalkEast");
                        }

                    }
                    else if (velocity.X < 0)
                    {
                        if (!animator.AnimationIsRunning("WalkWest"))
                        {
                            animator.SetAnimation("WalkWest");
                        }
                    }
                }
            }
            else
            {
                if (!animator.AnimationIsRunning("IdleSouth"))
                {
                    animator.SetAnimation("IdleSouth");
                }
            }
        }
    }
}
