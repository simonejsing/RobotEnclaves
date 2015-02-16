using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using VectorMath;

    public interface IRenderEngine
    {
        Vector2 Viewport { get; }
        void Initialize();
        void Clear();
        void Begin();
        void End();
        void DrawPolygon(Vector2[] points, Color color);
        void DrawPoint(Vector2 origin, Color color);
        void DrawVector(Vector2 origin, Vector2 vector, Color color);
        void DrawLine(Vector2 origin, Vector2 vector, Color color);
        void DrawText(Vector2 origin, string text, Color color);
        void FillRectangle(Vector2 origin, Vector2 size, Color color);
        void DrawCircle(Vector2 origin, float radius, Color color);
    }
}
