using System;
using System.Collections.Generic;
using System.Linq;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Bounding
{
    public class BoundingPolygon : BoundingObject
    {
        private Polygon2 _polygon;

        public override IEnumerable<Vector2> Points
        {
            get
            {
                return _polygon.Points;
            }
            protected set
            {
                _polygon.Points = value.ToArray();
            }
        }

        public override IEnumerable<PointVector2> Connections
        {
            get
            {
                for(var i = 0; i < _polygon.PointsArray.Length; i++)
                    for (var j = i + 1; j < _polygon.PointsArray.Length; j++)
                    {
                        var from = _polygon.PointsArray[i];
                        var to = _polygon.PointsArray[j];
                        yield return new PointVector2(from, to - from);
                    }
            }
        }

        public override IEnumerable<PointVector2> Sides {
            get
            {
                return _polygon.Sides;
            }
        }

        public BoundingPolygon(params Vector2[] points)
        {
            _polygon = new Polygon2(points);
        }

        public override IBoundingObject Translate(Vector2 vector)
        {
            return new BoundingPolygon(Points.Select(p => p + vector).ToArray());
        }

        public override IBoundingObject Scale(Vector2 scale)
        {
            return new BoundingPolygon(Points.Select(p => p * scale).ToArray());
        }

        public override bool Intersects(Vector2 point)
        {
            return _polygon.Intersect(point);
        }

        public override bool Intersects(PointVector2 vector)
        {
            return _polygon.Intersect(vector);
        }

        public override bool Intersects(IBoundingObject obj)
        {
            // We intersect if either one of our points is inside the other object, or if one of the other objects points is inside us
            if (Points.Any(obj.Intersects) || obj.Points.Any(Intersects))
                return true;

            // We intersect if any side crosses any other side in obj
            return Sides.Any(s => obj.Sides.Any(s.Intersect));
        }

        public static BoundingPolygon Box(Vector2 origin, Vector2 size)
        {
            var width = Math.Abs(size.X);
            var height = Math.Abs(size.Y);

            var points = new Vector2[4];
            points[0] = origin;
            points[1] = origin + new Vector2(width, 0);
            points[2] = origin + new Vector2(width, height);
            points[3] = origin + new Vector2(0, height);

            return new BoundingPolygon(points);
        }
    }
}
