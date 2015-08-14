using System;
using VectorMath;

namespace PhysicsEngine
{
    public class TemporaryObjectTransformation : IDisposable
    {
        private readonly Object _worldObj;
        private readonly Vector2 _originalPosition;
        private readonly Vector2 _originalFacing;
        private readonly Vector2 _originalVelocity;
        private readonly Vector2 _originalAcceleration;

        public TemporaryObjectTransformation(ObjectTransformation source)
        {
            _worldObj = source.TargetObject;
            _originalPosition = _worldObj.Position;
            _originalFacing = _worldObj.Facing;
            _originalVelocity = _worldObj.Velocity;
            _originalAcceleration = _worldObj.Acceleration;

            source.Apply();
        }

        public void Dispose()
        {
            _worldObj.Position = _originalPosition;
            _worldObj.Facing = _originalFacing;
            _worldObj.Velocity = _originalVelocity;
            _worldObj.Acceleration = _originalAcceleration;
        }
    }
}
