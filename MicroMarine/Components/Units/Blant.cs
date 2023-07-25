using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
    class Blant: Unit, Zand.IUpdateable
    {
        public Blant(int allegianceId) : base(allegianceId,
            attackRange: 35,
            sightRange: 250,
            followRange: 120,
            speed: 80,
            damage: 30,
            attacksPerSec: 4)
        {

        }
        public Blant()
        {

        }

        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);


            Entity.AddComponent(new Health(250, 250)); //250
            Entity.AddComponent(new Mover(Speed));
            Entity.AddComponent(new CommandQueue());

            // animations
            Texture2D blantSheet = Scene.Content.GetContent<Texture2D>("blantSheet");
            var spriteSheet = new SpriteSheet(blantSheet, 32, 32);
            var animator = new Animator();
            animator.AddAnimation("SpawnSouth", new Animation(blantSheet, spriteSheet.GetFrames(8, 15), 8, Animation.LoopMode.Once));
            animator.AddAnimation("IdleNorth", new Animation(blantSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleSouth", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleEast", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("IdleWest", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkNorth", new Animation(blantSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkSouth", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkEast", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("WalkWest", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackNorth", new Animation(blantSheet, spriteSheet.GetFrames(4, 7), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackSouth", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackEast", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            animator.AddAnimation("AttackWest", new Animation(blantSheet, spriteSheet.GetFrames(0, 3), 8, Animation.LoopMode.Loop));
            Entity.AddComponent(animator);

            // 0 is map, 1 is decals,  2 is units, 3 map top
            animator.RenderLayer = 2;

            // colliders
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(26, 30)), new Vector2(-13, -14));
            Entity.AddComponent(mouseCollider);


            CircleCollider collider = new CircleCollider(11, new Vector2(0, 6));
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);


            // shadows
            Entity.AddComponent(new Decale(Scene.Content.GetContent<Texture2D>("mediumUnitShadow"), new Vector2(16, -12)));

            // states
            _stateMachine.AddState(new BlantSpawn());
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.AddState(new Attacking());
            _stateMachine.SetInitialState<BlantSpawn>();

            // allegiance
            Entity.AddComponent(Allegiance);
        }

        public override void OnRemovedFromEntity()
        {
            var entity = Scene.CreateEntity("deadUnit", Entity.Position);
            Entity.Scene.GetComponent<SoundEffectManager>().PlaySoundEffect("bDeath", limitPlayback: true, randomChoice: true);
            entity.AddComponent(new DeadUnit(Scene.Content.GetContent<Texture2D>("deadBlant"), new Vector2(15, 10), null, 30));
        }
    }


}
