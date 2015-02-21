﻿using System;
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
        private readonly List<string> inputHistory = new List<string>();
        private int inputHistoryIndex = -1;

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
                this.ProcessSingleKeystroke(keystroke);
            }
        }

        private void ProcessSingleKeystroke(Keystroke keystroke)
        {
            switch (keystroke.Type)
            {
                case Keystroke.KeystrokeType.Literal:
                    if (keystroke.IsValid)
                    {
                        this.inputLabel.Text += keystroke.Literal;
                    }
                    break;
                case Keystroke.KeystrokeType.Backspace:
                    if (this.inputLabel.Text.Length > 0)
                    {
                        this.inputLabel.Text = this.inputLabel.Text.Substring(0, this.inputLabel.Text.Length - 1);
                    }
                    break;
                case Keystroke.KeystrokeType.Enter:
                    this.ExecuteCommand(this.inputLabel.Text);
                    break;
                case Keystroke.KeystrokeType.Up:
                    this.GoUpInCommandHistory();
                    break;
                case Keystroke.KeystrokeType.Down:
                    this.GoDownInCommandHistory();
                    break;
            }
        }

        private void GoUpInCommandHistory()
        {
            if (inputHistoryIndex < inputHistory.Count - 1)
            {
                inputHistoryIndex++;
                PromoteHistoryToCommandLine();
            }
        }

        private void GoDownInCommandHistory()
        {
            if (inputHistoryIndex == 0)
            {
                inputHistoryIndex--;
                this.inputLabel.Text = "";
            }
            else if (inputHistoryIndex > 0)
            {
                inputHistoryIndex--;
                PromoteHistoryToCommandLine();
            }
        }

        private void PromoteHistoryToCommandLine()
        {
            if (inputHistoryIndex >= 0 && inputHistoryIndex < inputHistory.Count)
            {
                this.inputLabel.Text = inputHistory[inputHistoryIndex];
            }
        }

        private void ExecuteCommand(string command)
        {
            this.console.Add("> " + command);
            this.AddCommandToHistory(command);
            
            var result = this.Ai.InterpretCommand(command);
            this.console.AddRange(result.Messages);
            this.inputLabel.Text = "";
        }

        private void AddCommandToHistory(string command)
        {
            this.inputHistory.Insert(0, command);
            inputHistoryIndex = -1;
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
