using Microsoft.Xna.Framework;
using Rendering;

namespace RobotEnclaves.Windows10
{
    using Engine;
    using VectorMath;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private readonly MonoRenderEngine renderEngine;
        private readonly MonoKeyboardInput keyboardInput;
        private GameEngine gameEngine;

        public Game1()
        {
            this.renderEngine = new MonoRenderEngine(this);
            this.keyboardInput = new MonoKeyboardInput();
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

            var test = renderEngine.Viewport;

            this.gameEngine = GameEngine.CreateExperimentalWorld(new TopologicalUserInterface(this.renderEngine.Viewport));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            renderEngine.LoadContent();
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
            keyboardInput.Update();
            gameEngine.ProcessInput(this.keyboardInput);
            gameEngine.ProgressTime((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            gameEngine.RenderFrame(renderEngine);
            base.Draw(gameTime);
        }
    }
}
