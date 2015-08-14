using VectorMath;

namespace PhysicsEngine.Interfaces
{
    public interface IPhysicsRule
    {
        Vector2 Apply(Object obj);
    }
}
