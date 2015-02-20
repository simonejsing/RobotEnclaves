using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using Engine.Robotics;
    using Engine.Spaceship;
    using UserInput;
    using VectorMath;

    public class GameEngine
    {
        private readonly IUserInterface userInterface;

        private readonly List<Robot> robots; 
        private readonly TextBuffer console = new TextBuffer();
        private string inputString = "";

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
            this.userInterface.UpdateWorld(this.World);
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
                    this.inputString += char.ToLower(keystroke.Character);
                else if (keystroke.IsBackspace && this.inputString.Length > 0)
                    this.inputString = this.inputString.Substring(0, this.inputString.Length - 1);
                else if (keystroke.IsEnter)
                {
                    console.Add("> " + this.inputString);
                    console.AddRange(Ai.InterpretCommand(this.inputString));
                    this.inputString = "";
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
            foreach (var robot in Robots)
            {
                robot.Position += robot.Direction*robot.Engine.Speed*deltaT;
            }
        }

        public void RenderFrame(IRenderEngine renderEngine)
        {
            renderEngine.Clear();
            renderEngine.Begin();
            userInterface.SetInputText(inputString);
            userInterface.Render(renderEngine);
            renderEngine.End();
        }
    }
}
