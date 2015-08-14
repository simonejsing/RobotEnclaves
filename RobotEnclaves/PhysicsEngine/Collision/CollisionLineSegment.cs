using System;
using System.Collections.Generic;
using VectorMath;

namespace PhysicsEngine.Collision
{
    public class CollisionLineSegment : LinearCollisionObject
    {
        protected float _length;

        public PointVector2 Segment { get; private set; }

        public CollisionLineSegment(Line2 line, float segmentLength) : base(line)
        {
            _length = segmentLength;
            Segment = new PointVector2(Line.Origin, Line.Normal.Hat() * _length);
        }

        public CollisionLineSegment(PointVector2 segment)
            : base(new Line2(segment.Origin, -segment.Vector.Hat()))
        {
            _length = segment.Vector.Length;
            Segment = new PointVector2(Line.Origin, Line.Normal.Hat() * _length);
        }

        public override IEnumerable<Collision> CheckCollision(ObjectTransformation transformation)
        {
            var minimumFactor = float.PositiveInfinity;
            Vector2 minimizer = null;

            // Check if any point trajectory intersects the line segment
            foreach (var point in transformation.TargetObject.BoundingObject.Points)
            {
                var trajectoryViolation = TrajectoryViolation(new PointVector2(point, transformation.TotalTranslation), minimumFactor);
                if (float.IsPositiveInfinity(trajectoryViolation))
                    continue;

                minimumFactor = trajectoryViolation;
                minimizer = point;
            }

            if (float.IsPositiveInfinity(minimumFactor))
                yield break;

            yield return new Collision(this)
            {
                Intersection = minimumFactor * transformation.TotalTranslation,
                ClippedTranslation = new ComponentizedVector2(VectorFromOriginToProjectedDestination(new PointVector2(minimizer, transformation.TotalTranslation), Line), Line.Normal),
                Momentum = new ComponentizedVector2(transformation.TotalTranslation, Line.Normal),
                ImpactNormal = Line.Normal
            };
        }

        public override IEnumerable<Violation> CheckViolation(Object obj)
        {
            var minimumFactor = float.PositiveInfinity;

            // Check all line segments in bounding object to check if the object overlaps the line segment
            foreach (var side in obj.BoundingObject.Connections)
            {
                var sideViolationFactor = SideViolation(side, minimumFactor);
                minimumFactor = Math.Min(minimumFactor, sideViolationFactor);
            }

            if (float.IsPositiveInfinity(minimumFactor))
                yield break;

            yield return new Violation(this, -minimumFactor * Line.Normal);
        }

        private float SideViolation(PointVector2 side, float minimumFactor)
        {
            // We need to ensure that the representation of this side goes from outside to inside of segment
            var dot1 = PointViolation(side.Origin);
            var dot2 = PointViolation(side.Destination);

            // If both points are on the same side of the line then no collision can occur
            if (dot1 * dot2 > 0)
                return float.PositiveInfinity;

            // Skip if violation is less (minDot is negative)
            var minDot = Math.Min(dot1, dot2);
            if (minDot > minimumFactor)
                return float.PositiveInfinity;

            // Align side such that it goes from the point that lies inside the allowed region (e.g. dot > 0)
            var pv = dot1 > 0 ? side : side.Reverse;

            // Considering the side as a trajectory (the minimum factor is along the wrong line so we can't use it for anything, disable by setting to +inf)
            if (float.IsPositiveInfinity(TrajectoryViolation(pv, float.PositiveInfinity)))
                return float.PositiveInfinity;

            return minDot;
        }

        protected override float TrajectoryViolation(PointVector2 trajectory, float lowerBound)
        {
            var transformationLine = new Line2(trajectory.Origin, trajectory.Vector.Hat());
            var t2 = VectorLineIntersectionFactor(Segment.Origin, Segment.Vector, transformationLine);
            
            if (!IntersectionFactorTest(Segment.Vector, t2))
                return float.PositiveInfinity;
            
            // If t2 is close to 0.0 or 1.0 it means that the trajectory passed by very close to the line segment, we skip these cases to avoid sticking to cornors
            if (t2 < Vector2.VectorLengthPrecission || t2 > 1.0f - Vector2.VectorLengthPrecission)
                return float.PositiveInfinity;

            return base.TrajectoryViolation(trajectory, lowerBound);
        }
    }
}
