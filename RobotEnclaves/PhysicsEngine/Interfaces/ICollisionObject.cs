using System;
using System.Collections.Generic;
using PhysicsEngine.Collision;

namespace PhysicsEngine.Interfaces
{
    public delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

    public interface ICollisionObject
    {
        float FrictionCoefficient { get; }
        bool Lethal { get; set; }
        IEnumerable<Collision.Collision> CheckCollision(ObjectTransformation transformation);
        IEnumerable<Violation> CheckViolation(Object obj);

        void OnCollision(Object target);

        event CollisionEventHandler CollisionEvent;
    }
}