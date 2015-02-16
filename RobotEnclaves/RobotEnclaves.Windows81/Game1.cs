using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rendering;
using Engine.World;

namespace RobotEnclaves.Windows81
{
    using Engine;
    using VectorMath;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private readonly RenderEngine renderEngine;
        private readonly WorldRenderer worldRenderer;
        private readonly World world;

        SpriteBatch spriteBatch;

        public Game1()
        {
            renderEngine = new RenderEngine(this);
            worldRenderer = new WorldRenderer(renderEngine);
            world = new World();
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
            renderEngine.Initialize();

            var robot = new Robot();
            robot.Position = renderEngine.Viewport/2;

            world.InsertObject(robot);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            renderEngine.Clear();
            renderEngine.Begin();
            worldRenderer.Render(world);
            renderEngine.End();

            base.Draw(gameTime);
        }
    }
}
