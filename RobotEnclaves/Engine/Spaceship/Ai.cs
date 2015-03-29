using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Spaceship
{
    using Common;
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Storyline;
    using UserInput;

    public class Ai : ProgrammableComponentBase, IRobot
    {
        private GameTimer timer;
        private Story story;
        private List<Robot> OwnedRobots = new List<Robot>();
        private readonly GameConsole console = new GameConsole();

        public bool Booted { get; set; }


        public IComputer Computer { get; private set; }
        public IHull Hull { get; private set; }
        public IObject Object { get; private set; }

        public override string Name
        {
            get { return "Oddysey"; }
            protected set { }
        }

        public IEnumerable<IProgram> Programs
        {
            get
            {
                return Enumerable.Empty<IProgram>();
            }
        }

        public GameConsole Console
        {
            get
            {
                return console;
            }
        }

        public IProgram CurrentProgram
        {
            get
            {
                return null;
            }
        }

        public ISensor Sensor
        {
            get
            {
                return Computer.Sensor;
            }
        }

        public IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                return Enumerable.Empty<IProgrammableComponent>();
            }
        }

        public Ai(GameTimer timer)
        {
            this.story = new Story();
            this.timer = timer;
            this.Hull = null;
            this.Object = new WorldObject(0f);
            this.Computer = new Computer(this.Object, this, "HardCore");
        }

        public Robot FindRobotByName(string name)
        {
            return OwnedRobots.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRobot(Robot r)
        {
            OwnedRobots.Add(r);
            this.Computer.AddProxyComponents(new []{new ProgrammableComputerWrapper(r.Name, r.Computer)});
        }

        public void ExecuteCommand(string command)
        {
            var result = this.InterpretCommand(command);
            Console.WriteResult(result);
        }

        public CommandResult InterpretCommand(string command)
        {
            try
            {
                var result = new CommandResult(true);
                var robotResult = this.Computer.EvaluateInstruction(command);
                if (!(robotResult is ComputerTypeVoid))
                {
                    result.AddMessages(
                        robotResult.ToString().Split(
                            new[] { Environment.NewLine },
                            StringSplitOptions.RemoveEmptyEntries));
                }

                return result;
            }
            catch (RobotException rex)
            {
                return new CommandResult(false, rex.Message);
            }
        }

        public void Progress(GameTimer gameTimer)
        {
            this.story.Progress(gameTimer);
        }

        public float Reboot(float startTime)
        {
            var t = startTime;
            Console.Clear();
            Booted = false;

            Computer.ApplyUpgrades();
            Computer.Sensor.Active = false;

            story.Clear();
            story.AddEvents(Story.AiBootEvents(this, ref t));

            return t;
        }
    }
}
