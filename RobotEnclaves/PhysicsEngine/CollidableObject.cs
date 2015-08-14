using PhysicsEngine.Interfaces;

namespace PhysicsEngine
{
    public class CollidableObject
    {
        public IBoundingObject CollisionBoundary { get; private set; }

        public CollidableObject(Object worldObject)
        {
            CollisionBoundary = worldObject.BoundingObject;
        }
    }
}
