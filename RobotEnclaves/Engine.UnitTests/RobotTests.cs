using System;
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

            robot.MemoryBank.Set(0, 0);
            robot.CurrentProgram = program.Object;
            robot.ExecuteNextProgramStatement();

            robot.MemoryBank.GetByte(0).Should().Be(expected);
        }

        [TestMethod]
        public void AiCoreCanInterpretCommandToChangeRobotsThrottleLevel()
        {
            var ai = new Ai();
            var robot = new Robot("AZ15");
            ai.AddRobot(robot);

            ai.InterpretCommand("AZ15.engine.throttle = 1.0");

            robot.Engine.Throttle.Should().Be(1.0f);
        }

        [TestMethod]
        public void ARobotWithAPositiveThrottleLevelMovesWhenTimeProgresses()
        {
            var mockUi = new Mock<IUserInterface>();
            var gameEngine = new GameEngine(mockUi.Object);

            var robot = new Robot("AZ15");
            robot.Engine.Throttle = 1.0f;
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

            robot.Crane.PickUpItem(item).Should().BeTrue();
            item.Collected.Should().BeTrue();
        }

        [TestMethod]
        public void RobotWithCraneCannotePickupItemsOutOfRange()
        {
            var smallDistance = new Vector2(1, 1);
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU") { Position = robot.Position + new Vector2(robot.Crane.Range, 0) + smallDistance };

            robot.Crane.PickUpItem(item).Should().BeFalse();
            item.Collected.Should().BeFalse();
        }

        [TestMethod]
        public void WhenRobotPicksUpAnItemItShowsUpInTheCargoBay()
        {
            var smallDistance = new Vector2(1, 1);
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU") { Position = Vector2.Zero + smallDistance };

            robot.Crane.PickUpItem(item);
            robot.CargoBay.Items.Should().Contain(item);
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

            robot.CargoBay.LoadItem(item);
            
            robot.Mass.Should().Be(robotBaseMass + itemBaseMass);
        }

        [TestMethod]
        public void PickingUpAnItemThatWouldExceedTheRobotsCapacityThrowsException()
        {
            const float robotBaseMass = 100.0f;
            const float itemBaseMass = 20.0f;

            var robot = new Robot("AZ15");
            var item1 = new CollectableItem("cpu", "CPU 1 Ghz") {Mass = itemBaseMass};

            robot.CargoBay.LoadItem(item1);

            var extraRoom = robot.CargoBay.Capacity - itemBaseMass;
            var item2 = new CollectableItem("memory", "20 Mb") {Mass = extraRoom + 10.0f};

            Action action = () => robot.CargoBay.LoadItem(item2);
            action.ShouldThrow<RobotException>();
        }
    }
}
