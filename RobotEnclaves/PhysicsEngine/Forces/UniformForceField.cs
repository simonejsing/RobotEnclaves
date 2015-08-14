using VectorMath;

namespace PhysicsEngine.Forces
{
    public class UniformForceField : ForceField
    {
        private readonly Vector2 Strength;

        public UniformForceField(Vector2 strength)
        {
            Strength = strength;
        }

        public override Vector2 Apply(Object obj)
        {
            return Strength;
            //obj.Velocity += Strength * time;
        }
    }
}
