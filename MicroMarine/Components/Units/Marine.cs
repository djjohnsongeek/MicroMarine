using MicroMarine.Components.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        public Marine(int allegianceId) : base(allegianceId)
        {

        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            AttackRange = 200;
            SightRange = 250;
            FollowRange = 120;
            Speed = 100;
            Damage = 3;
            AttacksPerSecond = 5f;
            AttackInterval = 5 / 60f;

            Entity.AddComponent(new Health(100, 100));
            Entity.AddComponent(new Mover(Speed));
            Entity.AddComponent(new CommandQueue());



            Texture2D shadowTexture = Scene.Content.GetContent<Texture2D>("smallUnitShadow");
            Texture2D selectTexture = Scene.Content.GetContent<Texture2D>("smallUnitSelect");
            Entity.AddComponent(new Decale(shadowTexture, new Vector2(10, -12)));
            Entity.AddComponent(new SelectionIndicator(selectTexture, new Vector2(10, -12)));

            AddAnimationComponents();
            AddCollisionComponents();
            AddUnitStates();
            AddAllegiance();

            Entity.AddComponent(new GlowStickPowerUp(Color.White));
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
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.AddState(new Attacking());
            _stateMachine.SetInitialState<Idle>();
        }

        public override void OnRemovedFromEntity()
        {
            Entity.Scene.GetComponent<SoundEffectManager>().StopAllSoundEffects(Entity);
            Entity.Scene.GetComponent<SoundEffectManager>().PlaySoundEffect("mDeath", limitPlayback: true, randomChoice: true);
            CreateDeadBody();
        }

        private void CreateDeadBody()
        {
            var deadMarineSheet = Entity.Scene.Content.GetContent<Texture2D>("deadMarineSheet");
            var spriteSheet = new SpriteSheet(deadMarineSheet, 32, 32);
            int index = Entity.Scene.Rng.Next(0, 3);
            Rectangle frame = spriteSheet.GetFrame(index);
            var entity = Scene.CreateEntity("deadUnit", Entity.Position);
            entity.AddComponent(new DeadUnit(deadMarineSheet, new Vector2(15, 0), frame, fadeDuration: 30));
        }

        private void AddAllegiance()
        {
            Entity.AddComponent(Allegiance);
        }
    }
}
