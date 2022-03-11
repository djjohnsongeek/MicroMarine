﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Assets;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    public enum MarineAnimation
    {
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

            animator.AddAnimation(0, new Animation(marineSheet, spriteSheet.GetFrames(0, 7), 8));
            animator.AddAnimation(1, new Animation(marineSheet, spriteSheet.GetFrames(8, 15), 8));
            animator.AddAnimation(2, new Animation(marineSheet, spriteSheet.GetFrames(16, 23), 8));
            animator.AddAnimation(3, new Animation(marineSheet, spriteSheet.GetFrames(24, 31), 8));
            animator.AddAnimation(4, new Animation(marineSheet, spriteSheet.GetFrames(32, 39)));
            animator.AddAnimation(5, new Animation(marineSheet, spriteSheet.GetFrames(40, 47)));
            animator.AddAnimation(6, new Animation(marineSheet, spriteSheet.GetFrames(48, 55)));
            animator.AddAnimation(7, new Animation(marineSheet, spriteSheet.GetFrames(56, 63)));

            #endregion

            animator.SetAnimation(1);
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
            UpdateMarineAnimation();
        }

        private void UpdateMarineAnimation()
        {
            Animator animator = Entity.GetComponent<Animator>();
            Vector2 velocity = Entity.GetComponent<WaypointMovement>().Velocity;

            if (velocity != Vector2.Zero)
            {
                float dot = Vector2.Dot(Vector2.UnitX, velocity);
                // close to zero, traveling up or down
                if (dot > -0.5F && dot < 0.5F)
                {
                    if (velocity.Y < 0)
                    {
                        if (!animator.AnimationIsRunning(4))
                        {
                            animator.SetAnimation(4);
                        }
                    }
                    else if (velocity.Y > 0)
                    {
                        if (!animator.AnimationIsRunning(5))
                        {
                            animator.SetAnimation(5);
                        }
                    }
                }
                // close to 1 traveling more horizontal
                if (dot < -0.5 || dot > 0.5F)
                {
                    if (velocity.X > 0)
                    {
                        if (!animator.AnimationIsRunning(6))
                        {
                            animator.SetAnimation(6);
                        }

                    }
                    else if (velocity.X < 0)
                    {
                        if (!animator.AnimationIsRunning(7))
                        {
                            animator.SetAnimation(7);
                        }
                    }
                }
            }
            else
            {
                if (!animator.AnimationIsRunning(1))
                {
                    animator.SetAnimation(1);
                }
            }
        }
    }
}
