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
    public class PlaneViolationTests
    {
        [TestMethod]
        public void ViolationReturnVectorThatResolvesTheMaximumViolation()
        {
            // Test that a bounding box with multiple points violating the plane returns a vector that is sufficient to resolve all violations
            // setup an object that is entirely on the wrong side of the plane
            var obj = new WorldBox(new Vector2(100, 100), new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(0, 0), new Vector2(0, -1));

            var violation = plane.CheckViolation(obj).First();
            VectorAssertion.Similar(new Vector2(0, -120), violation.ViolationVector);
        }

        [TestMethod]
        public void ViolationResolvesAlongPlaneNormalVector()
        {
            // Test that the violation vector is along the plane normal vector
            // setup a diagonal plane
            var normalVector = new Vector2(1, -2).Normalize();
            var obj = new WorldBox(new Vector2(100, 100), new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(0, 0), normalVector);

            var violation = plane.CheckViolation(obj).First();
            violation.ViolationVector.Length.Should().BeGreaterThan(0);
            Vector2.Dot(violation.ViolationVector, normalVector).Should().Be(violation.ViolationVector.Length);
        }

        [TestMethod]
        public void MoreThanHalfViolationReturnsViolationVectorPointingAwayFromPlane()
        {
            // This test is to prevent a bug where the violation would get the incorrect direction
            // and instead of pointing away from the plane, the object would be moved into the plane
            var startingPosition = new Vector2(10, 11);
            var obj = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(0, 20), new Vector2(0, -1));

            var violation = plane.CheckViolation(obj).First();
            VectorAssertion.Similar(new Vector2(0, -11), violation.ViolationVector);
        }
    }
}
