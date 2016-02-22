using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine.Collision;
using UnitTestSuite.VectorMath;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class LineSegmentViolationTests
    {
        [TestMethod]
        public void ViolationReturnVectorThatResolvesTheMaximumViolation()
        {
            // Test that a bounding box with multiple points violating the line segment returns a vector that is sufficient to resolve all violations
            var obj = new WorldBox(new Vector2(50 - 1, -50 - 1), new Vector2(20, 20));
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(0, 0), new Vector2(100, -100)));

            var violation = lineSegment.CheckViolation(obj).First();
            VectorAssertion.Similar(new Vector2(-19, -19), violation.ViolationVector);
        }

        [TestMethod]
        public void ViolationResolvesAlongLineSegmentNormalVector()
        {
            // Test that the violation vector is along the line segment normal vector
            var obj = new WorldBox(new Vector2(50 - 1, -50 - 1), new Vector2(20, 20));
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(0, 0), new Vector2(100, -100)));

            var violation = lineSegment.CheckViolation(obj).First();
            violation.ViolationVector.Length.Should().BeGreaterThan(0);
            Vector2.Dot(violation.ViolationVector, lineSegment.Line.Normal).Should().Be(violation.ViolationVector.Length);
        }
    }
}
