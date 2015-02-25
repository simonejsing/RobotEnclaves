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
    using Engine.Robotics;
    using UserInput;

    public class Ai
    {
        private List<Robot> OwnedRobots = new List<Robot>();

        public Robot FindRobotByName(string name)
        {
            return OwnedRobots.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRobot(Robot r)
        {
            OwnedRobots.Add(r);
        }

        public CommandResult Boot()
        {
            const string consoleInitialContent =
@"Hard-Core OS v" + Version.Text + @" booting...
BIOS check... OK
RAM check... FATAL ERROR
Peripheral sensor calibration... FATAL ERROR

FATAL ERRORS ENCOUNTERED!

Entering recovery mode...

Available RAM: 0 MB

Systems online";
            return new CommandResult(true, consoleInitialContent);
        }

        public CommandResult InterpretCommand(string command)
        {
            var result = new CommandResult(true);

            if (command.Equals("robots()", StringComparison.OrdinalIgnoreCase))
            {
                result.AddMessages(this.OwnedRobots.Select(r => r.Name));
                return result;
            }

            var tokens = command.Split(new[] {'.'}, 2);
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
                }
                catch (RobotException rex)
                {
                    return new CommandResult(false, rex.Message);
                }
            }

            return new CommandResult(false, "Invalid instruction");
        }
    }
}
