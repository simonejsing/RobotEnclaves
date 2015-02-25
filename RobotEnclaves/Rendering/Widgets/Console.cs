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
        private IGameConsole consoleBuffer;

        public Console(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public void SetConsole(IGameConsole console)
        {
            consoleBuffer = console;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.Translate(Position);

            renderEngine.FillRectangle(Vector2.Zero, Size, Color.Gray);

            var numberOfLinesOnScreen = (int)Math.Floor(Size.Y/20);
            var offset = 0;
            var visibleLines = consoleBuffer.Lines.AsEnumerable().Reverse().Take(numberOfLinesOnScreen).Reverse();
            foreach (var line in visibleLines)
            {
                var color = line.Value ? Color.White : Color.Red;
                renderEngine.DrawText(new Vector2(0, offset * 20), line.Key, color);
                offset++;
            }

            renderEngine.ResetTransformation();
        }
    }
}
