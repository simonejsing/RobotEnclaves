using VectorMath;

namespace PhysicsEngine.Collision
{
    public abstract class LinearCollisionObject : CollisionObject
    {
        public Line2 Line { get; private set; }

        protected LinearCollisionObject(Line2 line)
        {
            Line = line;
        }

        protected virtual float TrajectoryViolation(PointVector2 trajectory, float lowerBound)
        {
            // NOTE: This is a PointVector2.Intersect(PointVector2) implementation... but we need the intersection factors
            var t1 = VectorLineIntersectionFactor(trajectory.Origin, trajectory.Vector, Line);

            // Check if the intersection point is "inside" both line segments
            if (!IntersectionFactorTest(trajectory.Vector, t1))
                return float.PositiveInfinity;

            if (t1 >= lowerBound)
                return float.PositiveInfinity;

            // If t1 is very small it means that the collision happened very close to the starting point of the trajectory (ie. we are hugging the object)
            if (t1 < Vector2.VectorLengthPrecission)
            {
                // If the trajectory moves away from the line segment we are safe and no collision can occur
                var dot = Vector2.Dot(trajectory.Origin + trajectory.Vector - Line.Origin, Line.Normal);
                if (!(dot < 0.0f))
                {
                    return float.PositiveInfinity;
                }
            }

            // Guard against tiny collisions
            //if (intersection.TooSmall())
            //    continue;

            return t1;
        }

        protected virtual float PointViolation(Vector2 point)
        {
            return Vector2.Dot(point - Line.Origin, Line.Normal);
        }

        public static Vector2 VectorFromOriginToProjectedDestination(PointVector2 trajectory, Line2 line)
        {
            // Vector that puts origin onto line while keeping the perpendicular component
            return line.DistanceVector(trajectory.Origin) + line.Projection(trajectory.Vector);
        }

        public static bool IntersectionFactorTest(Vector2 vector, float factor)
        {
            //return PointVector2.IntersectionTest(factor, vector.LengthSquared);
            return !(factor < 0.0f) && !(factor > 1.0f);
        }

        /// <summary>
        /// Computes the floating point value for where the vector intersects the line
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vector"></param>
        /// <param name="line"></param>
        /// <returns>Length factor along 'vector' where the intersection happens</returns>
        public static float VectorLineIntersectionFactor(Vector2 origin, Vector2 vector, Line2 line)
        {
            //return line.IntersectionFactor(new PointVector2(origin, vector).Line);

            // Split tranlation vector in parallel and perpendicular components to the plane
            var components = new ComponentizedVector2(vector, line.Normal);

            // If the lines are very close to being parallel we do not intersect
            if (components.NormalComponent.TooSmall())
            {
                return float.PositiveInfinity;
            }

            // Scale back on vector
            return Vector2.Dot(line.Normal, line.Origin - origin) / Vector2.Dot(line.Normal, components.Vector);
        }
    }
}
