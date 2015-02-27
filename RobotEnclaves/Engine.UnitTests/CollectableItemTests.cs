using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Exceptions;
    using Engine.Items;
    using Engine.Robotics;
    using FluentAssertions;
    using Moq;
    using VectorMath;

    [TestClass]
    public class CollectableItemTests
    {
        [TestMethod]
        public void CollectableItemIsDiscoveredWhenRobotIsInCloseProximity()
        {
            var smallDistance = new Vector2(1, 1);
            var item = new CollectableItem("cpu", "CPU 7.2 THz") {Position = Vector2.Zero };
            var robot = new Robot("AZ15") { Position = Vector2.Zero + smallDistance };

            var mockUi = new Mock<IUserInterface>();
            var gameEngine = new GameEngine(mockUi.Object);

            gameEngine.AddRobot(robot);
            gameEngine.AddItem(item);

            gameEngine.ProgressTime(0.1f);

            item.Discovered.Should().BeTrue();
        }

        [TestMethod]
        public void ACollectableItemCannotBePickedUpWhenItIsAlreadyOwnedByARobot()
        {
            var smallDistance = new Vector2(1, 1);
            var item = new CollectableItem("cpu", "CPU 7.2 THz") { Position = Vector2.Zero };
            var robotA = new Robot("AZ15") { Position = Vector2.Zero + smallDistance };
            var robotB = new Robot("AZ16") { Position = Vector2.Zero + smallDistance };

            item.SetPickedUp(robotA);
            Action action = () => robotB.Hull.Crane.PickUpItem(item);
            action.ShouldThrow<RobotException>();
        }
    }
}
