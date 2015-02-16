using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.UnitTests.Stubs;
    using Engine.World;
    using Moq;
    using Rendering;
    using VectorMath;



    [TestClass]
    public class WorldRenderTests
    {
        [TestMethod]
        public void WorldRendersACircleForEveryObject()
        {
            Vector2 expectedPoint = new Vector2(12.1f, 15.2f);

            var renderEngine = new Mock<IRenderEngine>();
            var world = new World();
            var obj = new StubObject { Position = expectedPoint };
            world.InsertObject(obj);

            var render = new WorldRenderer(renderEngine.Object);
            render.Render(world);

            renderEngine.Verify(r => r.DrawCircle(expectedPoint, It.IsAny<float>(), It.IsAny<Color>()), Times.Once);
        }
    }
}
