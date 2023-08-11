﻿using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Myra;
using Myra.Graphics2D.UI;
using System.IO;
using System;

namespace Boids
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Desktop _desktop;

        List<Boid> Boids;
        float BoidsRadiusAverage;

        private Waypoint Waypoint;

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
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;
            _graphics.ApplyChanges();
            Boids = new List<Boid>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _shapeBatch = new ShapeBatch(GraphicsDevice, Content);

            Waypoint = new Waypoint(Vector2.Zero, 5f);
            LoadBoids();
            LoadUIAndSettings();
        }

        private void LoadBoids()
        {
            BoidsRadiusAverage = 0;

            for (int i = 0; i < Config.BoidCount; i++)
            {
                Boids.Add(
                    new Boid(
                        id: i,
                        position: new Vector2(Calc.RandomFloat() * Config.ScreenWidth, Calc.RandomFloat() * Config.ScreenHeight),
                        velocity: Vector2.Zero
                    )
                );

                BoidsRadiusAverage += Boids[i].Radius;

                // new Vector2(Calc.RandomFloat() * 1000, Calc.RandomFloat() * 1000)
            }
            BoidsRadiusAverage /= Boids.Count;
        }

        private void LoadUIAndSettings()
        {
            MyraEnvironment.Game = this;
            string importData = File.ReadAllText("ui.xml");
            var project = Project.LoadFromXml(importData, MyraEnvironment.DefaultAssetManager);

            HorizontalSlider cohesionSlider = project.Root.FindWidgetById("CohesionSlider") as HorizontalSlider;
            Label cohesionValueLabel = project.Root.FindWidgetById("CohesionValueLabel") as Label;
            cohesionSlider.ValueChanged += (src, value) =>
            {
                cohesionValueLabel.Text = value.NewValue.ToString();
                Config.CohesionFactor = value.NewValue;
            };

            HorizontalSlider avoidanceSlider = project.Root.FindWidgetById("AvoidanceSlider") as HorizontalSlider;
            Label avoidanceValueLabel = project.Root.FindWidgetById("AvoidanceValueLabel") as Label;
            avoidanceSlider.ValueChanged += (src, value) =>
            {
                avoidanceValueLabel.Text = value.NewValue.ToString();
                Config.AvoidanceFactor = value.NewValue;
            };

            HorizontalSlider avoidanceDistSlider = project.Root.FindWidgetById("AvoidanceDistSlider") as HorizontalSlider;
            Label avoidanceDistValueLabel = project.Root.FindWidgetById("AvoidanceDistValueLabel") as Label;
            avoidanceDistSlider.ValueChanged += (src, value) =>
            {
                avoidanceDistValueLabel.Text = value.NewValue.ToString();
                Config.AvoidanceMinDist = value.NewValue;
            };

            HorizontalSlider groupAlignmentSlider = project.Root.FindWidgetById("GroupAlignmentSlider") as HorizontalSlider;
            Label groupAlignmentValueLabel = project.Root.FindWidgetById("GroupAlignmentValueLabel") as Label;
            groupAlignmentSlider.ValueChanged += (src, value) =>
            {
                groupAlignmentValueLabel.Text = value.NewValue.ToString();
                Config.GroupAlignmentFactor = value.NewValue;
            };

            HorizontalSlider boundsMarginSlider = project.Root.FindWidgetById("BoundsMarginSlider") as HorizontalSlider;
            Label boundsMarginValueLabel = project.Root.FindWidgetById("BoundsMarginValueLabel") as Label;
            boundsMarginSlider.ValueChanged += (src, value) =>
            {
                boundsMarginValueLabel.Text = value.NewValue.ToString();
                Config.BoundsMargin = value.NewValue;
            };

            HorizontalSlider boundsRepelSlider = project.Root.FindWidgetById("BoundsRepelSlider") as HorizontalSlider;
            Label boundsRepelValueLabel = project.Root.FindWidgetById("BoundsRepelValueLabel") as Label;
            boundsRepelSlider.ValueChanged += (src, value) =>
            {
                boundsRepelValueLabel.Text = value.NewValue.ToString();
                Config.BoundRepelFactor = value.NewValue;
            };

            HorizontalSlider boidVisionSlider = project.Root.FindWidgetById("BoidVisionSlider") as HorizontalSlider;
            Label boidVisionValueLabel = project.Root.FindWidgetById("BoidVisonValueLabel") as Label;
            boidVisionSlider.ValueChanged += (src, value) =>
            {
                boidVisionValueLabel.Text = value.NewValue.ToString();
                Config.BoidVision = value.NewValue;
            };

            HorizontalSlider maxSpeedSlider = project.Root.FindWidgetById("MaxSpeedSlider") as HorizontalSlider;
            Label maxSpeedValueLabel = project.Root.FindWidgetById("MaxSpeedValueLabel") as Label;
            maxSpeedSlider.ValueChanged += (src, value) =>
            {
                maxSpeedValueLabel.Text = value.NewValue.ToString();
                Config.MaxSpeed = value.NewValue;
            };

            HorizontalSlider boidCountSlider = project.Root.FindWidgetById("BoidCountSlider") as HorizontalSlider;
            Label boidCountValueLabel = project.Root.FindWidgetById("BoidCountValueLabel") as Label;
            boidCountSlider.ValueChanged += (src, value) =>
            {
                boidCountValueLabel.Text = ((int)value.NewValue).ToString();
                Config.BoidCount = (int)value.NewValue;
            };

            HorizontalSlider destinationAlignmentSlider = project.Root.FindWidgetById("DestinationAlignmentSlider") as HorizontalSlider;
            Label destAlignmentValueLabel = project.Root.FindWidgetById("DestinationAlignmentValueLabel") as Label;
            destinationAlignmentSlider.ValueChanged += (src, value) =>
            {
                destAlignmentValueLabel.Text = value.NewValue.ToString();
                Config.DestinationFactor = value.NewValue;
            };

            var resetBtn = project.Root.FindWidgetById("ResetBtn") as ImageTextButton;
            resetBtn.Click += (src, value) =>
            {
                Boids.Clear();
                LoadBoids();
            };

            var toggleCollisionsBtn = project.Root.FindWidgetById("ToggleCollisonsBtn") as ImageTextButton;
            toggleCollisionsBtn.Click += (src, value) =>
            {
                Config.CollisionsEnabled = !Config.CollisionsEnabled;
                toggleCollisionsBtn.Text = Config.CollisionsEnabled ? "True" : "False";
            };

            var exportBtn = project.Root.FindWidgetById("ExportBtn") as ImageTextButton;
            exportBtn.Click += (src, value) =>
            {
                string exportString = "";
                exportString += $"CohesionFactor: {Config.CohesionFactor}\n";
                exportString += $"AvoidanceFactor: {Config.AvoidanceFactor}\n";
                exportString += $"AvoidanceMinDistance: {Config.AvoidanceMinDist}\n";
                exportString += $"BoundMargin: {Config.BoundsMargin}\n";
                exportString += $"BoundsRepelFactor: {Config.BoundRepelFactor}\n";
                exportString += $"GroupAlignmentFactor: {Config.GroupAlignmentFactor}\n";
                exportString += $"DestinationFactor: {Config.DestinationFactor}\n";
                exportString += $"MaxSpeed: {Config.MaxSpeed}\n";
                exportString += $"CollisionsEnabled: {Config.CollisionsEnabled}\n";
                exportString += $"BoidCount: {Config.BoundRepelFactor}\n";
                exportString += $"BoidVision: {Config.BoidVision}\n";
                File.WriteAllText("configExport.txt", exportString);
            };

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = project.Root;
            _desktop.Root.Visible = false;
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);

            if (Input.KeyWasPressed(Keys.Escape))
            {
                Exit();
            }

            if (Input.KeyWasPressed(Keys.Tab))
            {
                _desktop.Root.Visible = !_desktop.Root.Visible;
            }

            if (Input.LeftMouseBtnWasPressed() && !_desktop.Root.Visible)
            {
                Waypoint.Position = Input.MousePosition;
                Waypoint.Enabled = true;
                Waypoint.Radius = BoidsRadiusAverage;

                SetBoidsWaypoint(true);
            }
            else if (Input.RightMouseBtnWasPressed() && !_desktop.Root.Visible)
            {
                Waypoint.Position = Vector2.Zero;
                Waypoint.Enabled = false;

                SetBoidsWaypoint(false);
            }

            UpdateBoids(gameTime);
            if (Config.CollisionsEnabled)
            {
                Physics.ResolveCollisions(Boids);
            }
            base.Update(gameTime);
        }

        private void UpdateBoids(GameTime gameTime)
        {
            foreach (Boid b in Boids)
            {
                if (!b.Idle)
                {
                    var cohesionV = GetCohesionVelocity(b);
                    var seperationV = GetAvoidanceVelocity(b);
                    var groupV = GetGroupVelocity(b);
                    var boundsV = GetBoundsVelocity(b);
                    var destinationV = GetDestinationVelocity(b);

                    b.Velocity = cohesionV + seperationV + groupV + b.Velocity + boundsV + destinationV;

                    if (BoidArrived(b))
                    {
                        b.Idle = true;
                        b.Waypoint = null;
                        b.Velocity = Vector2.Zero;
                        Waypoint.Radius += (float)Math.Cbrt(BoidsRadiusAverage);
                    }


                    b.Velocity = ClampVelocity(b.Velocity);

                    b.Position += b.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private bool BoidArrived(Boid b)
        {
            return Vector2.DistanceSquared(b.Position, Waypoint.Position) < Waypoint.Radius * Waypoint.Radius;
        }

        private void SetBoidsWaypoint(bool active)
        {
            foreach (var boid in Boids)
            {
                if (active)
                {
                    boid.Waypoint = Waypoint;
                    boid.Idle = false;
                }
                else
                {
                    boid.Waypoint = null;
                    boid.Idle = true;
                }

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
                    var avoidDistance = b.Radius + boid.Radius + Config.AvoidanceMinDist;

                    if (distance < avoidDistance)
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

            return (averageVelocity - boid.Velocity) * Config.GroupAlignmentFactor;
        }

        private Vector2 GetBoundsVelocity(Boid boid)
        {
            Vector2 turnVelocity = Vector2.Zero;

            if (boid.Position.X < Config.BoundsMargin)
            {
                turnVelocity.X += Config.BoundRepelFactor;
            }
            if (boid.Position.X > Config.ScreenWidth - Config.BoundsMargin)
            {
                turnVelocity.X -= Config.BoundRepelFactor;
            }
            if (boid.Position.Y < Config.BoundsMargin)
            {
                turnVelocity.Y += Config.BoundRepelFactor;
            }
            if (boid.Position.Y > Config.ScreenHeight - Config.BoundsMargin)
            {
                turnVelocity.Y -= Config.BoundRepelFactor;
            }


            return turnVelocity;
        }

        private Vector2 GetDestinationVelocity(Boid boid)
        {
            var destVelocity = Vector2.Zero;
            if (Waypoint.Enabled)
            {
                destVelocity = Waypoint.Position - boid.Position;
                destVelocity.Normalize();
                destVelocity *= Config.MaxSpeed;
            }

            return destVelocity * Config.DestinationFactor;
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


            if (Waypoint.Enabled)
            {
                _shapeBatch.DrawCircle(Waypoint.Position, Waypoint.Radius, Color.Transparent, Color.White);
            }
            _desktop.Render();

            foreach (Boid b in Boids)
            {
                _shapeBatch.DrawCircle(b.Position, b.Radius, b.Color, b.BorderColor, 1);
            }

            _shapeBatch.DrawRectangle(Config.BoundsOrigin, Config.BoundsSize, Color.Transparent, Color.White);


            _shapeBatch.End();





            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
