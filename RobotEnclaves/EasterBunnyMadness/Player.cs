using PhysicsEngine.Bounding;
using VectorMath;

namespace EasterBunnyMadness
{
    class Player : PhysicsEngine.Object
    {
        private double jumpEndTime;

        public bool Jumping { get; private set; }

        public Player() : base(Vector2.Zero, new Vector2(30, -30))
        {
            jumpEndTime = 0.0;
            Jumping = false;
            BoundingObject = BoundingPolygon.Box(new Vector2(0, 0), new Vector2(1, 1));
        }

        public void Update(double time)
        {
            Jumping = time < jumpEndTime;
        }

        public void Jump(double time)
        {
            jumpEndTime = time + 0.2;
            Jumping = true;
        }
    }
}
