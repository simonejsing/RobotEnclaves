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
    using VectorMath;

    public class TopologicalUserInterface : IUserInterface
    {
        private List<Widget> Widgets = new List<Widget>(); 
        private Map Map;
        private Console Console;
        private Label InputLabel;

        public TopologicalUserInterface(Vector2 viewport)
        {
            float size = viewport.Y - 100;
            
            var mapPosition = new Vector2(50f, 50f);
            var mapSize = new Vector2(size, size);
            var consolePosition = mapPosition + new Vector2(size, 0);
            var consoleSize = new Vector2(viewport.X - 100f - size, 400);

            Map = new Map(mapPosition, mapSize);
            Console = new Console(consolePosition, consoleSize);
            this.InputLabel = new Label(consolePosition + new Vector2(0, consoleSize.Y), new Vector2(consoleSize.X, 20));

            this.AddWidget(this.Map);
            this.AddWidget(this.Console);
            this.AddWidget(this.InputLabel);
        }

        private void AddWidget(Widget widget)
        {
            this.Widgets.Add(widget);
        }

        public void AddLabel(Vector2 position, Vector2 size, TextLabel text)
        {
            var label = new Label(position, size);
            label.SetLabel(text);
            this.AddWidget(label);
        }

        public void Render(IRenderEngine renderEngine)
        {
            renderEngine.Clear();

            // Renders a map widget
            foreach (var widget in Widgets)
            {
                renderEngine.ResetTransformation();
                widget.Render(renderEngine);
            }
        }

        public void SetConsoleBuffer(TextList buffer)
        {
            Console.SetBuffer(buffer);
        }

        public void SetInputLabel(TextLabel label)
        {
            this.InputLabel.SetLabel(label);
        }

        public void UpdateWorld(World world)
        {
            Map.Graphics = new List<IGraphics>(world.GetObjects().Select(GraphicsFactory.CreateFromObject));
        }
    }
}
