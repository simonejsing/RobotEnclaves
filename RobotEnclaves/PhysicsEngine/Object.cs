using System;
using PhysicsEngine.Bounding;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine
{
    public abstract class Object
    {
        private Vector2 _size;
        private Vector2 _position;
        private Vector2 _facing;
        private Vector2 _acceleration;
        private Vector2 _velocity;
        private float _mass;

        public virtual Vector2 Size { get { return _size; } set { _size = value; } }
        public virtual Vector2 Position { get { return _position; } set { _position = value; } }
        public virtual Vector2 Facing { get { return _facing; } set { _facing = value; } }
        public virtual Vector2 Acceleration { get { return _acceleration; } set { _acceleration = value; } }
        public virtual Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public virtual Vector2 Center { get { return Position + Size / 2; } }

        public virtual float Mass { get { return _mass; } set { _mass = value; } }

        public bool OnGround { get; set; }

        public bool Dead { get; set; }

        // Events
        public Action<Object, Object> OnCollision;

        private IBoundingObject _relativeBoundingObject;

        public IBoundingObject BoundingObject
        {
            get { return _relativeBoundingObject.Scale(Size).Translate(Position); }
            protected set { _relativeBoundingObject = value; }
        }

        protected Object(Vector2 position, Vector2 size) : this()
        {
            _position = position;
            _size = size;
        }

        protected Object()
        {
            Dead = false;
            OnGround = false;
            _mass = 1.0f;
            _size = new Vector2(0, 0);
            _position = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _facing = new Vector2(1, 0);
            BoundingObject = new NoBounds();
        }
    }
}
