using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using System.Collections;
    using Common;
    using Engine;
    using Engine.World;
    using Rendering.Graphics;
    using Rendering.Widgets;
    using UserInput;
    using VectorMath;

    public class TopologicalUserInterface : IUserInterface
    {
        private Map Map;
        private Console Console;
        private InputField InputField;

        List<IGraphics> Graphics = new List<IGraphics>();

        public TopologicalUserInterface(Vector2 viewport)
        {
            float size = viewport.Y - 100;
            
            var mapPosition = new Vector2(50f, 50f);
            var mapSize = new Vector2(size, size);
            var consolePosition = mapPosition + new Vector2(size, 0);
            var consoleSize = new Vector2(viewport.X - 100f - size, 400);

            Map = new Map(mapPosition, mapSize);
            Console = new Console(consolePosition, consoleSize);
            InputField = new InputField(consolePosition + new Vector2(0, consoleSize.Y), new Vector2(consoleSize.X, 20));

        }

        public void Render(IRenderEngine renderEngine)
        {
            renderEngine.Clear();

            // Renders a map widget
            renderEngine.ResetTransformation();
            Map.Render(renderEngine, this.Graphics);

            renderEngine.ResetTransformation();
            Console.Render(renderEngine);

            renderEngine.ResetTransformation();
            InputField.Render(renderEngine);
        }

        public void SetConsoleBuffer(TextBuffer buffer)
        {
            Console.SetBuffer(buffer);
        }

        public void SetInputText(string line)
        {
            InputField.Text = line;
        }

        public void ProcessKeystrokes(IEnumerable<Keystroke> keystrokes)
        {
        }

        public void SetActiveWorld(World world)
        {
            this.Graphics = new List<IGraphics>(world.GetObjects().Select(GraphicsFactory.CreateFromObject));
        }
    }
}
