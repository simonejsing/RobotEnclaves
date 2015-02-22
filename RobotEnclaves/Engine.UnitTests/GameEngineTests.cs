using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using FluentAssertions;
    using Moq;
    using Rendering;
    using Rendering.Graphics;
    using VectorMath;

    [TestClass]
    public class GameEngineTests
    {
        [TestMethod]
        public void AiCoreCanInstructRobotToPickUpNamedItem()
        {
            var mockUi = new Mock<IUserInterface>();
            var gameEngine = new GameEngine(mockUi.Object);

            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var item = new CollectableItem("cpu", "CPU 7.2 THz") { Position = robot.Position };

            gameEngine.AddRobot(robot);
            gameEngine.AddItem(item);

            gameEngine.Ai.InterpretCommand("az15.crane.pickup(\"cpu\")");

            item.Collected.Should().BeTrue();
        }

    }
}
