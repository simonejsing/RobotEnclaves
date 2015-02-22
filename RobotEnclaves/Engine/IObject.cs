using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using VectorMath;

    public interface IObject
    {
        Vector2 Position { get; set; }
    }
}
