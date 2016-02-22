using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    [TestClass]
    public class LineTests
    {
        private static void ResidualTest(float value, float expected)
        {
            Math.Abs(value - expected).Should().BeLessThan(Vector2.VectorLengthPrecission);
        }

        [TestMethod]
        public void IntersectionFactorOfLinesMeetingAtCenter()
        {
            var lineA = new Line2(new Vector2(0, 0), new Vector2(-1, 1));
            var lineB = new Line2(new Vector2(1, 0), new Vector2(-1, -1));

            float intersectionFactorA = lineA.IntersectionFactor(lineB);
            float intersectionFactorB = lineB.IntersectionFactor(lineA);
            ResidualTest(intersectionFactorA, (float)Math.Sqrt(2)/2);
            ResidualTest(intersectionFactorB, (float)Math.Sqrt(2)/2);
        }

        [TestMethod]
        public void IntersectionFactorOfLinesMeetingOfCenter()
        {
            var lineA = new Line2(new Vector2(0, 0.5f), new Vector2(-1, 1));
            var lineB = new Line2(new Vector2(1, 0), new Vector2(-1, -1));

            float intersectionFactorA = lineA.IntersectionFactor(lineB);
            float intersectionFactorB = lineB.IntersectionFactor(lineA);

            // We expect:
            // intersectionA: x > 0 && x < length
            // intersectionB: y > 0 && y < length
            // -y > x
            intersectionFactorA.Should().BePositive();
            intersectionFactorA.Should().BeLessThan((float)Math.Sqrt(2));
            intersectionFactorB.Should().BePositive();
            intersectionFactorB.Should().BeLessThan((float)Math.Sqrt(2));
            intersectionFactorA.Should().BeLessThan(intersectionFactorB);
        }

        [TestMethod]
        public void IntersectionFactorOfParallelLines()
        {
            var lineA = new Line2(new Vector2(0, 0), new Vector2(1, 1));
            var lineB = new Line2(new Vector2(1, 0), new Vector2(1, 1));

            float intersectionFactorA = lineA.IntersectionFactor(lineB);
            float intersectionFactorB = lineB.IntersectionFactor(lineA);
            float.IsPositiveInfinity(intersectionFactorA).Should().BeTrue();
            float.IsPositiveInfinity(intersectionFactorB).Should().BeTrue();
        }

        [TestMethod]
        public void IntersectionFactorOfOverlappingLines()
        {
            var lineA = new Line2(new Vector2(0, 0), new Vector2(1, 1));
            var lineB = new Line2(new Vector2(0, 0), new Vector2(1, 1));

            float intersectionFactorA = lineA.IntersectionFactor(lineB);
            float intersectionFactorB = lineB.IntersectionFactor(lineA);
            float.IsNaN(intersectionFactorA).Should().BeTrue();
            float.IsNaN(intersectionFactorB).Should().BeTrue();
        }
    }
}
