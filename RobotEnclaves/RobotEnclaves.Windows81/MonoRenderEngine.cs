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
    using Engine;

    class MonoRenderEngine : IRenderEngine
    {
        private readonly Game _game;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _defaultFont;

        private XnaVector2 TranslationVector = new XnaVector2(0f, 0f);
        private float ScalingFactor = 1.0f;

        public MonoRenderEngine(Game game)
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

        public void LoadContent()
        {
            // Load fonts
            _defaultFont = _game.Content.Load<SpriteFont>("DefaultFont");
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

        private XnaVector2 ScaleVector(XnaVector2 vector)
        {
            return vector*ScalingFactor;
        }

        private XnaVector2 TransformVector(XnaVector2 vector)
        {
            return vector * ScalingFactor + TranslationVector;
        }

        private float TransformScalar(float radius)
        {
            return radius * ScalingFactor;
        }

        public void Translate(Vector2 vector)
        {
            TranslationVector += new XnaVector2(vector.X, vector.Y);
        }

        public void Scale(float factor)
        {
            ScalingFactor *= factor;
        }

        public void ResetTransformation()
        {
            TranslationVector = new XnaVector2(0f, 0f);
            ScalingFactor = 1.0f;
        }

        public void DrawPolygon(Vector2[] points, Color color)
        {
            for (var i = 0; i < points.Length; i++)
            {
                var from = points[i];
                var to = points[(i + 1) % points.Length];
                _spriteBatch.DrawLine(
                    this.TransformVector(new XnaVector2(from.X, from.Y)),
                    this.TransformVector(new XnaVector2(to.X, to.Y)),
                    new XnaColor(color.R, color.G, color.B, color.A));
            }
        }

        public void DrawCircle(Vector2 origin, float radius, Color color)
        {
            _spriteBatch.DrawCircle(
                this.TransformVector(new XnaVector2(origin.X, origin.Y)),
                this.TransformScalar(radius),
                20,
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawText(Vector2 origin, string text, Color color)
        {
            _spriteBatch.DrawString(
                _defaultFont,
                text,
                this.TransformVector(new XnaVector2(origin.X, origin.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void FillRectangle(Vector2 topLeft, Vector2 size, Color color)
        {
            _spriteBatch.FillRectangle(
                this.TransformVector(new XnaVector2(topLeft.X, topLeft.Y)),
                this.ScaleVector(new XnaVector2(size.X, size.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawPoint(Vector2 origin, Color color)
        {
            _spriteBatch.PutPixel(
                this.TransformVector(new XnaVector2(origin.X, origin.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }

        public void DrawVector(Vector2 origin, Vector2 vector, Color color)
        {
            var to = origin + vector;
            _spriteBatch.DrawLine(
                this.TransformVector(new XnaVector2(origin.X, origin.Y)),
                this.TransformVector(new XnaVector2(to.X, to.Y)),
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
                this.TransformVector(new XnaVector2(from.X, from.Y)),
                this.TransformVector(new XnaVector2(to.X, to.Y)),
                new XnaColor(color.R, color.G, color.B, color.A));
        }
    }
}
