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
    class Marine : Component, Zand.IUpdateable
    {
        private StateMachine<Marine> _stateMachine;

        public float Range { get; private set; }
        public float Speed { get; private set; }

        // TODO add a Group property and move logic away from UnitGroup's states and into Marine?
        public override void OnAddedToEntity()
        {
            Entity.Origin = new Vector2(Entity.Dimensions.X / 2, Entity.Dimensions.Y / 2);
            Range = 120;
            Speed = 100;

            Entity.AddComponent(new Health(100));
            Entity.AddComponent(new Mover(100));
            Entity.AddComponent(new CommandQueue());


            Texture2D texture = Scene.Content.GetContent<Texture2D>("smallUnitShadow");
            Entity.AddComponent(new UnitShadow(texture));

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
            Texture2D marineSheet = Scene.Content.LoadTexture("marineSheet", "Content/marineSheet32.png");
            var spriteSheet = new SpriteSheet(marineSheet, 32, 32);

            var animator = new Animator();
            animator.AddAnimation("IdleNorth", new Animation(marineSheet, spriteSheet.GetFrames(0, 7), 8));
            animator.AddAnimation("IdleSouth", new Animation(marineSheet, spriteSheet.GetFrames(8, 15), 8));
            animator.AddAnimation("IdleEast", new Animation(marineSheet, spriteSheet.GetFrames(16, 23), 8));
            animator.AddAnimation("IdleWest", new Animation(marineSheet, spriteSheet.GetFrames(24, 31), 8));
            animator.AddAnimation("WalkNorth", new Animation(marineSheet, spriteSheet.GetFrames(32, 39)));
            animator.AddAnimation("WalkSouth", new Animation(marineSheet, spriteSheet.GetFrames(40, 47)));
            animator.AddAnimation("WalkEast", new Animation(marineSheet, spriteSheet.GetFrames(48, 55)));
            animator.AddAnimation("WalkWest", new Animation(marineSheet, spriteSheet.GetFrames(56, 63)));

            Entity.AddComponent(animator);
        }

        private void AddCollisionComponents()
        {
            MouseSelectCollider mouseCollider = new MouseSelectCollider(new Rectangle(Entity.Position.ToPoint(), new Point(19, 26)), new Vector2(-9, -13)); // new Vector2(6, 4)
            Entity.AddComponent(mouseCollider);

            Texture2D circleTex = Shapes.CreateCircleTexture(18);
            CircleCollider collider = new CircleCollider(circleTex, 9, new Vector2(0, 6));
            Entity.AddComponent(collider);
            Scene.RegisterCollider(collider);
        }

        private void AddUnitStates()
        {
            _stateMachine = new StateMachine<Marine>(this);
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Following());
            _stateMachine.SetInitialState<Idle>();
        }

        private void AddAllegiance()
        {
            var random = new Random();
            int[] filteredAllegiances = new int[] { 2, 5 };
            int index = random.Next(0, 2);
            Entity.AddComponent(new UnitAllegiance(filteredAllegiances[index]));
        }
    }
}
