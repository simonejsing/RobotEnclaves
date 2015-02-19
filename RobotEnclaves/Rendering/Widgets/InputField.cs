using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Widgets
{
    using Engine;
    using VectorMath;

    class InputField : Widget
    {
        public string Text { get; set; }

        public InputField(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public void Render(IRenderEngine renderEngine)
        {
            renderEngine.Translate(Position);

            renderEngine.FillRectangle(Vector2.Zero, Size, Color.Black);

            renderEngine.DrawText(Vector2.Zero, "> " + (this.Text ?? ""), Color.White);
        }
    }
}
