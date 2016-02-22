using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    public static class VectorAssertion
    {
        public static void Similar(Vector2 expected, Vector2 actual, float maximumResidualSquared = Vector2.VectorLengthPrecission)
        {
            Assert.IsTrue((expected - actual).LengthSquared < Vector2.VectorLengthPrecission, String.Format("Expected vector {0} to be {1} within r^2 of {2}", actual, expected, maximumResidualSquared));
        }

        public static void AreNearlyEqual(float expected, float actual, float tolerance = Vector2.VectorLengthPrecission)
        {
            Math.Abs(expected - actual).Should().BeLessThan(tolerance);
        }
    }
}
