using VectorMath;

namespace PhysicsEngine
{
    public class ObjectTransformation
    {
        public Object TargetObject { get; private set; }
        public Vector2 PrimaryTranslation { get; set; }
        public Vector2 SecondaryTranslation { get; set; }
        public Vector2 Correction { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 VelocityAdjustment { get; set; }
        public Vector2 CollisionMomentum { get; set; }

        public Vector2 TotalTranslation
        {
            get { return PrimaryTranslation + SecondaryTranslation + Correction; }
        }

        public bool CollisionOccured { get; set; }
        public bool ViolationOccured { get; set; }
        public Collision.Collision PrimaryCollision { get; set; }
        public Collision.Collision SecondaryCollision { get; set; }
        public Collision.Violation PrimaryViolation { get; set; }

        public ObjectTransformation(ObjectTransformation source)
        {
            this.TargetObject = source.TargetObject;
            this.PrimaryTranslation = source.PrimaryTranslation;
            this.SecondaryTranslation = source.SecondaryTranslation;
            this.Correction = source.Correction;
            this.VelocityAdjustment = source.VelocityAdjustment;
            this.CollisionMomentum = source.CollisionMomentum;
            this.Acceleration = source.Acceleration;
            this.CollisionOccured = source.CollisionOccured;
            this.ViolationOccured = source.ViolationOccured;
            this.PrimaryCollision = source.PrimaryCollision;
            this.SecondaryCollision = source.SecondaryCollision;
            this.PrimaryViolation = source.PrimaryViolation;
        }

        public ObjectTransformation(Object obj)
        {
            TargetObject = obj;
            PrimaryTranslation = new Vector2(0, 0);
            SecondaryTranslation = new Vector2(0, 0);
            Correction = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            VelocityAdjustment = new Vector2(0, 0);
            CollisionMomentum = new Vector2(0, 0);
            CollisionOccured = false;
            ViolationOccured = false;
            PrimaryCollision = null;
            SecondaryCollision = null;
            PrimaryViolation = null;
        }

        public void Apply()
        {
            TargetObject.Acceleration = Acceleration;
            TargetObject.Velocity += VelocityAdjustment;

            var totalTranslation = TotalTranslation;
            if (totalTranslation.TooSmall())
                return;
            
            TargetObject.Position += totalTranslation;
            if (totalTranslation.X > 0)
            {
                TargetObject.Facing = new Vector2(1, 0);
            }
            else if (totalTranslation.X < 0)
            {
                TargetObject.Facing = new Vector2(-1, 0);
            }
        }
    }
}
