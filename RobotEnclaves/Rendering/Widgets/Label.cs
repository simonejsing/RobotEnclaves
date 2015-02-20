using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Widgets
{
    using Common;
    using Engine;
    using VectorMath;

    class Label : Widget
    {
        private TextLabel text;

        public Label(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public void SetLabel(TextLabel label)
        {
            text = label;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.Translate(Position);

            renderEngine.FillRectangle(Vector2.Zero, Size, Color.Black);

            renderEngine.DrawText(Vector2.Zero, this.text.Text, Color.White);
        }
    }
}
