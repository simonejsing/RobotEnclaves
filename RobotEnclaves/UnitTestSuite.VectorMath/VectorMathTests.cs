using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    [TestClass]
    public class VectorMathTests
    {
        [TestMethod]
        public void VectorZeroReturnZeroVector()
        {
            Vector2.Zero.Should().Be(new Vector2(0, 0));
        }

        [TestMethod]
        public void VectorAddition()
        {
            var a = new Vector2(12, 34);
            var b = new Vector2(-123, 98);

            (a + b).Should().Be(new Vector2(12 - 123, 34 + 98));
        }

        [TestMethod]
        public void VectorSubtraction()
        {
            var a = new Vector2(12, 34);
            var b = new Vector2(-123, 98);
            var c = a + b;

            // If: c = a + b, then a = c - b and b = c - a
            (c - a).Should().Be(b);
            (c - b).Should().Be(a);
        }

        [TestMethod]
        public void VectorMultiplication()
        {
            var a = new Vector2(12, 34);

            (a * 5).Should().Be(new Vector2(12 * 5, 34 * 5));
        }

        [TestMethod]
        public void VectorDivision()
        {
            var a = new Vector2(12, 34);

            (a / 5).Should().Be(new Vector2(12f / 5, 34f / 5));
        }

        [TestMethod]
        public void VectorDotProduct()
        {
            var a = new Vector2(12, 34);
            var b = new Vector2(-45, 67);

            (Vector2.Dot(a, b)).Should().Be(12*(-45)+34*67);

            // Associative test
            (Vector2.Dot(a, b)).Should().Be(Vector2.Dot(b, a));
        }

        [TestMethod]
        public void VectorHat()
        {
            var a = new Vector2(12, 34);

            a.Hat().Should().Be(new Vector2(-34, 12));
        }

        [TestMethod]
        public void VectorCopyConstructor()
        {
            var a = new Vector2(12, 34);
            var b = new Vector2(a);

            b.Should().Be(new Vector2(12, 34));
            Object.ReferenceEquals(a, b).Should().BeFalse();
        }

        [TestMethod]
        public void VectorEquals()
        {
            var a = new Vector2(1, 2);
            var b = (object) new Vector2(1, 2);
            var c = (object) new Vector2(2, 3);

            a.Equals(null).Should().BeFalse();
            a.Equals((object) null).Should().BeFalse();
            a.Equals(b).Should().BeTrue();
            a.Equals(c).Should().BeFalse();
        }

        [TestMethod]
        public void VectorComparison()
        {
            var a = new Vector2(1, 2);

            a.Equals(new Vector2(3, 4)).Should().BeFalse();
            a.Equals(new Vector2(1, 4)).Should().BeFalse();
            a.Equals(new Vector2(5, 2)).Should().BeFalse();
            a.Equals(new Vector2(1, 2)).Should().BeTrue();
        }

        [TestMethod]
        public void LineNormalIsNormalized()
        {
            var line = new Line2(Vector2.Zero, new Vector2(432.89f, -123.43f));
            Math.Abs(line.Normal.Length - 1).Should().BeLessThan(Vector2.VectorLengthPrecission);
        }

        [TestMethod]
        public void VectorComponentsAreSplitCorrectly()
        {
            // Horizontal n
            ComponentTest(new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, 0), new Vector2(0, 1));
            ComponentTest(new Vector2(2, 1), new Vector2(2, 0), new Vector2(2, 0), new Vector2(0, 1));
            
            // Horizontal n (negative)
            ComponentTest(new Vector2(2, -3), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(0, -3));
            ComponentTest(new Vector2(2, -3), new Vector2(0, 5), new Vector2(0, -3), new Vector2(2, 0));

            // Vertical n
            ComponentTest(new Vector2(2, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(2, 0));

            // Diagonal n
            ComponentTest(new Vector2(2, 2), new Vector2(1, 1), new Vector2(2, 2), new Vector2(0, 0));
            ComponentTest(new Vector2(2, -2), new Vector2(1, 1), new Vector2(0, 0), new Vector2(2, -2));
        }

        private static void ComponentTest(Vector2 vector, Vector2 normal, Vector2 expectedNormal, Vector2 expectedPerpendicular)
        {
            var components = new ComponentizedVector2(vector, normal);

            // Residual should be small
            (components.NormalComponent - expectedNormal).LengthSquared.Should().BeLessThan(0.0001f);
            (components.PerpendicularComponent - expectedPerpendicular).LengthSquared.Should().BeLessThan(0.0001f);

            // Sanity check that when we add the components we get back the original
            (components.NormalComponent + components.PerpendicularComponent - vector).LengthSquared.Should().BeLessThan(0.0001f);
        }

        [TestMethod]
        public void VectorComponentsAreUpdatedWhenNormalChanges()
        {
            var components = new ComponentizedVector2(new Vector2(1, 1), new Vector2(1, 0));

            // Check that the normal and perpendicular componenets are correct
            components.NormalComponent.Should().Be(new Vector2(1, 0));
            components.PerpendicularComponent.Should().Be(new Vector2(0, 1));

            // Change normal to point along y
            components.Normal = new Vector2(0, 1);

            // Compoenents should now have "flipped"
            components.NormalComponent.Should().Be(new Vector2(0, 1));
            components.PerpendicularComponent.Should().Be(new Vector2(1, 0));
        }

        [TestMethod]
        public void VectorProjectionForNonUnitDirectionVector()
        {
            var v1 = new Vector2(2, 2);
            var v2 = new Vector2(2, 0);

            v1.ProjectionLength(v2).Should().Be(2.0f);
            VectorAssertion.Similar(new Vector2(2, 0), v1.ProjectOn(v2));
        }

        [TestMethod]
        public void VectorProjectionForDoubleLengthVector()
        {
            var v1 = new Vector2(2, 2);
            var v2 = new Vector2(1, 0);

            v1.ProjectionLength(v2).Should().Be(2.0f);
            VectorAssertion.Similar(new Vector2(2, 0), v1.ProjectOn(v2));
        }

        [TestMethod]
        public void VectorProjectionForTrippleLengthVector()
        {
            var v1 = new Vector2(3, 3);
            var v2 = new Vector2(1, 0);

            v1.ProjectionLength(v2).Should().Be(3.0f);
            VectorAssertion.Similar(new Vector2(3, 0), v1.ProjectOn(v2));
        }

        [TestMethod]
        public void OrthogonalVectorProjection()
        {
            var v1 = new Vector2(0, 1);
            var v2 = new Vector2(1, 0);

            v1.ProjectionLength(v2).Should().Be(0.0f);
            VectorAssertion.Similar(Vector2.Zero, v1.ProjectOn(v2));
        }

        [TestMethod]
        public void ParallelVectorProjection()
        {
            var v1 = new Vector2(3, 9);
            var v2 = new Vector2(1, 3);

            v1.ProjectionLength(v2).Should().Be(v1.Length);
            VectorAssertion.Similar(new Vector2(3, 9), v1.ProjectOn(v2));
        }

        [TestMethod]
        public void AngleBetweenParallelVectors()
        {
            var v1 = new Vector2(3, 9);
            var v2 = new Vector2(1, 3);

            var angle = Vector2.AngleBetween(v1, v2);
            angle.Should().Be(0);
        }

        [TestMethod]
        public void AngleBetweenOrthogonalVectors()
        {
            var v1 = new Vector2(3, 9);
            var v2 = v1.Hat();

            var angle = Vector2.AngleBetween(v1, v2);
            angle.Should().Be((float)Math.PI/2);
        }

        [TestMethod]
        public void AngleBetweenAntiOrthogonalVectors()
        {
            var v1 = new Vector2(3, 9);
            var v2 = -v1.Hat();

            var angle = Vector2.AngleBetween(v1, v2);
            angle.Should().Be((float)Math.PI/2);
        }

        [TestMethod]
        public void AngleBetweenVectorsPointOppositeDirection()
        {
            var v1 = new Vector2(3, 9);
            var v2 = new Vector2(-1, -3);

            var angle = Vector2.AngleBetween(v1, v2);
            angle.Should().Be((float)Math.PI);
        }

    }
}
