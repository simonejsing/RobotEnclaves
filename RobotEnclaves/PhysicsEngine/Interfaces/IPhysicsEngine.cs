using System.Collections.Generic;

namespace PhysicsEngine.Interfaces
{
    public interface IPhysicsEngine
    {
        IEnumerable<ObjectTransformation> ProgressTime(IEnumerable<Object> movableObjects, IEnumerable<ICollisionObject> collisionObjects, IEnumerable<ExternalForce> externalAccelerations, float deltaTime);
        void AddRule(IPhysicsRule rule);
    }
}
