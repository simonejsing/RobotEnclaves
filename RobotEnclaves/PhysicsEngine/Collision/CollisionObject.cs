using System.Collections.Generic;
using System.Linq;
using PhysicsEngine.Interfaces;

namespace PhysicsEngine.Collision
{
    public abstract class CollisionObject : ICollisionObject
    {
        protected float frictionCoefficient = 1.0f;

        public virtual float FrictionCoefficient
        {
            get { return frictionCoefficient; }
            set { frictionCoefficient = value; }
        }

        public virtual bool Lethal { get; set; }

        public abstract IEnumerable<Collision> CheckCollision(ObjectTransformation transformation);
        public abstract IEnumerable<Violation> CheckViolation(Object obj);

        public static ICollisionObject FromBoundingObject(IBoundingObject boundary)
        {
            return new CollisionObjectGroup(boundary.Sides.Select(s => new CollisionLineSegment(s) as CollisionObject));
        }
    }
}
