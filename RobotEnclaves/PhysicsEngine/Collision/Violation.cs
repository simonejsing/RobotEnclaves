using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Collision
{
    public class Violation
    {
        public ICollisionObject CollisionObject;
        public Vector2 ViolationVector;

        public Violation(ICollisionObject obj, Vector2 violationVector)
        {
            CollisionObject = obj;
            ViolationVector = violationVector;
        }
    }
}
