using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Collision
{
    public class Collision
    {
        public ICollisionObject CollisionObject { get; private set; }
        public Vector2 Intersection { get; set; }
        public ComponentizedVector2 ClippedTranslation { get; set; }
        public ComponentizedVector2 Momentum { get; set; }
        public Vector2 ImpactNormal { get; set; }

        public Collision(ICollisionObject collisionObject)
        {
            CollisionObject = collisionObject;
        }
    }
}
