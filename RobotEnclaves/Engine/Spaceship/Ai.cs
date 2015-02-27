﻿using System;
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

    public class Ai : IRobot
    {
        private GameTimer timer;
        private Story story;
        private List<Robot> OwnedRobots = new List<Robot>();
        private readonly GameConsole console = new GameConsole();

        public bool Booted { get; set; }


        public IComputer Computer { get; private set; }
        public IHull Hull { get; private set; }
        public IObject Object { get; private set; }

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
            this.Computer = new Computer("HardCore");
            this.Object = new WorldObject(0f);
        }

        public Robot FindRobotByName(string name)
        {
            return OwnedRobots.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRobot(Robot r)
        {
            OwnedRobots.Add(r);
        }

        public void ExecuteCommand(string command)
        {
            var result = this.InterpretCommand(command);
            Console.WriteResult(result);
        }

        public CommandResult InterpretCommand(string command)
        {
            var result = new CommandResult(true);

            if (command.Equals("robots()", StringComparison.OrdinalIgnoreCase))
            {
                result.AddMessages(this.OwnedRobots.Select(r => r.Name));
                return result;
            }

            if (command.Equals("reboot()", StringComparison.OrdinalIgnoreCase))
            {
                Reboot();
                return result;
            }

            var tokens = command.Split(new[] { '.' }, 2);
            if (tokens.Length > 1)
            {
                var robotName = tokens[0];
                var instruction = tokens[1];

                var robot = this.FindRobotByName(robotName);
                if (robot == null)
                {
                    var noSuchRobotResult = new CommandResult(false);
                    noSuchRobotResult.AddMessages(String.Format("Robot with name '{0}' does not exist.", robotName));
                    return noSuchRobotResult;
                }

                try
                {
                    var robotResult = robot.EvaluateInstruction(instruction);
                    if (!(robotResult is ComputerTypeVoid))
                    {
                        result.AddMessages(
                            robotResult.ToString().Split(
                                new[] {Environment.NewLine},
                                StringSplitOptions.RemoveEmptyEntries));
                    }

                    return result;
                }
                catch (RobotException rex)
                {
                    return new CommandResult(false, rex.Message);
                }
            }

            return new CommandResult(false, "Invalid instruction");
        }

        public void Progress(GameTimer gameTimer)
        {
            this.story.Progress(gameTimer);
        }

        public void Reboot()
        {
            var t = timer.TotalSeconds;
            Console.Clear();
            Booted = false;

            Computer.ApplyUpgrades();

            story.AddEvents(Story.AiBootEvents(this, ref t));
        }
    }
}
