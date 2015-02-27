﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using FluentAssertions;
    using Moq;
    using UserInput;
    using VectorMath;
    using Engine.UnitTests.Stubs;

    [TestClass]
    public class RobotTests
    {
        [TestMethod]
        public void RobotExecutesNextStatementOfCurrentProgramWhenRequestedTo()
        {
            const byte expected = 0x7F;

            var robot = new Robot("");
            var statement = new Mock<IStatement>();
            var program = new Mock<IProgram>();
            program.Setup(p => p.GetNextStatement()).Returns(statement.Object);
            statement.Setup(s => s.Execute(It.IsAny<IComputer>())).Callback((IComputer c) => c.MemoryBank.Set(0, expected));

            robot.Computer.MemoryBank.Set(0, 0);
            robot.Computer.SetCurrentProgram(program.Object);
            robot.ExecuteNextProgramStatement();

            robot.Computer.MemoryBank.GetByte(0).Should().Be(expected);
        }

        [TestMethod]
        public void ARobotWithAPositiveThrottleLevelMovesWhenTimeProgresses()
        {
            var mockUi = new Mock<IUserInterface>();
            var gameEngine = new GameEngine(mockUi.Object);

            var robot = new Robot("AZ15");
            robot.Hull.Engine.Throttle = 1.0f;
            gameEngine.AddRobot(robot);
            gameEngine.ProgressTime(1.0f);

            robot.Position.X.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void RobotWithCraneCanPickupItemsInRange()
        {
            var smallDistance = new Vector2(1,1);
            var robot = new Robot("AZ15") {Position = Vector2.Zero};
            var item = new CollectableItem("cpu", "CPU") {Position = Vector2.Zero + smallDistance};

            robot.Hull.Crane.PickUpItem(item).Should().BeTrue();
            item.Collected.Should().BeTrue();
        }

        [TestMethod]
        public void RobotWithCraneCannotePickupItemsOutOfRange()
        {
            var smallDistance = new Vector2(1, 1);
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU") { Position = robot.Position + new Vector2(robot.Hull.Crane.Range, 0) + smallDistance };

            robot.Hull.Crane.PickUpItem(item).Should().BeFalse();
            item.Collected.Should().BeFalse();
        }

        [TestMethod]
        public void WhenRobotPicksUpAnItemItShowsUpInTheCargoBay()
        {
            var smallDistance = new Vector2(1, 1);
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU") { Position = Vector2.Zero + smallDistance };

            robot.Hull.Crane.PickUpItem(item);
            robot.Hull.CargoBay.Items.Should().Contain(item);
        }

        [TestMethod]
        public void APickedUpItemHasTheLocationOfTheRobot()
        {
            var smallDistance = new Vector2(1, 1);
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU") { Position = Vector2.Zero + smallDistance };

            item.SetPickedUp(robot);
            robot.Position = new Vector2(12, 34);

            item.Position.Should().Be(robot.Position);
        }

        [TestMethod]
        public void PickedUpItemIncreasesWeightOfRobot()
        {
            const float robotBaseMass = 100.0f;
            const float itemBaseMass = 20.0f;

            var robot = new Robot("AZ15");
            var item = new CollectableItem("cpu", "CPU 1 Ghz");

            robot.BaseMass = robotBaseMass;
            item.Mass = itemBaseMass;

            robot.Hull.CargoBay.LoadItem(item);
            
            robot.Object.Mass.Should().Be(robotBaseMass + itemBaseMass);
        }

        [TestMethod]
        public void PickingUpAnItemThatWouldExceedTheRobotsCapacityThrowsException()
        {
            const float itemBaseMass = 20.0f;

            var robot = new Robot("AZ15");
            var item1 = new CollectableItem("cpu", "CPU 1 Ghz") {Mass = itemBaseMass};

            robot.Hull.CargoBay.LoadItem(item1);

            var extraRoom = robot.Hull.CargoBay.Capacity - itemBaseMass;
            var item2 = new CollectableItem("memory", "20 Mb") {Mass = extraRoom + 10.0f};

            Action action = () => robot.Hull.CargoBay.LoadItem(item2);
            action.ShouldThrow<RobotException>();
        }

        [TestMethod]
        public void RobotMassCanBeInspected()
        {
            var robot = new Robot("AZ15");
            (robot.EvaluateInstruction("mass") as ComputerTypeFloat).Value.Should().Be(robot.Object.Mass);
        }

        [TestMethod]
        public void EngineSpeedCanBeInspected()
        {
            var engine = new ProgrammableEngine();
            engine.Throttle = 1.0f;
            (engine.EvaluateInstruction("speed") as ComputerTypeFloat).Value.Should().BeGreaterThan(0.0f);
        }

        [TestMethod]
        public void RobotComponentPropertyValueCanBeSetToDecimalValue()
        {
            const string propertyName = "p";
            float value = 0f;

            var robot = new TestableRobot("AZ15");
            var component = new TestableProgrammableComponent();
            component.AddProperty(
                new ProgrammableProperty<ComputerTypeFloat>(
                    propertyName,
                    () => { return new ComputerTypeFloat(0f); },
                    ct => { value = ct.Value; }));

            robot.AddComponent(component);
            robot.EvaluateInstruction(string.Format("{0}.{1} = 1.5", component.Name, propertyName));
            value.Should().Be(1.5f);
        }

        [TestMethod]
        public void RobotPropertyValueCanBeSetToDecimalValue()
        {
            const string propertyName = "p";
            float value = 0f;

            var robot = new TestableRobot("AZ15");
            robot.AddProperty(
                new ProgrammableProperty<ComputerTypeFloat>(
                    propertyName,
                    () => { return new ComputerTypeFloat(0f); },
                    ct => { value = ct.Value; }));

            robot.EvaluateInstruction(string.Format("{0} = 1.5", propertyName));
            value.Should().Be(1.5f);
        }
    }
}
