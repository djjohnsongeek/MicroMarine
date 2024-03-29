﻿using MicroMarine.Components.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    // Acts as 'Loading Component' for a Marine
    class Marine : Unit, Zand.IUpdateable
    {
        public Marine(int allegianceId) : base(allegianceId,
            attackRange: 200,
            sightRange: 250,
            followRange: 120,
            speed: 160,
            damage: 5,
            attacksPerSec: 5)
        {

        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);


            var health = new Health(100, 100);
            Entity.AddComponent(health);
            health.RenderLayer = 5;
            Entity.AddComponent(new CommandQueue());

            Texture2D shadowTexture = Scene.Content.GetContent<Texture2D>("smallUnitShadow");
            Texture2D selectTexture = Scene.Content.GetContent<Texture2D>("smallUnitSelect");
            var unitShadow = new SimpleSprite(shadowTexture, new Vector2(10, -12));
            var selectionShadow = new SelectionIndicator(selectTexture, new Vector2(10, -12));
            unitShadow.RenderLayer = 2;
            selectionShadow.RenderLayer = 3;
            Entity.AddComponent(unitShadow);
            Entity.AddComponent(selectionShadow);

            AddAnimationComponents();
            AddCollisionComponents();
            AddUnitStates();
            AddAllegiance();

            Entity.AddComponent(new ChemLightAbility(Color.White, 29));

            var controlGroup = new ControlGroup();
            controlGroup.RenderLayer = 6;
            Entity.AddComponent(controlGroup);
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
            animator.RenderLayer = 4;
        }

        private void AddCollisionComponents()
        {
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(21, 30)), new Vector2(-10, -14)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);

            CircleCollider collider = new CircleCollider(9, new Vector2(0, 8));
            collider.Weight = 2;
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);

            Entity.AddComponent(new Mover(Speed));
        }

        private void AddUnitStates()
        {
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.AddState(new Attacking());
            _stateMachine.AddState(new ExecuteAbility());
            _stateMachine.SetInitialState<Idle>();
        }

        public override void OnRemovedFromEntity()
        {
            var sfxManger = Entity.Scene.GetComponent<SoundEffectManager>();
            var controlGroups = Entity.Scene.GetComponent<UnitSelector>().ControlGroups;

            sfxManger.StopAllSoundEffects(Entity);
            sfxManger.PlaySoundEffect("mDeath", limitPlayback: true, randomChoice: true);
            controlGroups.RemoveEntity(Entity);
            CreateDeadBody();
        }

        private void CreateDeadBody()
        {
            var deadMarineSheet = Entity.Scene.Content.GetContent<Texture2D>("deadMarineSheet");
            var spriteSheet = new SpriteSheet(deadMarineSheet, 32, 32);
            int index = Entity.Scene.Rng.Next(0, 3);
            Rectangle frame = spriteSheet.GetFrame(index);
            var entity = Scene.CreateEntity("deadUnit", Entity.Position, new Point(32, 32));
            var component = new DeadUnit(deadMarineSheet, new Vector2(15, 0), frame, fadeDuration: 30);
            entity.AddComponent(component);
        }

        private void AddAllegiance()
        {
            Entity.AddComponent(Allegiance);
        }
    }
}
