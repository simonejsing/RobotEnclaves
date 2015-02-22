using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using VectorMath;

    public interface IUserInterface
    {
        void AddLabel(Vector2 position, Vector2 size, TextLabel text);
        void UpdateWorld(World world);

        void Render(IRenderEngine renderEngine);

        void SetConsoleBuffer(TextList buffer);

        void SetInputLabel(TextLabel label);
    }
}
