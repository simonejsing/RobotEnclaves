using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Computer;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using FluentAssertions;

    [TestClass]
    public class AiTests
    {
        [TestMethod]
        public void AiReturnsUnsuccessfulResultWhenTheSpecifiedRobotDoesNotExist()
        {
            var result = new Ai().InterpretCommand("robot.component.property = 1.0");
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public void AiCoreCanInspectRobotsCargoBay()
        {
            var ai = new Ai();
            var robot = new Robot("AZ15");
            var item = new CollectableItem("cpu", "CPU 1 Ghz");
            robot.CargoBay.LoadItem(item);
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.cargobay.items()");

            result.Messages.Should().Contain("cpu");
        }
    }
}
