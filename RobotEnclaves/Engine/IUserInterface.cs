using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using System.Collections.Generic;
    using Common;
    using VectorMath;

    public interface IUserInterface
    {
        void AddLabel(Vector2 position, Vector2 size, TextLabel text);
        void UpdateWorld(World world);

        void Render(IRenderEngine renderEngine);

        void SetConsole(IGameConsole console);

        void SetMapSensors(bool active);

        void SetInputLabel(TextLabel label);
    }
}
