using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Forces
{
    class MassInvariantForce : IPhysicsRule
    {
        private readonly IPhysicsRule _gravityField;

        public MassInvariantForce(IPhysicsRule gravityField)
        {
            _gravityField = gravityField;
        }

        public Vector2 Apply(Object obj)
        {
            return obj.Mass*_gravityField.Apply(obj);
        }
    }
}
