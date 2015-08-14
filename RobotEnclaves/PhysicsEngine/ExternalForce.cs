using VectorMath;

namespace PhysicsEngine
{
    public class ExternalForce
    {
        public Object WorldObject;
        public Vector2 Force;

        public ExternalForce(Object worldObject, Vector2 force)
        {
            WorldObject = worldObject;
            Force = force;
        }
    }
}
