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

            #region Setup Animation TODO: DRY
            // Idle North
            Rectangle[] frames = new Rectangle[]
            {
                spriteSheet[0],
                spriteSheet[1],
                spriteSheet[2],
                spriteSheet[3],
                spriteSheet[4],
                spriteSheet[5],
                spriteSheet[6],
                spriteSheet[7]
            };
            animator.AddAnimation(MarineAnimation.IdleNorth, new Animation(marineSheet, frames, 8));

            // Idle South
            frames = new Rectangle[]
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
            animator.AddAnimation(MarineAnimation.IdleSouth, new Animation(marineSheet, frames, 8));

            // Idle East
            frames = new Rectangle[]
            {
                spriteSheet[16],
                spriteSheet[17],
                spriteSheet[18],
                spriteSheet[19],
                spriteSheet[20],
                spriteSheet[21],
                spriteSheet[22],
                spriteSheet[23],
            };
            animator.AddAnimation(MarineAnimation.IdleEast, new Animation(marineSheet, frames, 8));

            // Idle West
            frames = new Rectangle[]
            {
                spriteSheet[24],
                spriteSheet[25],
                spriteSheet[26],
                spriteSheet[27],
                spriteSheet[28],
                spriteSheet[29],
                spriteSheet[30],
                spriteSheet[31],
            };
            animator.AddAnimation(MarineAnimation.IdleWest, new Animation(marineSheet, frames, 8));

            // Walk North
            frames = new Rectangle[]
            {
                spriteSheet[32],
                spriteSheet[33],
                spriteSheet[34],
                spriteSheet[35],
                spriteSheet[36],
                spriteSheet[37],
                spriteSheet[38],
                spriteSheet[39],
            };
            animator.AddAnimation(MarineAnimation.WalkNorth, new Animation(marineSheet, frames));

            // Walk South
            frames = new Rectangle[]
            {
                spriteSheet[40],
                spriteSheet[41],
                spriteSheet[42],
                spriteSheet[43],
                spriteSheet[44],
                spriteSheet[45],
                spriteSheet[46],
                spriteSheet[47],
            };
            animator.AddAnimation(MarineAnimation.WalkSouth, new Animation(marineSheet, frames));

            // Walk East
            frames = new Rectangle[]
            {
                spriteSheet[48],
                spriteSheet[49],
                spriteSheet[50],
                spriteSheet[51],
                spriteSheet[52],
                spriteSheet[53],
                spriteSheet[54],
                spriteSheet[55],
            };
            animator.AddAnimation(MarineAnimation.WalkEast, new Animation(marineSheet, frames));

            // Walk West
            frames = new Rectangle[]
            {
                spriteSheet[56],
                spriteSheet[57],
                spriteSheet[58],
                spriteSheet[59],
                spriteSheet[60],
                spriteSheet[61],
                spriteSheet[62],
                spriteSheet[63],
            };
            animator.AddAnimation(MarineAnimation.WalkWest, new Animation(marineSheet, frames));

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
            // dot product of unit vectors
            // parallel being 1 or -1 and perpendicular being 0

            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                animator.SetAnimation(MarineAnimation.IdleNorth);
            }

            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                animator.SetAnimation(MarineAnimation.IdleSouth);
            }

            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.D))
            {
                animator.SetAnimation(MarineAnimation.IdleEast);
            }

            if (Input.KeyWasPressed(Microsoft.Xna.Framework.Input.Keys.A))
            {
                animator.SetAnimation(MarineAnimation.IdleWest);
            }
        }
    }
}
