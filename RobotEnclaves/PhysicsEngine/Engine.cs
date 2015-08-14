using System.Collections.Generic;
using System.Linq;
using PhysicsEngine.Collision;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine
{
    public class Engine : IPhysicsEngine
    {
        private readonly List<IPhysicsRule> Rules = new List<IPhysicsRule>();

        public static Engine Default()
        {
            var rules = new List<IPhysicsRule>() { WorldEnvironment.Default.Gravity };
            return new Engine(rules);
        }

        public Engine(IEnumerable<IPhysicsRule> rules)
        {
            // Setup the rules that apply in this physical world
            Rules = rules.ToList();
        }

        public IEnumerable<ObjectTransformation> ProgressTime(IEnumerable<Object> movableObjects, IEnumerable<ICollisionObject> collisionObjects, IEnumerable<ExternalForce> externalAccelerations, float deltaTime)
        {
            var transformations = new List<ObjectTransformation>();
            var finalTransformations = new List<ObjectTransformation>();

            // Test if any movable object is violating a collision object and move them back to the correct side
            // violations are assumed to be small, and we do not treat them as collisions (e.g. no collision event is raised)
            // we rather just translate the object back into place
            foreach (var obj in movableObjects)
            {
                var violation = ResolveObjectViolations(obj, collisionObjects);
                if (violation != null)
                {
                    obj.Position += violation.ViolationVector;
                }
            }

            // Evaluate all the rules of physics without changing object positions (yet)
            foreach (var obj in movableObjects)
            {
                var externalForceElement = externalAccelerations.FirstOrDefault(ea => ea.WorldObject == obj);
                var externalForce = externalForceElement == null ? Vector2.Zero : externalForceElement.Force;
                var force = externalForce + ApplyRules(obj);

                // Generate an object transformation from the velocity vector
                transformations.Add(GenerateObjectTransformation(obj, force, deltaTime));
            }

            // Resolve all object transformations
            foreach (var transformation in transformations)
            {
                var suggestedTransformation = ResolveObjectTransformation(collisionObjects, transformation);

                // Adjust the final transformation such that they do not end up violating any collision objects
                using (new TemporaryObjectTransformation(suggestedTransformation))
                {
                    var violation = ResolveObjectViolations(transformation.TargetObject, collisionObjects);
                    if(violation != null)
                    {
                        suggestedTransformation.Correction = violation.ViolationVector;
                        suggestedTransformation.ViolationOccured = true;
                        suggestedTransformation.PrimaryViolation = violation;
                    }
                }

                finalTransformations.Add(suggestedTransformation);
            }

            return finalTransformations;
        }

        public void AddRule(IPhysicsRule rule)
        {
            Rules.Add(rule);
        }

        private Violation ResolveObjectViolations(Object obj, IEnumerable<ICollisionObject> collisionObjects)
        {
            var violations = collisionObjects.SelectMany(collisionObj => collisionObj.CheckViolation(obj)).ToList();
            return ViolationResolver.ResolveViolations(violations);
        }

        private ObjectTransformation ResolveObjectTransformation(IEnumerable<ICollisionObject> collisionObjects, ObjectTransformation transformation)
        {
            // To avoid rounding issues we simply ignore very small adjustments
            if (transformation.TotalTranslation.TooSmall())
            {
                return transformation;
            }

            var collisions = ComputeCollisions(collisionObjects, transformation);

            if (!collisions.Any())
            {
                //transformation.VelocityAdjustment = transformation.PrimaryTranslation;
                return transformation;
            }

            transformation.CollisionOccured = true;
            return CollisionResolver.Resolve(collisions, transformation);
        }

        private List<Collision.Collision> ComputeCollisions(IEnumerable<ICollisionObject> collisionObjects, ObjectTransformation transformation)
        {
            // Determine collisions
            return collisionObjects.SelectMany(collisionObj => collisionObj.CheckCollision(transformation)).ToList();
        }

        public ObjectTransformation GenerateObjectTransformation(Object obj, Vector2 force, float deltaT)
        {
            var acceleration = force/obj.Mass;
            return new ObjectTransformation(obj)
            {
                Acceleration = acceleration,
                VelocityAdjustment = acceleration * deltaT,
                PrimaryTranslation = (obj.Velocity + acceleration*deltaT)*deltaT
            };
        }

        private Vector2 ApplyRules(Object obj)
        {
            return Rules.Aggregate(Vector2.Zero, (current, rule) => current + rule.Apply(obj));
        }
    }
}
