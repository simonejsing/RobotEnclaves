using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using VectorMath;

namespace PhysicsEngine.Collision
{
    public class ViolationResolver
    {
        public static Violation ResolveViolations(IEnumerable<Violation> violations)
        {
            var correction = Vector2.Zero;

            // Violations should be resolved in minimum order and the first violation locks in the direction of the second correction
            var primaryViolation = MinimumViolation(violations);
            if (primaryViolation == null)
                return null;

            correction += primaryViolation.ViolationVector;

            var secondaryViolation = MinimumViolation(violations.Where(v => v != primaryViolation));
            if (secondaryViolation == null)
                return primaryViolation;

            // Secondary correction must happen perpendicular to primary
            var perpendicularDirection = primaryViolation.ViolationVector.Hat().Normalize();
            correction += secondaryViolation.ViolationVector.ProjectOn(perpendicularDirection);

            return new Violation(primaryViolation.CollisionObject, correction);
        }

        private static Violation MinimumViolation(IEnumerable<Violation> violations)
        {
            var nonZeroVectors = violations.Where(v => !v.ViolationVector.Equals(Vector2.Zero));
            if (!nonZeroVectors.Any())
                return null;

            return nonZeroVectors.ArgMin(v => v.ViolationVector.LengthSquared);
        }

    }
}