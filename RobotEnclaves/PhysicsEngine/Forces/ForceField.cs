using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Forces
{
    public abstract class ForceField : IPhysicsRule
    {
        public abstract Vector2 Apply(Object obj);
    }
}
