using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShims;
using PhysicsEngine;
using PhysicsEngine.Bounding;
using PhysicsEngine.Collision;
using PhysicsEngine.Interfaces;
using VectorMath;
using World;
using Object = PhysicsEngine.Object;
using Vector2 = VectorMath.Vector2;

namespace EasterBunnyMadness
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private readonly MonoKeyboardInput keyboard;

        private List<Object> movableObjects;
        private List<Object> hitableObjects;
        private List<CollisionLineSegment> collisionObjects;

        private Engine physics;
        private Renderer renderer;
        private Player player;
        private int score = 0;

        private readonly TiledWorldMap TiledWorldMap = new TiledWorldMap(new Vector2(30, 30));

        public Game1()
        {
            keyboard = new MonoKeyboardInput();
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
            player = new Player() { Position = new Vector2(20, -150) };
            movableObjects = new List<Object>() { player };
            hitableObjects = new List<Object>();

            CreateMap();

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

        private void CreateMap()
        {
            // Create tiles on map
            for (int i = 0; i < 5; i++)
            {
                TiledWorldMap.AddBlock(i, 10);
            }
            for (int i = 5; i < 8; i++)
            {
                TiledWorldMap.AddSpikes(i, 11);
            }
            for (int i = 8; i < 12; i++)
            {
                TiledWorldMap.AddBlock(i, 11);
            }

            // Add coins to map
            AddCoin(11, 8);
            AddCoin(12, 8);
            AddCoin(13, 8);

            // Create collision segments
            var killLineSegment = new CollisionLineSegment(new PointVector2(TiledWorldMap.TilePosition(5, 11), TiledWorldMap.TilePosition(3, 0)).Reverse);
            killLineSegment.CollisionEvent += (sender, e) => { e.Target.Dead = true; };

            collisionObjects = new List<CollisionLineSegment>()
            {
                new CollisionLineSegment(new PointVector2(TiledWorldMap.TilePosition(0, 10), TiledWorldMap.TilePosition(5, 0)).Reverse),
                new CollisionLineSegment(new PointVector2(TiledWorldMap.TilePosition(8, 11), TiledWorldMap.TilePosition(4, 0)).Reverse),
                killLineSegment,
            };
        }

        private void AddCoin(int x, int y)
        {
            var coin = TiledWorldMap.AddCoin(x, y);
            coin.OnHit += (source, target) =>
            {
                source.Dead = true;
                score++;
            };
            hitableObjects.Add(coin);
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

            keyboard.Update();
            player.Update(time);

            var playerForce = new List<ExternalForce>()
            {
                new ExternalForce(player, GetPlayerMovementForce(time)),
            };

            var transformations = physics.ProgressTime(movableObjects, collisionObjects, playerForce, deltaT);

            physics.ApplyTransformations(transformations, hitableObjects, deltaT);

            CheckPlayerJump(time);
            RemoveCollectedCoins();

            base.Update(gameTime);
        }

        private void RemoveCollectedCoins()
        {
            foreach (var coin in TiledWorldMap.Coins.Where(c => c.Dead))
            {
                hitableObjects.Remove(coin);
            }
        }

        private void CheckPlayerJump(float time)
        {
            if (player.OnGround && keyboard.Jump())
            {
                player.Jump(time);
            }
        }

        private Vector2 GetPlayerMovementForce(double time)
        {
            Vector2 force = Vector2.Zero;
            if (keyboard.MoveLeft())
                force += new Vector2(-150, 0);
            if (keyboard.MoveRight())
                force += new Vector2(150, 0);

            if (player.Jumping)
            {
                force += new Vector2(0, 500);
            }

            return force;
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

            foreach (var block in TiledWorldMap.Blocks)
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Block, block.Position);
            }

            foreach (var spike in TiledWorldMap.Spikes)
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Spikes, spike.Position);
            }

            foreach (var coin in TiledWorldMap.Coins.Where(c => !c.Dead))
            {
                RenderOpagueSprite(SpriteLibrary.SpriteIdentifier.Coin, coin.Position);
            }

            foreach (var collisionLine in collisionObjects)
            {
                renderer.DrawVector(spriteBatch, collisionLine.Segment.Origin, collisionLine.Segment.Vector, Color.Black);
            }

            renderer.RenderText(spriteBatch, Vector2.Zero, string.Format("Score: {2}, Player: {0}, Time: {1}", player.Position, time, score), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RenderOpagueSprite(SpriteLibrary.SpriteIdentifier spriteIdentifier, Vector2 position)
        {
            renderer.RenderOpagueSprite(spriteBatch, spriteIdentifier, position);
        }
    }
}
