using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using System.Linq;
    using Engine.Computer;
    using Engine.Exceptions;
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

        [TestMethod]
        public void AiCoreCanQueryRobotComponents()
        {
            var ai = new Ai();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.components()");

            result.Messages.Should().BeEquivalentTo(robot.Components.Select(c => c.Name));
        }

        [TestMethod]
        public void AiCoreCanQueryRobotProperties()
        {
            var ai = new Ai();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.properties()");

            result.Messages.Should().BeEquivalentTo(robot.Properties.Select(c => c.Name));
        }

        [TestMethod]
        public void CallingAnInvalidMethodFromTheAiWritesAnExceptionToTheConsole()
        {
            var expectedException = new InvalidRobotMethodException("incorrectMethodName");

            var ai = new Ai();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.engine.incorrectMethodName()");

            result.Success.Should().BeFalse();
            result.Messages.Should().BeEquivalentTo(expectedException.Message);
        }

        [TestMethod]
        public void ReadingAnInvalidPropertyFromTheAiWritesAnExceptionToTheConsole()
        {
            var expectedException = new InvalidRobotPropertyException("incorrectPropertyName");

            var ai = new Ai();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.engine.incorrectPropertyName");

            result.Success.Should().BeFalse();
            result.Messages.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}
