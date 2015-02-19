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

    public class Console : Widget
    {
        private TextBuffer ConsoleBuffer = null;

        public Console(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public void SetBuffer(TextBuffer buffer)
        {
            ConsoleBuffer = buffer;
        }

        public void Render(IRenderEngine renderEngine)
        {
            renderEngine.Translate(Position);

            renderEngine.FillRectangle(Vector2.Zero, Size, Color.Gray);

            var numberOfLinesOnScreen = (int)Math.Floor(Size.Y/20);
            var offset = 0;
            foreach (var line in ConsoleBuffer.Lines.AsEnumerable().Reverse().Take(numberOfLinesOnScreen).Reverse())
            {
                renderEngine.DrawText(new Vector2(0, offset * 20), line, Color.White);
                offset++;
            }

            renderEngine.ResetTransformation();
        }
    }
}
