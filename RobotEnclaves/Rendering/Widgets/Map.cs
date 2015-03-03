using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Widgets
{
    using Common;
    using Engine;
    using Rendering.Animation;
    using VectorMath;

    public class Map : Widget
    {
        const float GridSpacing = 100.0f;

        private float ZoomFactor = 1.0f;

        private readonly List<IAnimation> animations = new List<IAnimation>();

        public List<IGraphics> Graphics { get; set; }
        public bool Sensors { get; set; }

        public Map(Vector2 position, Vector2 size)
            : base(position, size)
        {
            Graphics = new List<IGraphics>();
            GenerateNoiseMap();
        }

        public void AddAnimation(IAnimation animation)
        {
            animations.Add(animation);
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
            foreach(var animation in animations)
            {
                animation.Update(timer);
            }

            animations.RemoveAll(a => a.Completed);

            if (!Sensors)
            {
                var columns = (int)Math.Ceiling(Size.X / noiseBlockSize);
                var rows = (int)Math.Ceiling(Size.Y / noiseBlockSize);
                if (timer.Frame % 3 == 0)
                {
                    UpdateNoiseMap();
                    this.SmoothNoiseMap(0, rows, columns);
                }
            }
        }

        private int updateStartRow = 0;

        private void UpdateNoiseMap()
        {
            const int updateBlockSize = 1;

            var columns = (int)Math.Ceiling(Size.X / noiseBlockSize);
            var rows = (int)Math.Ceiling(Size.Y / noiseBlockSize);

            this.GenerateRandomDither(updateStartRow, Math.Min(updateStartRow + updateBlockSize, rows - 1), columns);
            this.SmoothNoiseMap(Math.Max(updateStartRow - 1, 0), Math.Min(updateStartRow + updateBlockSize + 1, rows - 1), columns);
            
            updateStartRow = (updateStartRow + updateBlockSize) % rows;
        }

        private void GenerateNoiseMap()
        {
            var columns = (int)Math.Ceiling(Size.X/noiseBlockSize);
            var rows = (int)Math.Ceiling(Size.Y / noiseBlockSize);
            noiseMap = new Color[columns,rows];

            this.GenerateRandomDither(0, rows, columns);
            this.SmoothNoiseMap(0, rows, columns);

            /*for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    var randomValue = rand.Next(50, 200);
                    noiseMap[x, y] = new Color() { R = randomValue, G = randomValue, B = randomValue };
                }
            }*/
        }

        private void GenerateRandomDither(int startRow, int endRow, int columns)
        {
            for (var y = 0; startRow < endRow; startRow++)
            {
                for (var x = startRow%2; x < columns; x += 2)
                {
                    var randomValue = rand.Next(50, 255);
                    this.noiseMap[x, startRow] = new Color() {R = randomValue, G = randomValue, B = randomValue};
                }
            }
        }

        private void SmoothNoiseMap(int startRow, int endRow, int columns)
        {
            for (var y = startRow; y < endRow; y++)
            {
                for (var x = (y + 1)%2; x < columns; x += 2)
                {
                    float average = 0f;
                    if (x > 0)
                    {
                        average += this.noiseMap[x - 1, y].R;
                    }
                    if (x < columns - 1)
                    {
                        average += this.noiseMap[x + 1, y].R;
                    }
                    if (y > 0)
                    {
                        average += this.noiseMap[x, y - 1].R;
                    }
                    if (y < endRow - 1)
                    {
                        average += this.noiseMap[x, y + 1].R;
                    }

                    average /= 4;

                    this.noiseMap[x, y] = new Color() {R = (int)average, G = (int)average, B = (int)average};
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
            // Center (0,0)
            renderEngine.Translate(this.Size/2);
            renderEngine.Scale(new Vector2(this.ZoomFactor, this.ZoomFactor));

            this.RenderGridLines(renderEngine);

            foreach (var graphic in this.Graphics.Where(g => g.Visible))
            {
                graphic.Render(renderEngine);
            }

            foreach (var animation in animations)
            {
                animation.Render(renderEngine);
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
