using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;

    public interface IUserInterface
    {
        void UpdateWorld(World.World world);

        void Render(IRenderEngine renderEngine);

        void SetConsoleBuffer(TextBuffer buffer);

        void SetInputText(string line);
    }
}
