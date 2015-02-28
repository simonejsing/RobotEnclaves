using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Animation
{
    public interface IAnimation
    {
        bool Completed { get; }

        void Update(GameTimer gameTimer);
        void Render(IRenderEngine renderEngine);
    }
}
