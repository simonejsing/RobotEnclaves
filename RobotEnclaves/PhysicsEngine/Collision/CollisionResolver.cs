using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using PhysicsEngine.Interfaces;

namespace PhysicsEngine.Collision
{
    public static class CollisionResolver
    {
        public static ObjectTransformation Resolve(IEnumerable<Collision> collisions, ObjectTransformation requestedTransformation)
        {
            var transformation = new ObjectTransformation(requestedTransformation);

            // Resolve collisions in the order in which they "happen" - e.g. shortest distance
            var primaryCollision = collisions.ArgMin(c => c.Intersection.LengthSquared);
            transformation.PrimaryCollision = primaryCollision;

            //float normalMomentum = primaryCollision.CollisionObject.FrictionCoefficient * primaryCollision.VelocityAdjustment.NormalComponent.Length;
            float normalMomentum = primaryCollision.Momentum.NormalComponent.Length;
            float perpendicularMomentum = primaryCollision.Momentum.PerpendicularComponent.Length;
            float momentumScaling = transformation.TotalTranslation.Length / (normalMomentum + perpendicularMomentum);
            
            // Revert back to original behavior
            //momentumScaling = 1.0f;

            // Now we need to translate along the normal vector to the collidable object
            transformation.PrimaryTranslation = primaryCollision.ClippedTranslation.NormalComponent;
            var remainingTranslation = primaryCollision.ClippedTranslation.PerpendicularComponent;
            transformation.CollisionMomentum = remainingTranslation * momentumScaling;

            // Now we need to translate along the trajectory until we intersect with the collidable object
            /*transformation.PrimaryTranslation = primaryCollision.Intersection;
            transformation.VelocityAdjustment = transformation.PrimaryTranslation * momentumScaling;
            var remainingTranslation = primaryCollision.ClippedTranslation.Vector - primaryCollision.Intersection;*/

            // Notify the game engine that a collision occured with another object
            NotifyWorldOfCollision(transformation.TargetObject, primaryCollision.CollisionObject);

            // If the remainder is too small we are done
            if (remainingTranslation.TooSmall())
            {
                return transformation;
            }

            // Temporarily adjust object position for re-calculation of collisions
            using (new TemporaryObjectTransformation(transformation))
            {
                // Now the remaining perpendicular translation must be re-checked for collisions
                var remainingTransformation = new ObjectTransformation(transformation) { PrimaryTranslation = remainingTranslation };
                var secondaryCollisions = RecomputeCollisions(collisions.Where(c => c != primaryCollision), remainingTransformation);

                // If there are no secondary collisions we are free to move along the remaining translation vector
                if (!secondaryCollisions.Any())
                {
                    transformation.SecondaryTranslation = remainingTranslation;
                    return transformation;
                }

                // Secondary collision is projected along the vector because the primary collision restricted movement to be along the plane
                var secondaryCollision = secondaryCollisions.ArgMin(c => c.Intersection.LengthSquared);
                var secondaryTranslation = secondaryCollision.ClippedTranslation.Vector;

                transformation.SecondaryCollision = secondaryCollision;

                // Notify the game engine that a collision occured with another object
                NotifyWorldOfCollision(transformation.TargetObject, secondaryCollision.CollisionObject);

                if (secondaryTranslation.TooSmall())
                {
                    return transformation;
                }

                transformation.SecondaryTranslation = secondaryTranslation;
                transformation.CollisionMomentum = transformation.SecondaryTranslation * momentumScaling;
                return transformation;
            }
        }

        private static void NotifyWorldOfCollision(Object obj, ICollisionObject collisionObj)
        {
            if (obj.OnCollision != null)
            {
                //obj.OnCollision(obj, collisionObj);
                obj.OnCollision(obj, null);
            }
        }

        private static IEnumerable<Collision> RecomputeCollisions(IEnumerable<Collision> collisions, ObjectTransformation transformation)
        {
            return collisions.SelectMany(c => c.CollisionObject.CheckCollision(transformation)).ToList();
        }
    }
}
