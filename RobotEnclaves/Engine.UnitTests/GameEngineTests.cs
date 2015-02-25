using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Engine.Exceptions;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using Engine.UnitTests.Stubs;
    using FluentAssertions;
    using Moq;
    using Rendering;
    using Rendering.Graphics;
    using UserInput;
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

        [TestMethod]
        public void RobotCanChangeDirectionBySteeringLeft()
        {
            var mockUi = new Mock<IUserInterface>();
            var gameEngine = new GameEngine(mockUi.Object);

            var robot = new Robot("AZ15") { Position = Vector2.Zero, Direction = UnitVector2.GetInstance(1, 0)};
            gameEngine.AddRobot(robot);

            gameEngine.Ai.InterpretCommand("az15.engine.steering = 1.0");
            gameEngine.ProgressTime(0.1f);

            robot.Direction.Y.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void AiErrorMessagesGeneratesErrorTextInConsole()
        {
            const string errorMessage = "some error";

            var robot = new TestableRobot("az15");
            robot.AddMethod(new ProgrammableMethod("foo", ct => { throw new RobotException(errorMessage); }));

            IGameConsole console = null;

            var mockInput = new Mock<ITextInput>();
            mockInput.Setup(m => m.GetNewKeystrokes()).Returns(Keystroke.FromString("az15.foo()\n"));
            var mockUi = new Mock<IUserInterface>();
            mockUi.Setup(m => m.SetConsole(It.IsAny<IGameConsole>())).Callback<IGameConsole>(c => console = c);
            var gameEngine = new GameEngine(mockUi.Object);
            console.Should().NotBeNull("GameEngine is supposed to set the console of the UI");
            
            gameEngine.AddRobot(robot);
            gameEngine.ProcessInput(mockInput.Object);

            console.Lines.Last().Key.Should().Be(errorMessage);
            console.Lines.Last().Value.Should().Be(false);
        }
    }
}
