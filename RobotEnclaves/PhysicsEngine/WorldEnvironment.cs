using PhysicsEngine.Forces;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine
{
    public class WorldEnvironment
    {
        public const float GravitationalStrength = 9.82f * 10;

        public IPhysicsRule Gravity { get; set; }

        public static WorldEnvironment Default
        {
            get { return new WorldEnvironment {Gravity = new MassInvariantForce(new UniformForceField(new Vector2(0, -GravitationalStrength)))}; }
        }
    }
}
