using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using System.Linq;
    using Moq;
    using Rendering;
    using VectorMath;

    [TestClass]
    public class MapRenderingTests
    {
        [TestMethod]
        public void MapRendersOnASandColoredCanvas()
        {
            var mockRenderEngine = new Mock<IRenderEngine>();
            var map = new Map(Vector2.Zero, Vector2.Zero);

            map.Render(mockRenderEngine.Object, Enumerable.Empty<IGraphics>());

            mockRenderEngine.Verify(r => r.FillRectangle(It.IsAny<Vector2>(), It.IsAny<Vector2>(), Color.Sand));
        }
    }
}
