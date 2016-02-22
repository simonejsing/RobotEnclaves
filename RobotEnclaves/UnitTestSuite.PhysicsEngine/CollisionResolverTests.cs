using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine;
using PhysicsEngine.Collision;
using UnitTestSuite.VectorMath;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class CollisionResolverTests
    {
        [TestMethod]
        public void ObjectStopsAtPrimaryCollisionPlaneWhenMakingADirectHit()
        {
            var startingPosition = new Vector2(20, 0);
            var obj = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(100, 0), new Vector2(-1, 0));
            var transformation = new ObjectTransformation(obj);
            transformation.PrimaryTranslation = new Vector2(100, 0);
            var collisions = new List<Collision>();
            collisions.AddRange(plane.CheckCollision(transformation));

            var translation = CollisionResolver.Resolve(collisions, transformation);
            VectorAssertion.Similar(new Vector2(60, 0), translation.TotalTranslation);

            translation.Apply();
            VectorAssertion.Similar(new Vector2(80, 0), obj.Position);
        }

        [TestMethod]
        public void ObjectIsTranslatedPerpendicularToPrimaryCollisionPlaneAfterHitting()
        {
            var startingPosition = new Vector2(0, 0);
            var obj = new WorldBox(startingPosition, new Vector2(20, 20));
            var plane = new CollisionPlane(new Vector2(100, 0), new Vector2(-1, 0));
            var transformation = new ObjectTransformation(obj);
            transformation.PrimaryTranslation = new Vector2(100, 10);
            var collisions = new List<Collision>();
            collisions.AddRange(plane.CheckCollision(transformation));

            var translation = CollisionResolver.Resolve(collisions, transformation);
            translation.Apply();
            obj.Position.Should().Be(new Vector2(80, 10));
        }

        [TestMethod]
        public void ObjectPerpendicularTranslationIsStoppedBySecondaryCollisionPlane()
        {
            var obj = new WorldBox(new Vector2(60, 0), new Vector2(20, 20));
            var primaryPlane = new CollisionPlane(new Vector2(100, 0), new Vector2(-1, 0));
            var secondaryPlane = new CollisionPlane(new Vector2(100, 100), new Vector2(0, -1));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(100, 100) };
            var collisions = new List<Collision>();
            collisions.AddRange(primaryPlane.CheckCollision(transformation));
            collisions.AddRange(secondaryPlane.CheckCollision(transformation));

            var translation = CollisionResolver.Resolve(collisions, transformation);
            translation.Apply();
            VectorAssertion.Similar(new Vector2(80, 80), obj.Position);
        }

        [TestMethod]
        public void PlayerShouldOnlyBeAffectedByNearestCollisionWhenThatCollisionResolvesTheEntireTranslationVector()
        {
            var startingPosition = new Vector2(410, 280);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var planeA = new CollisionPlane(new Vector2(0, 200), new Vector2(1, -4));
            var planeB = new CollisionPlane(new Vector2(0, 300), new Vector2(0, -1));

            var collisions = new List<Collision>();
            var transformation = new ObjectTransformation(player) { PrimaryTranslation = new Vector2(0, 10) };
            collisions.AddRange(planeA.CheckCollision(transformation));
            collisions.AddRange(planeB.CheckCollision(transformation));

            // Resolve collisions
            CollisionResolver.Resolve(collisions, transformation);

            VectorAssertion.Similar(startingPosition, player.Position);
        }

        [TestMethod]
        public void CollisionResolutionWhenHittingTwoPlanes()
        {
            var startingPosition = new Vector2(0, -20);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var planeA = new CollisionPlane(new Vector2(0, 0), new Vector2(1, -1));
            var planeB = new CollisionPlane(new Vector2(0, 50), new Vector2(0, -1));

            var collisions = new List<Collision>();
            var transformation = new ObjectTransformation(player) { PrimaryTranslation = new Vector2(0, 1000) };
            collisions.AddRange(planeA.CheckCollision(transformation));
            collisions.AddRange(planeB.CheckCollision(transformation));

            // Resolve collisions
            var finalTranslation = CollisionResolver.Resolve(collisions, transformation);
            VectorAssertion.Similar(new Vector2(500, 50), finalTranslation.TotalTranslation);
        }

        [TestMethod]
        public void CollisionResidualMomentumWhenCollidingWithDiagonalPlane()
        {
            var startingPosition = new Vector2(0, 0);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var planeA = new CollisionPlane(new Vector2(0, 20), new Vector2(1, -1)) { FrictionCoefficient = 1.0f };

            var collisions = new List<Collision>();
            var originalTranslation = new Vector2(0, 1000);
            var expectedMomentumAfterCollision = originalTranslation.Length/2; // We expect to loose half momentum when making a 45 degree collision
            var transformation = new ObjectTransformation(player) { PrimaryTranslation = originalTranslation };
            collisions.AddRange(planeA.CheckCollision(transformation));

            var finalTranslation = CollisionResolver.Resolve(collisions, transformation);
            
            // Momuntum should be preserved
            var residualMomentum = Math.Abs(expectedMomentumAfterCollision - finalTranslation.CollisionMomentum.Length);
            residualMomentum.Should().BeLessThan(Vector2.VectorLengthPrecission);
        }

        [TestMethod]
        public void CollisionPushingOtherBoundingPointOnToOtherPlane()
        {
            var startingPosition = new Vector2(0, -30);
            var player = new WorldBox(startingPosition, new Vector2(20, 20));
            var planeA = new CollisionPlane(new Vector2(0, 0), new Vector2(1, -1));
            var planeB = new CollisionPlane(new Vector2(50, 0), new Vector2(-1, -1));

            var collisions = new List<Collision>();
            var transformation = new ObjectTransformation(player) { PrimaryTranslation = new Vector2(0, 1000) };
            collisions.AddRange(planeA.CheckCollision(transformation));
            collisions.AddRange(planeB.CheckCollision(transformation));

            // Resolve collisions
            var finalTranslation = CollisionResolver.Resolve(collisions, transformation);
            finalTranslation.Apply();
            VectorAssertion.Similar(new Vector2(15, -5), player.Position);
        }

    }
}
