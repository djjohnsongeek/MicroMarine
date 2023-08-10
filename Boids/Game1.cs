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
                new Boid(2, new Vector2(50, 100), new Vector2(125, 111)),
                new Boid(3, new Vector2(70, 200), new Vector2(130, 133)),
                new Boid(4, new Vector2(80, 300), new Vector2(110, 132)),
            };
            // TODO: use this.Content to load your game content here
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
                var cohesion = GetCohesionVelocity(b);
                var seperation = GetSeperationVelocity(b);
                var groupV = GetGroupVelocity(b);

                b.Velocity = cohesion + seperation + groupV + b.Velocity;

                if (b.Velocity.Length() > 200)
                {
                    b.Velocity.Normalize();
                    b.Velocity *= 200;
                }

                b.Position += b.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (b.Position.X > _graphics.PreferredBackBufferWidth)
                {
                    b.Position.X = -b.Radius;
                }

                if (b.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    b.Position.Y = -b.Radius;
                }
            }
        }

        private Vector2 GetCohesionVelocity(Boid boid)
        {
            Vector2 center = Vector2.Zero;
            float cohesionFactor = 0.005f;

            foreach (Boid b in Boids)
            {
                center += b.Position;
            }

            center /= Boids.Count;

            return (center - boid.Position) * cohesionFactor;
        }

        private Vector2 GetSeperationVelocity(Boid boid)
        {
            Vector2 seperationVelocity = Vector2.Zero;
            int minDistance = 20;
            float avoidFactor = .05f;

            foreach (Boid b in Boids)
            {
                if (b.Id != boid.Id)
                {
                    var distance = Vector2.Distance(boid.Position, b.Position);

                    if (distance < minDistance)
                    {
                        seperationVelocity += (b.Position - boid.Position);
                    }
                }
            }

            return seperationVelocity * avoidFactor;
        }

        private Vector2 GetGroupVelocity(Boid boid)
        {
            float groupingFactor = .05f;
            Vector2 averageVelocity = Vector2.Zero;
            foreach (Boid b in Boids)
            {
                averageVelocity += boid.Velocity;
            }

            averageVelocity /= Boids.Count;

            return (averageVelocity - boid.Velocity) * groupingFactor;
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
