using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine;
using PhysicsEngine.Collision;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class PlaneCollisionTests
    {
        [TestMethod]
        public void CollisionWithHorizontalPlanePointingUpwards()
        {
            var startingPosition = new Vector2(100, 79);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(100, 100), new Vector2(0, -1));

            var transformation = new ObjectTransformation(player) {PrimaryTranslation = new Vector2(0, 10)};
            plane.CheckCollision(transformation).First().ClippedTranslation.Vector.Should().Be(new Vector2(0, 1));
        }

        [TestMethod]
        public void CollideReturnsNullWhenNoCollisionOccured()
        {
            var startingPosition = new Vector2(100, 50);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(100, 100), new Vector2(1, -1));

            var transformation = new ObjectTransformation(player) { PrimaryTranslation = new Vector2(-10, 10) };
            plane.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void CollisionWithDiagonalPlanePointingUpwards()
        {
            var startingPosition = new Vector2(100, 80);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var normalVector = (new Vector2(1, -1)).Normalize();
            var translation = new Vector2(0, 10);
            var plane = new CollisionPlane(new Vector2(100, 100), normalVector);
            // The colliding point is exactly on the plane, expected outcome is that it looses the normal component of the translation
            var expected = translation - normalVector * Vector2.Dot(normalVector, translation);

            var transformation = new ObjectTransformation(player) { PrimaryTranslation = translation };
            plane.CheckCollision(transformation).First().ClippedTranslation.Vector.Should().Be(expected);
        }

        [TestMethod]
        public void IntersectionWithDiagonalPlanePointingUpwards()
        {
            var startingPosition = new Vector2(10, -30);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(0, 0), new Vector2(1, -1));

            var transformation = new ObjectTransformation(player) { PrimaryTranslation = new Vector2(0, 100) };
            plane.CheckCollision(transformation).First().Intersection.Should().Be(new Vector2(0, 20));
        }
    }
}
