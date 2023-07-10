using MicroMarine.Components.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zand;
using Zand.AI;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;
using Zand.Graphics;

namespace MicroMarine.Components
{
    // Acts as 'Loading Component' for a Marine
    class Marine : Unit, Zand.IUpdateable
    {
        private StateMachine<Unit> _stateMachine;

        public Marine(int allegianceId)
        {
            Allegiance = new UnitAllegiance(allegianceId);
        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            AttackRange = 250;
            FollowRange = 120;
            Speed = 100;
            Damage = 3;
            AttacksPerSecond = 4.3f;
            AttackInterval = 5 / 60f;

            Entity.AddComponent(new Health(100));
            Entity.AddComponent(new Mover(100));
            Entity.AddComponent(new CommandQueue());


            Texture2D shadowTexture = Scene.Content.GetContent<Texture2D>("smallUnitShadow");
            Texture2D selectTexture = Scene.Content.GetContent<Texture2D>("smallUnitSelect");
            Entity.AddComponent(new UnitShadow(shadowTexture));
            Entity.AddComponent(new SelectionIndicator(selectTexture));

            AddAnimationComponents();
            AddCollisionComponents();
            AddUnitStates();
            AddAllegiance();
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        private void AddAnimationComponents()
        {
            Texture2D marineSheet = Scene.Content.GetContent<Texture2D>("marineSheet");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);

            var animator = new Animator();
            animator.AddAnimation("IdleNorth", new Animation(marineSheet, spriteSheet.GetFrames(0, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleSouth", new Animation(marineSheet, spriteSheet.GetFrames(8, 15), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleEast", new Animation(marineSheet, spriteSheet.GetFrames(16, 23), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleWest", new Animation(marineSheet, spriteSheet.GetFrames(24, 31), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkNorth", new Animation(marineSheet, spriteSheet.GetFrames(32, 39), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkSouth", new Animation(marineSheet, spriteSheet.GetFrames(40, 47), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkEast", new Animation(marineSheet, spriteSheet.GetFrames(48, 55), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkWest", new Animation(marineSheet, spriteSheet.GetFrames(56, 63), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackNorth", new Animation(marineSheet, spriteSheet.GetFrames(64, 71), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackSouth", new Animation(marineSheet, spriteSheet.GetFrames(72, 79), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackEast", new Animation(marineSheet, spriteSheet.GetFrames(80, 87), 24, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackWest", new Animation(marineSheet, spriteSheet.GetFrames(88, 95), 24, Animation.LoopMode.Loop));

            Entity.AddComponent(animator);
        }

        private void AddCollisionComponents()
        {
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(19, 26)), new Vector2(-9, -13)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);

            CircleCollider collider = new CircleCollider(9, new Vector2(0, 6));
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);
        }

        private void AddUnitStates()
        {
            _stateMachine = new StateMachine<Unit>(this);
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.AddState(new Attacking());
            _stateMachine.SetInitialState<Idle>();
        }

        private void AddAllegiance()
        {
            Entity.AddComponent(Allegiance);
        }
    }
}
