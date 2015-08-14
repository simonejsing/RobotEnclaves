using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

namespace PhysicsEngine.Collision
{
    public class CollisionObjectGroup : CollisionObject
    {
        private readonly List<CollisionObject> _group;

        public IEnumerable<CollisionObject> CollisionObjects { get { return _group; } } 

        public override bool Lethal
        {
            get
            {
                return _group.Any(o => o.Lethal);
            }
            set
            {
                _group.ForEach(o => o.Lethal = value);
            }
        }

        public CollisionObjectGroup(params CollisionObject[] collisionObjects)
        {
            _group = new List<CollisionObject>(collisionObjects);
        }

        public CollisionObjectGroup(IEnumerable<CollisionObject> collisionObjects) : this(collisionObjects.ToArray())
        {
        }

        public override IEnumerable<Collision> CheckCollision(ObjectTransformation transformation)
        {
            return _group.SelectMany(o => o.CheckCollision(transformation));
        }

        public override IEnumerable<Violation> CheckViolation(Object obj)
        {
            return _group.SelectMany(o => o.CheckViolation(obj));
        }
    }
}
