using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using System.Diagnostics;
    using System.Reflection;
    using Common;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using Engine.Storyline;
    using UserInput;
    using VectorMath;

    public class GameEngine
    {
        private const float RobotDetectionRangeSqaured = 100.0f * 100.0f;

        private readonly IUserInterface userInterface;

        private long lastFrameTime = DateTime.Now.Ticks;
        private readonly LapStopwatch stopwatch = new LapStopwatch();

        private readonly GameTimer gameTimer = new GameTimer();
        private readonly TimeCounter fpsCounter = new FrequencyTimeCounter(50);
        private readonly TimeCounter updateTimer = new AverageTimeCounter(50);
        private readonly TimeCounter renderTimer = new AverageTimeCounter(50);
        private readonly TextLabel inputLabel = new TextLabel();
        private readonly TextLabel fpsLabel = new TextLabel();
        private readonly List<string> inputHistory = new List<string>();
        private int inputHistoryIndex = -1;

        public Story Story { get; private set; }

        public Ai Ai { get; private set; }
        public World World { get; private set; }

        public GameEngine(IUserInterface userInterface)
        {
            this.World = new World();
            this.Story = new Story();
            this.Ai = new Ai(gameTimer);
            this.userInterface = userInterface;

            this.World.AddComputer(Ai.Computer);

            this.userInterface.SetConsole(this.Ai.Console);
            this.userInterface.SetInputLabel(this.inputLabel);
            this.userInterface.UpdateWorld(this.World);

            this.userInterface.AddLabel(Vector2.Zero, new Vector2(), fpsLabel);
        }

        public static GameEngine CreateExperimentalWorld(IUserInterface userInterface)
        {
            var gameEngine = new GameEngine(userInterface);
            gameEngine.Ai.Computer.Sensor = new RadarSensor() {Active = true};

            var r1 = new RepairBot("r1") { Position = new Vector2(-52.4f, 42.2f) };
            var r2 = new RepairBot("r2") { Position = new Vector2(52.4f, 127.2f) };
            var r3 = new RepairBot("r3") { Position = new Vector2(52.4f, -27.2f) };
            gameEngine.AddRobot(r1);
            gameEngine.AddRobot(r2);
            gameEngine.AddRobot(r3);

            return gameEngine;
        }

        public static GameEngine CreateTutorialWorld(IUserInterface userInterface)
        {
            var gameEngine = new GameEngine(userInterface);
            gameEngine.World.AddObject(new Spaceship.Spaceship() { Position = Vector2.Zero });
            var repairBotAz15 = new RepairBot("az15") { Position = new Vector2(-52.4f, 27.2f) };
            gameEngine.AddRobot(repairBotAz15);

            gameEngine.Story = Story.TutorialStory(gameEngine, 0.0f);

            return gameEngine;
        }

        public void WriteConsole(CommandResult result)
        {
            Ai.Console.WriteResult(result);
        }

        public void AddRobot(Robot robot)
        {
            this.Ai.AddRobot(robot);
            this.World.AddRobot(robot);
            userInterface.UpdateWorld(World);
        }

        public void DiscoverItem(CollectableItem item)
        {
            this.World.AddItem(item);
            userInterface.AddItemDiscoveredAnimation(gameTimer, item);
            userInterface.UpdateWorld(World);
            Ai.Console.WriteResult(new CommandResult(true, string.Format("Unknown object detected at ({0}, {1})", item.Position.X, item.Position.Y)));
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
            if (!Ai.Booted)
                return;

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
            this.Ai.Console.WriteResult(new CommandResult(true, "> " + command));
            this.AddCommandToHistory(command);
            
            this.Ai.ExecuteCommand(command);
            this.inputLabel.Text = "";
        }

        private void AddCommandToHistory(string command)
        {
            this.inputHistory.Insert(0, command);
            inputHistoryIndex = -1;
        }

        public void ProcessInput(ITextInput consoleInput)
        {
            stopwatch.Restart();

            // Capture user keyboard input
            var keystrokes = consoleInput.GetNewKeystrokes();
            ProcessKeystrokes(keystrokes);
        }

        public void ProgressTime(float deltaT)
        {
            this.gameTimer.Progress(deltaT);

            this.userInterface.Update(gameTimer);

            this.Story.Progress(this.gameTimer);
            this.Ai.Progress(this.gameTimer);
            foreach (var robot in World.Robots)
            {
                robot.Progress(this.gameTimer);
            }

            this.MoveRobots(deltaT);
            this.DiscoverItemInCloseProximity();

            this.updateTimer.Update(stopwatch.LapMilliseconds);
        }

        public void RenderFrame(IRenderEngine renderEngine)
        {
            userInterface.SetMapSensors(Ai.Sensor.Active);

            // Update FPS label
            fpsLabel.Text = String.Format("FPS: {0}, U/R: {1}/{2}", this.fpsCounter, this.updateTimer, this.renderTimer);
            renderEngine.Clear();
            renderEngine.Begin();
            userInterface.Render(renderEngine);
            renderEngine.End();

            this.renderTimer.Update(stopwatch.LapMilliseconds);

            this.fpsCounter.Update((DateTime.Now.Ticks - lastFrameTime) / 10000);
            lastFrameTime = DateTime.Now.Ticks;
        }

        private void DiscoverItemInCloseProximity()
        {
            foreach (var item in World.Items.Where(i => i.Discovered == false))
            {
                foreach (var robot in World.Robots)
                {
                    if ((robot.Position - item.Position).LengthSquared < RobotDetectionRangeSqaured)
                    {
                        item.SetDiscovered();
                    }
                }
            }
        }

        private void MoveRobots(float deltaT)
        {
            foreach (var robot in World.Robots)
            {
                robot.Direction = robot.Direction.Rotate(robot.Hull.Engine.RadiansPerSecond*deltaT);
                robot.Position += robot.Direction*robot.Hull.Engine.Speed*deltaT;
            }
        }
    }
}
