using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Boids
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Boid> Boids;

        private ShapeBatch _shapeBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _shapeBatch = new ShapeBatch(GraphicsDevice, Content);
            Boids = new List<Boid>
            {
                new Boid(1, Vector2.Zero, new Vector2(2, 3)),
                new Boid(2, new Vector2(50, 100), new Vector2(2, 2)),
                new Boid(3, new Vector2(70, 200), new Vector2(2, 2)),
                new Boid(4, new Vector2(80, 300), new Vector2(2, 2)),
            };
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            UpdateBoidPositions();

            base.Update(gameTime);
        }

        private void UpdateBoidPositions()
        {
            foreach (Boid b in Boids)
            {
                var cohesion = GetCohesionVelocity(b);
                var seperation = GetSeperationVelocity(b);
                var groupV = GetGroupVelocity(b);

                b.Velocity = cohesion + seperation + groupV + b.Velocity;
                b.Position += b.Velocity;
            }
        }

        private Vector2 GetCohesionVelocity(Boid boid)
        {
            Vector2 center = Vector2.Zero;
            foreach (Boid b in Boids)
            {
                if (b.Id != boid.Id)
                {
                    center += b.Position;
                }

            }

            center /= Boids.Count - 1;

            return (center - boid.Position) / 100;
        }

        private Vector2 GetSeperationVelocity(Boid boid)
        {
            Vector2 seperationVelocity = Vector2.Zero;
            foreach (Boid b in Boids)
            {
                if (b.Id != boid.Id)
                {
                    var distance = Vector2.Distance(boid.Position, b.Position);

                    if (distance < (boid.Radius + b.Radius + 2))
                    {
                        seperationVelocity -= (b.Position - boid.Position);
                    }
                }
            }

            return seperationVelocity;
        }

        private Vector2 GetGroupVelocity(Boid boid)
        {
            Vector2 groupV = Vector2.Zero;
            foreach (Boid b in Boids)
            {
                if (b.Id != boid.Id)
                {
                    groupV += boid.Velocity;
                }
            }

            groupV /= Boids.Count - 1;

            return (groupV - boid.Velocity) / 8;
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _shapeBatch.Begin();

            foreach (Boid b in Boids)
            {
                _shapeBatch.DrawCircle(b.Position, b.Radius, Color.White, Color.Black, 1);
            }

            _shapeBatch.End();





            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
