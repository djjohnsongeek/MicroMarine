using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class Blant: Unit, Zand.IUpdateable
    {
        public Blant(int allegianceId) : base(allegianceId)
        {
        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            AttackRange = 25;
            SightRange = 250;
            FollowRange = 120;
            Speed = 120;
            Damage = 10;
            AttacksPerSecond = 1f;
            AttackInterval = 1 / 60f;

            Entity.AddComponent(new Health(300, 300));
            Entity.AddComponent(new Mover(Speed));
            Entity.AddComponent(new CommandQueue());



            // animations
            Texture2D marineSheet = Scene.Content.GetContent<Texture2D>("blantSheet");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);

            var animator = new Animator();
            animator.AddAnimation("IdleNorth", new Animation(marineSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleSouth", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleEast", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleWest", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkNorth", new Animation(marineSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkSouth", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkEast", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkWest", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackNorth", new Animation(marineSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackSouth", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackEast", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackWest", new Animation(marineSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));

            Entity.AddComponent(animator);

            // colliders
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(19, 26)), new Vector2(-9, -13)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);


            CircleCollider collider = new CircleCollider(9, new Vector2(0, 6));
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);

            // states
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.AddState(new Attacking());
            _stateMachine.SetInitialState<Idle>();

            // allegiance
            Entity.AddComponent(Allegiance);
        }
    }


}
