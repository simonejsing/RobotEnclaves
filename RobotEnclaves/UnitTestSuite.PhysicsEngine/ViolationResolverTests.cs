using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine.Collision;
using UnitTestSuite.VectorMath;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class ViolationResolverTests
    {
        [TestMethod]
        public void ResolveEmptyListOfViolations()
        {
            var violations = new List<Violation>();
            var resolution = ViolationResolver.ResolveViolations(violations);
            resolution.Should().BeNull();
        }

        [TestMethod]
        public void ResolveZeroViolation()
        {
            var violations = new List<Violation>();
            var resolution = ViolationResolver.ResolveViolations(violations);
            violations.Add(new Violation(null, new Vector2(0, 0)));
            resolution.Should().BeNull();
        }

        [TestMethod]
        public void ResolveSingleViolation()
        {
            var v = new Vector2(10, 12);
            var violations = new List<Violation>();
            violations.Add(new Violation(null, v));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(v, resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolveDuplicateViolation()
        {
            var v1 = new Vector2(10, 12);
            var v2 = new Vector2(10, 12);
            var violations = new List<Violation>();
            violations.Add(new Violation(null, v1));
            violations.Add(new Violation(null, v2));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(v1, resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolveViolationsPointingInOppositeDirections()
        {
            var v1 = new Vector2(10, 12);
            var v2 = new Vector2(-5, -6);
            var violations = new List<Violation>();
            violations.Add(new Violation(null, v1));
            violations.Add(new Violation(null, v2));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(v2, resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolvePerpendicularViolations()
        {
            var v1 = new Vector2(10, 12);
            var v2 = v1.Hat()*0.5f;
            var violations = new List<Violation>();
            violations.Add(new Violation(null, v1));
            violations.Add(new Violation(null, v2));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(v1 + v2, resolution.ViolationVector);
        }

        private const float tiny = 0.000001f;

        [TestMethod]
        public void ResolveTinyViolation()
        {
            var violations = new List<Violation>();
            violations.Add(new Violation(null, new Vector2(tiny, 0)));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(new Vector2(tiny, 0), resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolveTwoOrthogonalTinyViolations()
        {
            var violations = new List<Violation>();
            violations.Add(new Violation(null, new Vector2(tiny, 0)));
            violations.Add(new Violation(null, new Vector2(0, tiny)));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(new Vector2(tiny, tiny), resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolveTwoTinyViolationsInSameDirection()
        {
            var violations = new List<Violation>();
            violations.Add(new Violation(null, new Vector2(tiny, 0)));
            violations.Add(new Violation(null, new Vector2(tiny, 0)));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(new Vector2(tiny, 0), resolution.ViolationVector);
        }

        [TestMethod]
        public void ResolveTwoZeroViolations()
        {
            var violations = new List<Violation>();
            violations.Add(new Violation(null, new Vector2(0, 0)));
            violations.Add(new Violation(null, new Vector2(0, 0)));
            var resolution = ViolationResolver.ResolveViolations(violations);
            resolution.Should().BeNull();
        }

        [TestMethod]
        public void ResolveZeroViolationAndNonZeroViolation()
        {
            var violations = new List<Violation>();
            violations.Add(new Violation(null, new Vector2(0, 0)));
            violations.Add(new Violation(null, new Vector2(12, 1)));
            var resolution = ViolationResolver.ResolveViolations(violations);
            VectorAssertion.Similar(new Vector2(12, 1), resolution.ViolationVector);
        }
    }
}
