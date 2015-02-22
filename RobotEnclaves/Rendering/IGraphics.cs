using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine;

    public interface IGraphics
    {
        bool Visible { get; }

        void Render(IRenderEngine renderEngine);
    }
}
