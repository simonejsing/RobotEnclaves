using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using System.Linq;
    using Common;
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using Engine.Storyline;
    using Engine.UnitTests.Stubs;
    using FluentAssertions;
    using Moq;
    using UserInput;
    using VectorMath;

    [TestClass]
    public class AiTests
    {
        private static Ai DefaultAi()
        {
            return new Ai(new GameTimer());
        }

        [TestMethod]
        public void AiCoreCanInterpretCommandToChangeRobotsThrottleLevel()
        {
            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            ai.InterpretCommand("AZ15.engine.throttle = 1.0");

            robot.Hull.Engine.Throttle.Should().Be(1.0f);
        }

        [TestMethod]
        public void AiReturnsUnsuccessfulResultWhenTheSpecifiedRobotDoesNotExist()
        {
            var result = DefaultAi().InterpretCommand("robot.component.property = 1.0");
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public void AiCoreCanInspectRobotsCargoBay()
        {
            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            var item = new CollectableItem("cpu", "CPU 1 Ghz");
            robot.Hull.CargoBay.LoadItem(item);
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.cargobay.items()");

            result.Messages.Should().Contain(item.ToString());
        }

        [TestMethod]
        public void AiCoreCanQueryRobotComponents()
        {
            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.components()");

            result.Messages.Should().BeEquivalentTo(robot.Components.Select(c => c.Name));
        }

        [TestMethod]
        public void AiCoreCanQueryRobotProperties()
        {
            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.properties()");

            result.Messages.Should().BeEquivalentTo(robot.Properties.Select(c => c.Name));
        }

        [TestMethod]
        public void CallingAnInvalidMethodFromTheAiWritesAnExceptionToTheConsole()
        {
            var expectedException = new InvalidRobotMethodException("incorrectMethodName");

            var ai = DefaultAi();
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

            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            var result = ai.InterpretCommand("AZ15.engine.incorrectPropertyName");

            result.Success.Should().BeFalse();
            result.Messages.Should().BeEquivalentTo(expectedException.Message);
        }

        [TestMethod]
        public void RobotCanInstallItemInAi()
        {
            var world = new World();
            var ai = DefaultAi();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            world.AddComputer(ai.Computer);
            world.AddRobot(robot);
            robot.SetCurrentWorld(world);

            var item = new SensorUpgrade(new RadarSensor(), "item", "label");
            robot.Hull.CargoBay.LoadItem(item);
            robot.EvaluateInstruction("install(\"item\",\"HardCore\")");

            ai.Computer.PendingUpgrades.Should().Contain(item);
        }

        [TestMethod]
        public void RobotCannotInstallItemInAiIfOutOfRange()
        {
            var world = new World();
            var ai = DefaultAi();
            var robot = new Robot("AZ15") {Position = new Vector2(100, 100)};
            ai.AddRobot(robot);

            world.AddComputer(ai.Computer);
            world.AddRobot(robot);
            robot.SetCurrentWorld(world);

            var item = new SensorUpgrade(new RadarSensor(), "item", "label");
            robot.Hull.CargoBay.LoadItem(item);
            Action action = () => robot.EvaluateInstruction("install(\"item\",\"HardCore\")");
            action.ShouldThrow<RobotException>();
            ai.Computer.PendingUpgrades.Should().NotContain(item);
            robot.Hull.CargoBay.Items.Should().Contain(item);
        }

        [TestMethod]
        public void AiInstallsPendingUpgradesOnReboot()
        {
            var ai = DefaultAi();
            var sensor = new RadarSensor();
            var item = new SensorUpgrade(sensor, "item", "label");
            ai.Computer.InstallUpgrade(item);
            ai.Reboot(0);
            ai.Computer.Sensor.Should().Be(sensor);
        }
    }
}
