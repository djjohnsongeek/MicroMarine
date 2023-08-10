using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
            _graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Config.ScreenHeight;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _shapeBatch = new ShapeBatch(GraphicsDevice, Content);
            LoadBoids();
        }

        private void LoadBoids()
        {
            Boids = new List<Boid>();
            for (int i = 0; i < Config.BoidCount; i++)
            {
                Boids.Add(
                    new Boid(
                        id: i,
                        position: new Vector2(Calc.RandomFloat() * Config.ScreenWidth, Calc.RandomFloat() * Config.ScreenHeight),
                        velocity: new Vector2(Calc.RandomFloat() * 100, Calc.RandomFloat() * 100)
                    )
                );
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            UpdateBoidPositions(gameTime);

            base.Update(gameTime);
        }

        private void UpdateBoidPositions(GameTime gameTime)
        {
            foreach (Boid b in Boids)
            {
                var cohesionV = GetCohesionVelocity(b);
                var seperationV = GetAvoidanceVelocity(b);
                var groupV = GetGroupVelocity(b);
                var boundsV = GetBoundsVelocity(b);

                b.Velocity = cohesionV + seperationV + groupV + b.Velocity + boundsV;
                b.Velocity = ClampVelocity(b.Velocity);

                b.Position += b.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private Vector2 GetCohesionVelocity(Boid boid)
        {
            Vector2 center = Vector2.Zero;
            int count = 0;

            foreach (Boid b in Boids)
            {
                if (Vector2.DistanceSquared(boid.Position, b.Position) < Config.BoidVisionSquared)
                {
                    center += b.Position;
                    count++;
                }

            }

            center /= count;

            return (center - boid.Position) * Config.CohesionFactor;
        }

        private Vector2 GetAvoidanceVelocity(Boid boid)
        {
            Vector2 seperationVelocity = Vector2.Zero;
            // Do we need count?

            foreach (Boid b in Boids)
            {
                if (b.Id != boid.Id)
                {
                    var distance = Vector2.Distance(boid.Position, b.Position);

                    if (distance < Config.AvoidaceMinDistance)
                    {
                        seperationVelocity += (boid.Position - b.Position);
                    }
                }
            }

            return seperationVelocity * Config.AvoidanceFactor;
        }

        private Vector2 GetGroupVelocity(Boid boid)
        {
            Vector2 averageVelocity = Vector2.Zero;
            int count = 0;
            foreach (Boid b in Boids)
            {
                if (Vector2.DistanceSquared(boid.Position, b.Position) < Config.BoidVisionSquared)
                {
                    averageVelocity += boid.Velocity;
                    count++;
                }

            }

            averageVelocity /= count;

            return (averageVelocity - boid.Velocity) * Config.MatchVelocityFactor;
        }


        private Vector2 GetBoundsVelocity(Boid boid)
        {
            Vector2 turnVelocity = Vector2.Zero;

            if (boid.Position.X < Config.BoundsMargin)
            {
                turnVelocity.X += Config.BoundsTurnFactor;
            }
            if (boid.Position.X > Config.ScreenWidth - Config.BoundsMargin)
            {
                turnVelocity.X -= Config.BoundsTurnFactor;
            }
            if (boid.Position.Y < Config.BoundsMargin)
            {
                turnVelocity.Y += Config.BoundsTurnFactor;
            }
            if (boid.Position.Y > Config.ScreenHeight - Config.BoundsMargin)
            {
                turnVelocity.Y -= Config.BoundsTurnFactor;
            }


            return turnVelocity;
        }

        private Vector2 ClampVelocity(Vector2 velocity)
        {
            if (velocity.Length() > Config.MaxSpeed)
            {
                velocity.Normalize();
                velocity *= Config.MaxSpeed;
            }

            return velocity;
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
