using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhysicsEngine;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class PhysicsTests
    {
        private static global::PhysicsEngine.Engine DefaultPhysics()
        {
            return global::PhysicsEngine.Engine.Default();
        }

        [TestMethod]
        public void TransformationMovesWorldObjectToTheLeft()
        {
            TranslationTest(new Vector2(-1, 0));
        }

        [TestMethod]
        public void TransformationMovesWorldObjectToTheRight()
        {
            TranslationTest(new Vector2(1, 0));
        }

        private static void TranslationTest(Vector2 force)
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));
            var expected = obj.Position + force;

            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = force };
            transformation.Apply();
            obj.Position.Should().Be(expected);
        }

        [TestMethod]
        public void WorldObjectFacesRightAfterUpdateThatMovesToTheRight()
        {
            var force = new Vector2(10, 1);
            FacingTest(force, new Vector2(1, 0));
        }

        [TestMethod]
        public void WorldObjectFacesLeftAfterUpdateThatMovesToTheLeft()
        {
            var force = new Vector2(-10, 23);
            FacingTest(force, new Vector2(-1, 0));
        }

        private static void FacingTest(Vector2 force, Vector2 expected)
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = force };
            transformation.Apply();
            obj.Facing.Should().Be(expected);
        }

        [TestMethod]
        public void WorldObjectFacesSameWayAfterZeroUpdate()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(0, 1) };

            // Test when facing right
            obj.Facing = new Vector2(1, 0);
            transformation.Apply();
            obj.Facing.Should().Be(new Vector2(1, 0));

            // Test when facing left
            obj.Facing = new Vector2(-1, 0);
            transformation.Apply();
            obj.Facing.Should().Be(new Vector2(-1, 0));
        }

        [TestMethod]
        public void WorldObjectTranslatesAccordingToVelocity()
        {
            const float deltaT = 0.1f;
            const float v = 2.4f;
            var velocity = new Vector2(1, -1) * v;

            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10)) {Velocity = velocity};
            var physics = DefaultPhysics();

            physics.GenerateObjectTransformation(obj, Vector2.Zero, deltaT).PrimaryTranslation.Should().Be(velocity*deltaT);
        }

        [TestMethod]
        public void WorldObjectTranslatesAccordingToTransformation()
        {
            const float v = 2.4f;
            var startingPos = new Vector2(0, 0);
            var translation = new Vector2(1, 1) * v;
            var obj = new WorldBox(startingPos, new Vector2(10, 10));

            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = translation };

            // Translate object position
            transformation.Apply();
            obj.Position.Should().Be(startingPos + translation);
        }

        [TestMethod]
        public void WorldObjectAffectedByGravity()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));

            var gravitationalForce = WorldEnvironment.Default.Gravity.Apply(obj);
            gravitationalForce.Should().Be(new Vector2(0, -WorldEnvironment.GravitationalStrength));
        }

        [TestMethod]
        public void GravitationalAccelerationIsNotAffectedByObjectMass()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10)) { Mass = 2.0f };

            var gravitationalForce = WorldEnvironment.Default.Gravity.Apply(obj);
            gravitationalForce.Should().Be(new Vector2(0, -obj.Mass * WorldEnvironment.GravitationalStrength));
        }

        [TestMethod]
        public void WorldObjectAccelerationAffectedByObjectMass()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10)) { Mass = 1.0f };

            var mockForce = new Vector2(1, 1);
            var mockRule = new Mock<IPhysicsRule>();
            mockRule.Setup(m => m.Apply(obj)).Returns(mockForce);

            var physicsEngine = new global::PhysicsEngine.Engine(new[] { mockRule.Object });
            var transformations = physicsEngine.ProgressTime(new[] {obj}, Enumerable.Empty<ICollisionObject>(), Enumerable.Empty<ExternalForce>(), 1.0f).ToArray();

            transformations[0].TargetObject.Should().Be(obj);
            transformations[0].Acceleration.Should().Be(mockForce);

            obj.Mass = 0.5f;
            var transformations2 = physicsEngine.ProgressTime(new[] { obj }, Enumerable.Empty<ICollisionObject>(), Enumerable.Empty<ExternalForce>(), 1.0f).ToArray();
            transformations2[0].TargetObject.Should().Be(obj);
            transformations2[0].Acceleration.Should().Be(mockForce * 2.0f);
        }

    }
}
