using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    [TestClass]
    public class PointVectorTests
    {
        [TestMethod]
        public void PointVectorIntersectionWhenHittingStartingPoint()
        {
            var pvA = new PointVector2(new Vector2(0, 0), new Vector2(0, 1));
            var pvB = new PointVector2(new Vector2(-1, 0), new Vector2(2, 0));

            pvA.Intersect(pvB).Should().BeTrue();
            pvB.Intersect(pvA).Should().BeTrue();
        }

        [TestMethod]
        public void PointVectorIntersectionWhenMissingStartingPoint()
        {
            var pvA = new PointVector2(new Vector2(0, 0), new Vector2(0, 1));
            var pvB = new PointVector2(new Vector2(-1, -Vector2.VectorLengthPrecission), new Vector2(2, 0));

            pvA.Intersect(pvB).Should().BeFalse();
            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void BugRepro_LineVectorIntersectionMustHaveBothPointVectorsIntersecting()
        {
            var pvA = new PointVector2(new Vector2(0, 0), new Vector2(0, 1));
            var pvB = new PointVector2(new Vector2(-1, -1), new Vector2(2, 0));

            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void PointVectorIntersectionWithOverlappingLine()
        {
            var pv = new PointVector2(new Vector2(30, 0), new Vector2(20, 0));
            var line = new Line2(new Vector2(30, 0), new Vector2(0, 1));

            pv.Intersect(line).Should().BeTrue();
        }

        [TestMethod]
        public void PointVectorIntersectionWithParallelLine()
        {
            var pv = new PointVector2(new Vector2(30, 0), new Vector2(20, 0));
            var line = new Line2(new Vector2(30, 3), new Vector2(0, 1));

            pv.Intersect(line).Should().BeFalse();
        }

        [TestMethod]
        public void PointVectorIntersectionWithPointVectorOfLengthZero()
        {
            var pvA = new PointVector2(new Vector2(30, 0), new Vector2(20, 0));
            var pvB = new PointVector2(new Vector2(21, 0), new Vector2(0, 0));

            pvA.Intersect(pvB).Should().BeFalse();
            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void PointVectorIntersectionWhenHittingEndingPoint()
        {
            var pvA = new PointVector2(new Vector2(0, 0), new Vector2(0, 1));
            var pvB = new PointVector2(new Vector2(-1, 1), new Vector2(2, 0));

            pvA.Intersect(pvB).Should().BeTrue();
            pvB.Intersect(pvA).Should().BeTrue();
        }

        [TestMethod]
        public void PointVectorIntersectionWhenMissingEndingPoint()
        {
            var pvA = new PointVector2(new Vector2(0, 0), new Vector2(0, 1));
            var pvB = new PointVector2(new Vector2(-1, 1 + Vector2.VectorLengthPrecission), new Vector2(2, 0));

            pvA.Intersect(pvB).Should().BeFalse();
            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void TwoPointVectorsOnSameLineThatAreNotOverlappingShouldNotIntersect()
        {
            var pvA = new PointVector2(new Vector2(10, 0), new Vector2(1, 0));
            var pvB = new PointVector2(new Vector2(0, 0), new Vector2(1, 0));

            pvA.Intersect(pvB).Should().BeFalse();
            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void TwoPointVectorsOnSameLineThatOverlappingShouldIntersect()
        {
            var pvA = new PointVector2(new Vector2(10, 0), new Vector2(1, 0));
            var pvB = new PointVector2(new Vector2(0, 0), new Vector2(1, 0));

            pvA.Intersect(pvB).Should().BeFalse();
            pvB.Intersect(pvA).Should().BeFalse();
        }

        [TestMethod]
        public void TwoOverlappingPointVectorsShouldIntersect()
        {
            var pvA = new PointVector2(new Vector2(10, 0), new Vector2(1, 0));
            var pvB = new PointVector2(new Vector2(10, 0), new Vector2(1, 0));

            pvA.Intersect(pvB).Should().BeTrue();
            pvB.Intersect(pvA).Should().BeTrue();
        }
    }
}
