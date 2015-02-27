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

    public class Map : Widget
    {
        const float GridSpacing = 100.0f;

        private float ZoomFactor = 1.0f;

        public List<IGraphics> Graphics { get; set; }
        public bool Sensors { get; set; }

        public Map(Vector2 position, Vector2 size)
            : base(position, size)
        {
            Graphics = new List<IGraphics>();
            GenerateNoiseMap();
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.Translate(this.Position);
            if (Sensors)
            {
                this.RenderActiveMap(renderEngine);
            }
            else
            {
                this.RenderStaticNoise(renderEngine);
            }

            this.RenderMapContent(renderEngine);

            renderEngine.ResetTransformation();
        }

        private const int noiseBlockSize = 10;
        private Color[,] noiseMap;

        public override void Update(GameTimer timer)
        {
            if (Sensors)
                return;

            if (timer.Frame % 20 == 0)
            {
                GenerateNoiseMap();
            }
        }

        private void GenerateNoiseMap()
        {
            var columns = (int)Math.Ceiling(Size.X/noiseBlockSize);
            var rows = (int)Math.Ceiling(Size.Y / noiseBlockSize);
            noiseMap = new Color[columns,rows];

            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    var randomValue = rand.Next(50, 200);
                    noiseMap[x, y] = new Color() { R = randomValue, G = randomValue, B = randomValue };
                }
            }
        }

        private static readonly Random rand = new Random();

        private void RenderStaticNoise(IRenderEngine renderEngine)
        {
            var columns = (int)Math.Ceiling(Size.X / noiseBlockSize);
            var rows = (int)Math.Ceiling(Size.Y / noiseBlockSize);

            var p = new Vector2(0, 0);
            var blockSizeV = new Vector2(noiseBlockSize, noiseBlockSize);
            renderEngine.FillRectangle(Vector2.Zero, this.Size, Color.Black);
            for (var y = 0; y < rows; y++)
            {
                p.Y = y * noiseBlockSize;
                for (var x = 0; x < columns; x++)
                {
                    p.X = x * noiseBlockSize;
                    renderEngine.FillRectangle(p, blockSizeV, noiseMap[x, y]);
                }
            }
        }

        private void RenderActiveMap(IRenderEngine renderEngine)
        {
            renderEngine.FillRectangle(Vector2.Zero, this.Size, Color.Sand);
        }

        private void RenderMapContent(IRenderEngine renderEngine)
        {
            // Center (0,0) and flip Y-axis (+ is then up)
            renderEngine.Translate(this.Size/2);
            renderEngine.Scale(new Vector2(this.ZoomFactor, -this.ZoomFactor));

            this.RenderGridLines(renderEngine);

            foreach (var graphic in this.Graphics.Where(g => g.Visible))
            {
                graphic.Render(renderEngine);
            }
        }

        private void RenderGridLines(IRenderEngine renderEngine)
        {
            Vector2 mapSize = Size / this.ZoomFactor;

            DrawVerticalGridLine(renderEngine, 0, mapSize.Y);
            DrawHorizontalGridLine(renderEngine, 0, mapSize.X);
            for (float x = GridSpacing; x < mapSize.X / 2.0f; x += GridSpacing)
            {
                DrawVerticalGridLine(renderEngine, x, mapSize.Y);
            }
            for (float x = -GridSpacing; x > -mapSize.X / 2.0f; x -= GridSpacing)
            {
                DrawVerticalGridLine(renderEngine, x, mapSize.Y);
            }
            for (float y = GridSpacing; y < mapSize.Y / 2.0f; y += GridSpacing)
            {
                DrawHorizontalGridLine(renderEngine, y, mapSize.X);
            }
            for (float y = -GridSpacing; y > -mapSize.Y / 2.0f; y -= GridSpacing)
            {
                DrawHorizontalGridLine(renderEngine, y, mapSize.X);
            }
        }

        private static void DrawVerticalGridLine(IRenderEngine renderEngine, float x, float height)
        {
            Vector2 start = new Vector2(x, -height / 2.0f);
            Vector2 vector = new Vector2(0.0f, height);
            renderEngine.DrawVector(start, vector, Color.Gray);
        }

        private static void DrawHorizontalGridLine(IRenderEngine renderEngine, float y, float width)
        {
            Vector2 start = new Vector2(-width / 2.0f, y);
            Vector2 vector = new Vector2(width, 0.0f);
            renderEngine.DrawVector(start, vector, Color.Gray);
        }
    }
}
