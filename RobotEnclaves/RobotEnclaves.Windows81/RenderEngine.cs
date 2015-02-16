using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ExtensionMethods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = VectorMath.Vector2;
using Color = VectorMath.Color;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaColor = Microsoft.Xna.Framework.Color;
using Rendering;

namespace RobotEnclaves.Windows81
{
    class RenderEngine : IRenderEngine
    {
        private readonly Game _game;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _defaultFont;

        private XnaVector2 TranslationVector = new XnaVector2(0f, 0f);

        public RenderEngine(Game game)
        {
            _game = game;
            _graphics = new GraphicsDeviceManager(game);
        }

        public Vector2 Viewport
        {
            get
            {
                var viewport = _graphics.GraphicsDevice.Viewport.Bounds.Size();
                return new Vector2(viewport.X, viewport.Y);
            }
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public void LoadFonts(ContentManager content)
        {
            // Load fonts
            _defaultFont = content.Load<SpriteFont>("DefaultFont");
        }

        public void Clear()
        {
            _graphics.GraphicsDevice.Clear(XnaColor.CornflowerBlue);
        }

        public void Begin()
        {
            _spriteBatch.Begin();
        }

        public void End()
        {
            _spriteBatch.End();
        }

        private XnaVector2 Transform(XnaVector2 vector)
        {
            return vector + TranslationVector;
        }

        private float TransformScalar(float radius)
        {
            return radius;
        }

        public void Translate(Vector2 vector)
        {
            TranslationVector += new XnaVector2(vector.X, vector.Y);
        }

        public void ResetTransformation()
        {
            TranslationVector = new XnaVector2(0f, 0f);
        }

        public void DrawPolygon(Vector2[] points, Color color)
        {
            for (var i = 0; i < points.Length; i++)
            {
                var from = points[i];
                var to = points[(i + 1) % points.Length];
                _spriteBatch.DrawLine(
                    Transform(new XnaVector2(from.X, from.Y)),
                    Transform(new XnaVector2(to.X, to.Y)),
                    new XnaColor(color.R, color.G, color.B, color.A));
            }
        }

        public void DrawCircle(Vector2 origin, float radius, Color color)
        {
            _spriteBatch.DrawCircle(
                Transform(new XnaVector2(origin.X, origin.Y)),
                this.TransformScalar(radius),
                10,
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawText(Vector2 origin, string text, Color color)
        {
            _spriteBatch.DrawString(
                _defaultFont,
                text,
                Transform(new XnaVector2(origin.X, origin.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void FillRectangle(Vector2 topLeft, Vector2 bottomRight, Color color)
        {
            _spriteBatch.FillRectangle(
                Transform(new XnaVector2(topLeft.X, topLeft.Y)),
                Transform(new XnaVector2(bottomRight.X, bottomRight.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawPoint(Vector2 origin, Color color)
        {
            _spriteBatch.PutPixel(
                Transform(new XnaVector2(origin.X, origin.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawVector(Vector2 origin, Vector2 vector, Color color)
        {
            var to = origin + vector;
            _spriteBatch.DrawLine(
                Transform(new XnaVector2(origin.X, origin.Y)),
                Transform(new XnaVector2(to.X, to.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawLine(Vector2 origin, Vector2 vector, Color color)
        {
            // Compute extended line that spans entire viewport
            Vector2 from, to;
            var flip = Math.Abs(vector.Y) > Math.Abs(vector.X);

            if (flip)
            {
                var alpha = vector.X / vector.Y;
                var intersect = origin.X - origin.Y * alpha;

                from = new Vector2(intersect, 0);
                to = new Vector2(intersect + _graphics.GraphicsDevice.Viewport.Height * alpha, _graphics.GraphicsDevice.Viewport.Height);
            }
            else
            {
                var alpha = vector.Y / vector.X;
                var intersect = origin.Y - origin.X * alpha;

                from = new Vector2(0, intersect);
                to = new Vector2(_graphics.GraphicsDevice.Viewport.Width, intersect + _graphics.GraphicsDevice.Viewport.Width * alpha);
            }

            // Draw line
            _spriteBatch.DrawLine(
                Transform(new XnaVector2(from.X, from.Y)),
                Transform(new XnaVector2(to.X, to.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }
    }
}
