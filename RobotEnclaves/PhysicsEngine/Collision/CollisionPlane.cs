using System.Collections.Generic;
using VectorMath;

namespace PhysicsEngine.Collision
{
    public class CollisionPlane : LinearCollisionObject
    {
        public CollisionPlane(Vector2 positionVector, Vector2 normalVector)
            : base(new Line2(positionVector, normalVector))
        {
            frictionCoefficient = 0.5f;
        }

        public override IEnumerable<Collision> CheckCollision(ObjectTransformation transformation)
        {
            var translation = transformation.TotalTranslation;
            var minimumFactor = float.PositiveInfinity;
            Vector2 minimizer = null;

            // Check all points in the bounding box
            foreach (var point in transformation.TargetObject.BoundingObject.Points)
            {
                var trajectoryViolation = TrajectoryViolation(new PointVector2(point, translation), minimumFactor);
                if (float.IsPositiveInfinity(trajectoryViolation))
                    continue;

                minimumFactor = trajectoryViolation;
                minimizer = point;
            }
            
            if (float.IsPositiveInfinity(minimumFactor))
                yield break;

            yield return new Collision(this)
            {
                Intersection = minimumFactor * translation,
                ClippedTranslation = new ComponentizedVector2(VectorFromOriginToProjectedDestination(new PointVector2(minimizer, translation), Line), Line.Normal),
                Momentum = new ComponentizedVector2(translation, Line.Normal),
                ImpactNormal = Line.Normal
            };
        }

        public override IEnumerable<Violation> CheckViolation(Object obj)
        {
            var minimumFactor = float.PositiveInfinity;

            // Check if any point is on the wrong side of the plane
            foreach (var point in obj.BoundingObject.Points)
            {
                var dot = PointViolation(point);
                if (dot >= 0.0f || dot > minimumFactor)
                    continue;

                minimumFactor = dot;
            }

            if (float.IsPositiveInfinity(minimumFactor))
                yield break;

            // Guard against tiny overlaps
            var intersection = -minimumFactor * Line.Normal;
            if (intersection.TooSmall())
                yield break;

            yield return new Violation(this, intersection);
        }
    }
}
