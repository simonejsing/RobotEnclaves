using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine;
using PhysicsEngine.Collision;
using UnitTestSuite.VectorMath;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class LineSegmentCollisionTests
    {
        [TestMethod]
        public void LineSegmentIsCorrectlyReconstructedFromLineNormal()
        {
            var origin = new Vector2(10, 20);
            var vector = new Vector2(-15, 12);
            var lineSegment = new CollisionLineSegment(new PointVector2(origin, vector));
            VectorAssertion.Similar(origin, lineSegment.Segment.Origin);
            VectorAssertion.Similar(vector, lineSegment.Segment.Vector);
        }

        [TestMethod]
        public void ObjectWhereAllBoundingPointsTranslatesIntoLineSegmentGeneratesCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, 10), new Vector2(20, 20));
            var translation = new Vector2(100, 0);
            var transformation = new ObjectTransformation(obj) {PrimaryTranslation = translation};

            var collision = lineSegment.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(10, 0), collision.Intersection);
            VectorAssertion.Similar(new Vector2(10, 0), collision.ClippedTranslation.Vector);
        }

        [TestMethod]
        public void ObjectWithHighVelocityIsStoppedByLineSegment()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, 10), new Vector2(20, 20));
            var translation = new Vector2(10000, 0);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            var collision = lineSegment.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(10, 0), collision.Intersection);
            VectorAssertion.Similar(new Vector2(10, 0), collision.ClippedTranslation.Vector);
        }

        [TestMethod]
        public void ObjectWhereAllBoundingPointsTranslatesReverselyIntoLineSegmentGeneratesCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(-10, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, 10), new Vector2(20, 20));
            var translation = new Vector2(-100, 0);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            var collision = lineSegment.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(-10, 0), collision.Intersection);
            VectorAssertion.Similar(new Vector2(-10, 0), collision.ClippedTranslation.Vector);
        }

        [TestMethod]
        public void ObjectTranslatesDiagonallyIntoLineSegmentGeneratesCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, 10), new Vector2(20, 20));
            var translation = new Vector2(100, 100);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            var collision = lineSegment.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(10, 10), collision.Intersection);
            VectorAssertion.Similar(new Vector2(10, 100), collision.ClippedTranslation.Vector);
        }

        [TestMethod]
        public void ObjectWhereOneBoundingPointsTranslatesIntoLineSegmentGeneratesCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, -10), new Vector2(20, 20));
            var translation = new Vector2(20, 0);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            var collision = lineSegment.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(10, 0), collision.Intersection);
            VectorAssertion.Similar(new Vector2(10, 0), collision.ClippedTranslation.Vector);
        }

        [TestMethod]
        public void ObjectWhereAllBoundingPointsTranslatesHorizontallyPastLineSegmentDoesNotGenerateCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, -30), new Vector2(20, 20));
            var translation = new Vector2(20, 0);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void ObjectWhereAllBoundingPointsTranslatesDiagonallyPastLineSegmentDoesNotGenerateCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, -10), new Vector2(20, 20));
            var translation = new Vector2(20, -40);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void ObjectThatDoesNotTranslateFarEnoughToHitLineSegmentDoesNotGenerateCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(0, 10), new Vector2(20, 20));
            var translation = new Vector2(1, 1);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void ObjectThatHasOneBoundingPointOnBothSidesOfLineSegmentGeneratesViolation()
        {
            // Construct the object such that it overlaps the collidable line segment (reverse the pointvector to make the line normal point to the left)
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)).Reverse);
            var obj = new WorldBox(new Vector2(20, 10), new Vector2(20, 20));

            var violation = lineSegment.CheckViolation(obj).First();
            VectorAssertion.Similar(new Vector2(-10, 0), violation.ViolationVector);
        }

        [TestMethod]
        public void ObjectThatStartsBeyondLineSegmentDoesNotGenerateCollision()
        {
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(30, 0), new Vector2(0, 50)));
            var obj = new WorldBox(new Vector2(40, 10), new Vector2(20, 20));
            var translation = new Vector2(10, -10);
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void ObjectCloseToEdgeOfLineSegmentShouldNotJump()
        {
            // An object that is close to the edge of a line segment will cause a jumping collision
            // this problem manifests itself in cornors where two segments meat
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(0, 0), new Vector2(30, 0)));
            var obj = new WorldBox(new Vector2(30, 0), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = Vector2.Zero };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void ObjectMovingCloseByEdgeOfLineSegmentShouldNotCollide()
        {
            // An object that is close to the edge of a line segment will cause a jumping collision
            // this problem manifests itself in cornors where two segments meat
            var lineSegment = new CollisionLineSegment(new PointVector2(new Vector2(0, 0), new Vector2(30, 0)));
            var obj = new WorldBox(new Vector2(30, 0), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(-0.01f, -50) };

            lineSegment.CheckCollision(transformation).Should().BeEmpty();
        }

        [TestMethod]
        public void BugRepro_TinyTrajectoryCollisionDidNotOverrideLarger()
        {
            var collisionObject = new CollisionLineSegment(new PointVector2(new Vector2(0, 120), new Vector2(200, 0)));
            var obj = new WorldBox(new Vector2(20, 100), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(20, 20) };

            var collision = collisionObject.CheckCollision(transformation).First();
            VectorAssertion.Similar(Vector2.Zero, collision.Intersection);
        }

        [TestMethod]
        public void ObjectOnLineSegmentShouldKeepPerpendicularMomentum()
        {
            var collisionObject = new CollisionLineSegment(new PointVector2(new Vector2(0, 20), new Vector2(30, 0)));
            var obj = new WorldBox(new Vector2(10, 0), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(20, 20) };

            var collision = collisionObject.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(20, 0), collision.Momentum.PerpendicularComponent);
        }

        [TestMethod]
        public void ObjectLineSegmentCollisionMomentum()
        {
            var collisionObject = new CollisionLineSegment(new PointVector2(new Vector2(0, 100), new Vector2(300, 0)));
            var obj = new WorldBox(new Vector2(0, 80.01f), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(100, 100) };

            var collision = collisionObject.CheckCollision(transformation).First();
            VectorAssertion.Similar(new Vector2(100, 0), collision.Momentum.PerpendicularComponent);
        }

        [TestMethod]
        public void ObjectLineSegmentCollisionProjectedTranslationIsZeroWhenSlightOverlap()
        {
            var collisionObject = new CollisionLineSegment(new PointVector2(new Vector2(0, 100), new Vector2(300, 0)));
            var obj = new WorldBox(new Vector2(0, 80.1f), new Vector2(20, 20));

            var violation = collisionObject.CheckViolation(obj).First();
            VectorAssertion.Similar(new Vector2(0, -0.1f), violation.ViolationVector);
        }

        [TestMethod]
        public void ObjectShouldNotGetStuckOnCornor()
        {
            var obj = new WorldBox(new Vector2(49, -20), new Vector2(20, 20));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(10, 1) };
            var lineSegmentA = new CollisionLineSegment(new PointVector2(new Vector2(0, 0), new Vector2(50, 0)));
            var lineSegmentB = new CollisionLineSegment(new PointVector2(new Vector2(50, 0), new Vector2(0, 50)));

            var collisionA = lineSegmentA.CheckCollision(transformation).First();
            var collisionB = lineSegmentB.CheckCollision(transformation).First();

            var result = CollisionResolver.Resolve(new[] {collisionA, collisionB}, transformation);

            result.TotalTranslation.X.Should().BeGreaterThan(1);
        }

    }
}
