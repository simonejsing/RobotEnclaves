using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using UserInput;

    public interface IUserInterface
    {
        void SetActiveWorld(World.World world);

        void Render(IRenderEngine renderEngine);

        void SetConsoleBuffer(TextBuffer buffer);

        void SetInputText(string line);

        void ProcessKeystrokes(IEnumerable<Keystroke> keystrokes);
    }
}
