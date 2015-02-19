using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Widgets
{
    using VectorMath;

    public abstract class Widget
    {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }

        protected Widget(Vector2 position, Vector2 size)
        {
            this.Position = position;
            this.Size = size;
        }
    }
}
