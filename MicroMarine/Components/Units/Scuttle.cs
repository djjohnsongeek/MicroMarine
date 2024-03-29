﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class Scuttle : Unit, Zand.IUpdateable
    {
        public Scuttle(int allegianceId) : base(
            allegianceId,
            attackRange: 25,
            sightRange: 300,
            followRange: 80,
            speed: 100,
            damage: 1,
            attacksPerSec: 6)
        {

        }

        public Scuttle()
        {

        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);

            var health = new Health(40, 40);
            health.RenderLayer = 5;
            Entity.AddComponent(health);
            Entity.AddComponent(new CommandQueue());

            // animations
            Texture2D scuttleSheet = Scene.Content.GetContent<Texture2D>("scuttleSheet");
            var spriteSheet = new SpriteSheet(scuttleSheet, 16, 16);
            var animator = new Animator();
            animator.AddAnimation("IdleNorth", new Animation(scuttleSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleSouth", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleEast", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleWest", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkNorth", new Animation(scuttleSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkSouth", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkEast", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkWest", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackNorth", new Animation(scuttleSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackSouth", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackEast", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackWest", new Animation(scuttleSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            Entity.AddComponent(animator);

            animator.RenderLayer = 4;

            // colliders
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(16, 16)), new Vector2(-13, -14));
            Entity.AddComponent(mouseCollider);


            CircleCollider collider = new CircleCollider(8, Vector2.Zero);
            collider.Weight = 1;
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);
            Entity.AddComponent(new Mover(Speed));


            // shadows
            var shadow = new SimpleSprite(Scene.Content.GetContent<Texture2D>("tinyShadow"), new Vector2(4, -1));
            shadow.RenderLayer = 2;
            Entity.AddComponent(shadow);

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
