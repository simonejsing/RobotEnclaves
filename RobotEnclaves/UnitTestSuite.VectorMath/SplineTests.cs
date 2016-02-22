using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    [TestClass]
    public class SplineTests
    {
        [TestMethod]
        public void SplineWithZeroSegmentsShouldBeEmpty()
        {
            var spline = new Spline2(new Vector2(0, 0), Enumerable.Empty<Vector2>());
            spline.Segments.Should().BeEmpty();
        }

        [TestMethod]
        public void SplineWithZeroSegmentsShouldThrowIfReturnSegmentRequested()
        {
            Action action = () => new Spline2(new Vector2(0, 0), Enumerable.Empty<Vector2>(), true);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void SplineWithOneSegment()
        {
            var spline = new Spline2(new Vector2(0, 0), new [] {new Vector2(1, 0)});
            spline.Segments.Count().Should().Be(1);
            spline.Segments.First().Should().Be(new PointVector2(new Vector2(0, 0), new Vector2(1, 0)));
        }

        [TestMethod]
        public void SplineWithTwoSegments()
        {
            var spline = new Spline2(new Vector2(0, 0), new[] { new Vector2(1, 0), new Vector2(0, 1)});
            spline.Segments.Count().Should().Be(2);
            spline.Segments.Last().Should().Be(new PointVector2(new Vector2(1, 0), new Vector2(0, 1)));
        }

        [TestMethod]
        public void SplineWithReturnSegments()
        {
            var spline = new Spline2(new Vector2(0, 0), new[] { new Vector2(1, 0), new Vector2(0, 1)}, true);
            spline.Segments.Count().Should().Be(3);
            spline.Segments.Last().Should().Be(new PointVector2(new Vector2(1, 1), new Vector2(-1, -1)));
        }
    }
}
