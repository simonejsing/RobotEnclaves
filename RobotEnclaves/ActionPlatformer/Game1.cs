using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine;
using PhysicsEngine.Collision;
using PhysicsEngine.Interfaces;
using VectorMath;
using Object = PhysicsEngine.Object;
using Vector2 = VectorMath.Vector2;

namespace ActionPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private readonly IMonoInput playerInput;

        private List<Object> movableObjects;
        private List<CollisionLineSegment> collisionObjects;

        private Engine physics;
        private Renderer renderer;
        private Player player;
        private Block[] blocks;
        private Spikes[] spikes;

        public Game1()
        {
            //playerInput = new MonoKeyboardInput();
            playerInput = new MonoControllerInput(PlayerIndex.One);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            physics = Engine.Default();
            player = new Player() {Position = new Vector2(20, -150)};
            blocks = new []
            {
                new Block() {Position = TilePosition(0, 10)},
                new Block() {Position = TilePosition(1, 10)},
                new Block() {Position = TilePosition(2, 10)},
                new Block() {Position = TilePosition(3, 10)},
                new Block() {Position = TilePosition(4, 10)},

                new Block() {Position = TilePosition(8, 11)},
                new Block() {Position = TilePosition(9, 11)},
                new Block() {Position = TilePosition(10, 11)},
                new Block() {Position = TilePosition(11, 11)},
            };

            spikes = new[]
            {
                new Spikes() {Position = TilePosition(5, 11)},
                new Spikes() {Position = TilePosition(6, 11)},
                new Spikes() {Position = TilePosition(7, 11)},
            };

            movableObjects = new List<Object>() {player};
            var killLineSegment = new CollisionLineSegment(new PointVector2(TilePosition(5, 11), TilePosition(3, 0)).Reverse);
            killLineSegment.CollisionEvent += (sender, e) => { e.Target.Dead = true; };

            collisionObjects = new List<CollisionLineSegment>()
            {
                new CollisionLineSegment(new PointVector2(TilePosition(0, 10), TilePosition(5, 0)).Reverse),
                new CollisionLineSegment(new PointVector2(TilePosition(8, 11), TilePosition(4, 0)).Reverse),
                killLineSegment,
            };

            // Create a box for player 1
            /*var splineOrigin = new Vector2(0, -100);
            var splineSteps = new List<Vector2>
            {
                new Vector2(1000, 0),
                new Vector2(0, -400),
                new Vector2(-1000, 0),
            };

            var spline = new Spline2(splineOrigin, splineSteps, true);
            foreach (var segment in spline.Segments)
            {
                collisionObjects.Add(new CollisionLineSegment(segment));
            }*/

            base.Initialize();
        }

        private static Vector2 TilePosition(int x, int y)
        {
            return new Vector2(x*30,-y*30);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            renderer = new Renderer(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerInput.Update();
            player.Update(time);

            var playerForce = new List<ExternalForce>()
            {
                new ExternalForce(player, GetPlayerMovementForce(time)),
            };

            var transformations = physics.ProgressTime(movableObjects, collisionObjects, playerForce, deltaT);

            foreach (var transformation in transformations)
            {
                ApplyTransformation(transformation, deltaT);
            }

            CheckPlayerJump(time);

            base.Update(gameTime);
        }

        private void CheckPlayerJump(float time)
        {
            if (player.OnGround && playerInput.Jump())
            {
                player.Jump(time);
            }
        }

        private Vector2 GetPlayerMovementForce(double time)
        {
            Vector2 force = Vector2.Zero;
            if (playerInput.MoveLeft())
                force += new Vector2(-50, 0);
            if (playerInput.MoveRight())
                force += new Vector2(50, 0);

            if (player.Jumping)
            {
                force += new Vector2(0, 500);
            }

            return force;
        }

        private static void ApplyTransformation(ObjectTransformation transformation, float deltaTime)
        {
            transformation.Apply();

            transformation.TargetObject.OnGround = false;

            // In case of collision, update velocity vector to reflect the actual movement
            if (transformation.CollisionOccured)
            {
                transformation.PrimaryCollision.CollisionObject.OnCollision(transformation.TargetObject);

                var angleOfImpactObject = Vector2.AngleBetween(transformation.PrimaryCollision.ImpactNormal, new Vector2(0, 1));
                transformation.TargetObject.OnGround = angleOfImpactObject < Math.PI / 8;
                transformation.TargetObject.Velocity = transformation.CollisionMomentum / deltaTime;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (!player.Dead)
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Player, player.Position);
            }

            foreach (var block in blocks)
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Block, block.Position);
            }

            foreach (var spike in spikes)
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Spikes, spike.Position);
            }

            foreach (var collisionLine in collisionObjects)
            {
                renderer.DrawVector(spriteBatch, collisionLine.Segment.Origin, collisionLine.Segment.Vector, Color.Black);
            }

            renderer.RenderText(spriteBatch, Vector2.Zero, string.Format("Player: {0}, Time: {1}", player.Position, time), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RenderOpagueSprite(SpriteLibrary.SpriteIdentifier spriteIdentifier, Vector2 position)
        {
            renderer.RenderOpagueSprite(spriteBatch, spriteIdentifier, position);
        }
    }
}
