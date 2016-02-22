using System;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace UnitTestSuite.VectorMath
{
    [TestClass]
    public class PolygonTests
    {
        [TestMethod]
        public void PolygonPointIntersection()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));

            // Point on border
            polygon.Intersect(new Vector2(0.4f, 0)).Should().BeTrue();

            // Point inside
            polygon.Intersect(new Vector2(0.2f, 0.1f)).Should().BeTrue();

            // Point to the left of polygon
            polygon.Intersect(new Vector2(0.1f, 0.35f)).Should().BeFalse();

            // Point to the right of polygon
            polygon.Intersect(new Vector2(0.9f, 0.35f)).Should().BeFalse();

            // Point above polygon
            polygon.Intersect(new Vector2(0.9f, -0.01f)).Should().BeFalse();

            // Point just below
            polygon.Intersect(new Vector2(0.5f, 1.5f + float.Epsilon)).Should().BeFalse();
        }

        [TestMethod]
        public void PolygonIntersectionLineStartingInsidePolygon()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));
            PolygonVectorPointIntersectionBody(polygon, new Vector2(0.5f, 0.5f), new Vector2(3, 2), true);
        }

        [TestMethod]
        public void PolygonIntersectionLineContainedInsidePolygon()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));
            PolygonVectorPointIntersectionBody(polygon, new Vector2(0.5f, 0.5f), new Vector2(0.1f, 0.1f), true);
        }

        [TestMethod]
        public void PolygonIntersectionLineEndingInsidePolygon()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));
            PolygonVectorPointIntersectionBody(polygon, new Vector2(0.5f - 3, 0.5f - 2), new Vector2(3, 2), true);
        }

        [TestMethod]
        public void PolygonIntersectionHorizontalLineGoingThroughPolygon()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));
            PolygonVectorPointIntersectionBody(polygon, new Vector2(-1f, 0.5f), new Vector2(3, 0), true);
        }

        [TestMethod]
        public void PolygonIntersectionParalleleLineStartingOutsidePolygon()
        {
            // Triangle polygon
            var polygon = new Polygon2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 1.5f));
            PolygonVectorPointIntersectionBody(polygon, new Vector2(0, 0.1f), new Vector2(10, 30), false);
        }

        private static void PolygonVectorPointIntersectionBody(Polygon2 polygon, Vector2 origin, Vector2 vector, bool expected)
        {
            polygon.Intersect(new PointVector2(origin, vector)).Should().Be(expected);
        }
    }
}
