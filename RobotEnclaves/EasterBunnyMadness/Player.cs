using PhysicsEngine.Bounding;
using VectorMath;

namespace EasterBunnyMadness
{
    class Player : PhysicsEngine.Object
    {
        private double jumpEndTime;

        public bool Jumping { get; private set; }

        public Player(Vector2 size) : base(Vector2.Zero, size)
        {
            jumpEndTime = 0.0;
            BoundingObject = BoundingPolygon.Box(new Vector2(0, 0), new Vector2(1, 1));
            HitObject = BoundingPolygon.Box(new Vector2(0, 0), new Vector2(1, 1));

            Reset();
        }

        public void Reset()
        {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Dead = false;
            Jumping = false;
            OnGround = false;
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
