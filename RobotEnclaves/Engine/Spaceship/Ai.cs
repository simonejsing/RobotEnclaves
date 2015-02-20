using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Spaceship
{
    using Common;
    using Engine.Robotics;
    using UserInput;

    public class Ai
    {
        private List<Robot> OwnedRobots = new List<Robot>();

        public Robot FindRobotByName(string name)
        {
            return OwnedRobots.First(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRobot(Robot r)
        {
            OwnedRobots.Add(r);
        }

        public void Boot(TextList console)
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
            console.AddRange(
                consoleInitialContent.Split(new[] { Environment.NewLine },
                StringSplitOptions.None));
        }

        public IEnumerable<string> InterpretCommand(string command)
        {
            if (command == "robots")
            {
                return this.OwnedRobots.Select(r => r.Name);
            }

            var tokens = command.Split(new[] {'.'}, 2);
            if (tokens.Length > 1)
            {
                var robotName = tokens[0];
                var instruction = tokens[1];

                var robot = this.FindRobotByName(robotName);
                robot.ExecuteStatement(instruction);
            }

            return Enumerable.Empty<string>();
        }
    }
}
