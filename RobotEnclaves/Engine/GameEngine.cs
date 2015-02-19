using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using UserInput;

    public class GameEngine
    {
        private readonly IRenderEngine renderEngine;
        private readonly IUserInterface userInterface;
        private readonly ITextInput consoleInput;
        private readonly SpaceshipAi Ai;

        public GameEngine(IRenderEngine renderEngine, IUserInterface userInterface, ITextInput consoleInput)
        {
            this.Ai = new SpaceshipAi();
            this.renderEngine = renderEngine;
            this.userInterface = userInterface;
            this.consoleInput = consoleInput;

            World.World world = TestWorld.Generate();
            this.userInterface.SetConsoleBuffer(Ai.Console);
            this.userInterface.SetActiveWorld(world);
        }

        public void Process()
        {
            // Capture user keyboard input
            var keystrokes = consoleInput.GetNewKeystrokes();
            Ai.ProcessKeystrokes(keystrokes);
        }

        public void RenderFrame()
        {
            renderEngine.Clear();
            renderEngine.Begin();
            userInterface.SetInputText(Ai.Input);
            userInterface.Render(renderEngine);
            renderEngine.End();
        }
    }
}
