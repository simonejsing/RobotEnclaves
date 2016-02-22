using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine.Collision;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class BasicCollisionTests
    {
        private static readonly int seed = DateTime.Now.Millisecond;
        //private static readonly int seed = 48;
        private static readonly Random random = new Random(seed);

        private static Vector2 RandomVector()
        {
            return new Vector2((float)((random.NextDouble() - 0.5) * 1000), (float)((random.NextDouble() - 0.5) * 1000));
        }

        [TestMethod]
        public void BugRepro_VectorIntersection()
        {
            var line = new Line2(new Vector2(0, 200), new Vector2(1, -4));
            var origin = new Vector2(384.0002f, 276.0001f);
            var vector = new Vector2(-1, 111.7869f);
            
            //var intersection = CollisionObject.VectorFromOriginToProjectedDestination(origin, vector, line);
            var intersection = LinearCollisionObject.VectorFromOriginToProjectedDestination(new PointVector2(origin, vector), line);

            // Origin + intersection should now lie on the line
            float dx = (origin + intersection).X - line.Origin.X;
            float expectedY = line.Origin.Y + dx * (1.0f/4.0f);
            ((origin + intersection).Y - expectedY).Should().BeLessThan(0.0001f);
        }

        [TestMethod]
        public void VectorLineProjectionWhenVectorIsParallelToLine()
        {
            var line = new Line2(new Vector2(300, -100), new Vector2(0, -1));
            var origin = new Vector2(0, 50);
            var intersection = LinearCollisionObject.VectorFromOriginToProjectedDestination(new PointVector2(origin, new Vector2(100, 0)), line);

            // Origin + intersection should now lie on the line (e.g. Y = 0)
            ((origin + intersection).Y).Should().BeLessThan(0.0001f);
        }

        [TestMethod]
        public void VectorLineIntersectionTest()
        {
            var horizontalLine = new Line2(new Vector2(0, 0), new Vector2(0, -1));
            var point = new Vector2(0, 0);
            var vector = new Vector2(0, 1);

            LinearCollisionObject.VectorFromOriginToProjectedDestination(new PointVector2(point, vector), horizontalLine).Should().Be(new Vector2(0, 0));
        }

        [TestMethod]
        public void FuzzyVectorLineProjection()
        {
            for (var i = 0; i < 10000; i++)
            {
                var point = RandomVector();
                var vector = RandomVector();

                FuzzyVectorLineProjectionBody(point, vector);
            }
        }

        private static void FuzzyVectorLineProjectionBody(Vector2 point, Vector2 vector)
        {
            var horizontalLine = new Line2(new Vector2(0, 0), new Vector2(0, -1));
            var clippedVector = LinearCollisionObject.VectorFromOriginToProjectedDestination(new PointVector2(point, vector), horizontalLine);

            // No matter what the input is it must never have y > 0 (within rounding tolerance)
            (point + clippedVector).Y.Should().BeLessOrEqualTo(0.0001f, String.Format("Fuzzy testing seed: {0}", seed));
        }
    }
}
