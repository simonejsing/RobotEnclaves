using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using VectorMath;

    public interface IRenderEngine
    {
        Vector2 Viewport { get; }

        void Initialize();

        void LoadContent();

        void Clear();
        void Begin();
        void End();

        void Translate(Vector2 translateVector);

        void Scale(Vector2 scaleVector);

        void ResetTransformation();

        void DrawPolygon(Vector2[] points, Color color, float thickness = 1.0f);
        void DrawPoint(Vector2 origin, Color color);
        void DrawVector(Vector2 origin, Vector2 vector, Color color, float thickness = 1.0f);
        void DrawLine(Vector2 origin, Vector2 vector, Color color, float thickness = 1.0f);
        void DrawText(Vector2 origin, string text, Color color);
        void FillRectangle(Vector2 topLeft, Vector2 size, Color color);
        void DrawCircle(Vector2 origin, float radius, Color color, float thickness = 1.0f);
    }
}
