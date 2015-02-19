using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine;
    using Engine.World;

    public interface IGraphics
    {
        void Render(IRenderEngine renderEngine);
    }
}
