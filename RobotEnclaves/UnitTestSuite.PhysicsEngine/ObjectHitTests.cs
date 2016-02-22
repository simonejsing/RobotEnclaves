using System;
using System.Linq;
using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine;
using PhysicsEngine.Bounding;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class ObjectHitTests
    {
        [TestMethod]
        public void OnHitEventIsRaisedWhenTwoObjectsHaveIntersectingHitBoxes()
        {
            bool objectHitEventTriggered = false;

            // Create two objects with hitboxes
            var objectA = new HitableObject() { Position = new Vector2(0, 0) };
            var objectB = new HitableObject() { Position = new Vector2(29, 29) };
            objectB.OnHit += (o, o1) => { objectHitEventTriggered = true; };

            // Test that the physics engine fires OnHit event when movable objects overlap
            var engine = new Engine(Enumerable.Empty<IPhysicsRule>());

            var transformation = new ObjectTransformation(objectA);
            engine.ApplyTransformations(new [] {transformation}, new [] {objectA, objectB}, 0.1f);

            objectHitEventTriggered.Should().BeTrue();
        }

        [TestMethod]
        public void OnHitEventIsNotRaisedWhenTwoObjectsDoesNotHaveIntersectingHitBoxes()
        {
            bool objectHitEventTriggered = false;

            // Create two objects with hitboxes
            var objectA = new HitableObject() { Position = new Vector2(0, 0) };
            var objectB = new HitableObject() { Position = new Vector2(31, 31) };
            objectB.OnHit += (o, o1) => { objectHitEventTriggered = true; };

            // Test that the physics engine fires OnHit event when movable objects overlap
            var engine = new Engine(Enumerable.Empty<IPhysicsRule>());

            var transformation = new ObjectTransformation(objectA);
            engine.ApplyTransformations(new[] { transformation }, new[] { objectA, objectB }, 0.1f);

            objectHitEventTriggered.Should().BeFalse();
        }
    }

    public class HitableObject : global::PhysicsEngine.Object
    {
        public HitableObject() : base(Vector2.Zero, new Vector2(30, -30))
        {
            HitObject = BoundingPolygon.Box(new Vector2(0, 0), new Vector2(1, 1));
        }
    }
}
