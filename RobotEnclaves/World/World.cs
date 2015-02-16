using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.World
{
    using VectorMath;

    public class World
    {
        private readonly List<IObject> Objects = new List<IObject>();

        public IEnumerable<IObject> GetObjects()
        {
            return Objects;
        } 

        public void InsertObject(IObject obj)
        {
            Objects.Add(obj);
        }

        public void ProcessFrame()
        {
            
        }
    }
}
