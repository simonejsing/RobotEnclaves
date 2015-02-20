using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using System.Reflection;
    using Common;
    using Engine.Robotics;
    using Engine.Spaceship;
    using UserInput;
    using VectorMath;

    public class GameEngine
    {
        private readonly IUserInterface userInterface;

        private readonly FrequencyCounter fpsCounter = new FrequencyCounter(50);
        private readonly List<Robot> robots; 
        private readonly TextList console = new TextList();
        private readonly TextLabel inputLabel = new TextLabel();
        private readonly TextLabel fpsLabel = new TextLabel();

        public World.World World { get; private set; }

        public Ai Ai { get; private set; }
        public IEnumerable<Robot> Robots {
            get
            {
                return robots;
            }
        } 

        public GameEngine(IUserInterface userInterface)
        {
            this.World = new World.World();
            this.Ai = new Ai();
            this.robots = new List<Robot>();
            this.Ai.Boot(this.console);
            this.userInterface = userInterface;

            this.userInterface.SetConsoleBuffer(this.console);
            this.userInterface.SetInputLabel(this.inputLabel);
            this.userInterface.UpdateWorld(this.World);

            this.userInterface.AddLabel(Vector2.Zero, new Vector2(), fpsLabel);
        }

        public static GameEngine CreateTutorialWorld(IUserInterface userInterface)
        {
            var gameEngine = new GameEngine(userInterface);
            gameEngine.World.InsertObject(new Spaceship.Spaceship() { Position = Vector2.Zero });
            gameEngine.AddRobot(new Robot("AZ15") {Position = new Vector2(0f, 0f)});

            gameEngine.userInterface.UpdateWorld(gameEngine.World);

            return gameEngine;
        }

        public void AddRobot(Robot robot)
        {
            this.robots.Add(robot);
            this.Ai.AddRobot(robot);
            this.World.InsertObject(robot);
        }

        private void ProcessKeystrokes(IEnumerable<Keystroke> keystrokes)
        {
            foreach (var keystroke in keystrokes)
            {
                if (keystroke.IsValid)
                    this.inputLabel.Text += char.ToLower(keystroke.Character);
                else if (keystroke.IsBackspace && this.inputLabel.Text.Length > 0)
                    this.inputLabel.Text = this.inputLabel.Text.Substring(0, this.inputLabel.Text.Length - 1);
                else if (keystroke.IsEnter)
                {
                    console.Add("> " + this.inputLabel.Text);
                    console.AddRange(Ai.InterpretCommand(this.inputLabel.Text));
                    this.inputLabel.Text = "";
                }
            }
        }

        public void ProcessInput(ITextInput consoleInput)
        {
            // Capture user keyboard input
            var keystrokes = consoleInput.GetNewKeystrokes();
            ProcessKeystrokes(keystrokes);
        }

        public void ProgressTime(float deltaT)
        {
            fpsCounter.Update();

            foreach (var robot in Robots)
            {
                robot.Position += robot.Direction*robot.Engine.Speed*deltaT;
            }
        }

        public void RenderFrame(IRenderEngine renderEngine)
        {
            // Update FPS label
            fpsLabel.Text = String.Format("FPS: {0:0.0}", fpsCounter.Frequency);

            renderEngine.Clear();
            renderEngine.Begin();
            userInterface.Render(renderEngine);
            renderEngine.End();
        }
    }
}
