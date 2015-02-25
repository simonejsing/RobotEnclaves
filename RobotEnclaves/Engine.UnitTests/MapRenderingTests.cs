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
    using Rendering.Widgets;
    using VectorMath;

    [TestClass]
    public class MapRenderingTests
    {
        [TestMethod]
        public void MapRendersOnASandColoredCanvas()
        {
            var mockRenderEngine = new Mock<IRenderEngine>();
            var map = new Map(Vector2.Zero, Vector2.Zero);

            map.Render(mockRenderEngine.Object);

            mockRenderEngine.Verify(r => r.FillRectangle(It.IsAny<Vector2>(), It.IsAny<Vector2>(), Color.Sand));
        }

        [TestMethod]
        public void DiscoveredItemsRendersTextLabelOnMap()
        {
            const string name = "name";
            const string label = "label";

            var world = new World();
            var item = new CollectableItem(name, label) {Position = Vector2.Zero};
            world.AddItem(item);
            var mockRenderEngine = new Mock<IRenderEngine>();
            var itemWidget = new CollectableItemSprite(item);

            item.SetDiscovered();
            itemWidget.Render(mockRenderEngine.Object);

            mockRenderEngine.Verify(r => r.DrawText(It.IsAny<Vector2>(), name + ":" + label, It.IsAny<Color>()), Times.Once);
        }

        [TestMethod]
        public void CollectedItemsDoNotRenderOnMap()
        {
            var item = new CollectableItem("", "") { Position = Vector2.Zero };
            var itemWidget = new CollectableItemSprite(item);

            item.SetPickedUp(new Robot(""));
            itemWidget.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void ItemsWithinRangeOfRobotRendersInGreen()
        {
            const string name = "name";
            const string label = "label";

            var mockRenderEngine = new Mock<IRenderEngine>();

            var world = new World();
            var robot = new Robot("AZ15") { Position = Vector2.Zero };
            var distance = robot.Crane.Range - 0.1f;
            var item = new CollectableItem(name, label) { Position = robot.Position + UnitVector2.GetInstance(1, 0) * distance };
            item.SetDiscovered();

            world.AddRobot(robot);
            world.AddItem(item);

            var widget = new CollectableItemSprite(item);
            widget.Render(mockRenderEngine.Object);
            mockRenderEngine.Verify(r => r.DrawText(It.IsAny<Vector2>(), name + ":" + label, Color.Green), Times.Once);
        }

        [TestMethod]
        public void ErrorMessagesRendersAsRedTextInConsole()
        {
            const string errorMessage = "some error";

            var console = new GameConsole();
            var widget = new Console(Vector2.Zero, new Vector2(100, 100));
            widget.SetConsole(console);
            
            console.WriteResult(new CommandResult(false, errorMessage));
            var mockRenderEngine = new Mock<IRenderEngine>();
            widget.Render(mockRenderEngine.Object);

            mockRenderEngine.Verify(r => r.DrawText(It.IsAny<Vector2>(), errorMessage, Color.Red), Times.Once);
        }
    }
}
