using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VectorMath;
using Vector2 = VectorMath.Vector2;

namespace MonoShims
{
    public class Renderer
    {
        private SpriteFont DefaultFont;
        private SpriteLibrary sprites;
        private Matrix2x2 Transformation;
        private Texture2D Pixel;

        public Renderer(ContentManager manager)
        {
            Pixel = manager.Load<Texture2D>(@"Sprites\pixel");

            sprites = new SpriteLibrary(manager);
            sprites.LoadSprite(SpriteLibrary.SpriteIdentifier.Player, @"Sprites\red-player-1");
            sprites.LoadSprite(SpriteLibrary.SpriteIdentifier.Block, @"Sprites\block");
            sprites.LoadSprite(SpriteLibrary.SpriteIdentifier.Spikes, @"Sprites\spikes");
            sprites.LoadSprite(SpriteLibrary.SpriteIdentifier.Coin, @"Sprites\coin");

            DefaultFont = manager.Load<SpriteFont>("DefaultFont");

            // Mirror y-axis
            Transformation = new Matrix2x2(new Vector2(1, 0), new Vector2(0, -1));
        }

        public void DrawVector(SpriteBatch spriteBatch, Vector2 origin, Vector2 vector, Color color, float thickness = 1.0f)
        {
            var to = origin + vector;
            RenderLine(
                spriteBatch,
                Transformation * origin,
                Transformation * to,
                color,
                thickness);
        }

        public void RenderLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness)
        {
            // calculate the distance between the two vectors
            float distance = Vector2.DistanceBetween(point1, point2);

            // calculate the angle between the two vectors
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness)
        {
            // stretch the pixel between the two vectors
            spriteBatch.Draw(Pixel,
                             ToXnaVector(point),
                             null,
                             color,
                             angle,
                             ToXnaVector(Vector2.Zero),
                             ToXnaVector(new Vector2(length, thickness)),
                             SpriteEffects.None,
                             0);
        }

        public void RenderPixel(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(Pixel, ToXnaVector(Transformation * position), color);
        }

        public void RenderOpagueSprite(SpriteBatch spriteBatch, SpriteLibrary.SpriteIdentifier spriteIdentifier, Vector2 position, Vector2 size)
        {
            var xnaVector = ToXnaVector(Transformation * position);
            var xnaRect = new Rectangle(xnaVector.ToPoint(), ToXnaPoint(size));
            spriteBatch.Draw(sprites.GetSprite(spriteIdentifier), xnaRect, Color.White);
        }

        private static Point ToXnaPoint(Vector2 v)
        {
            return new Point((int) v.X, (int) v.Y);
        }

        public void RenderText(SpriteBatch spriteBatch, Vector2 origin, string text, Color color)
        {
            spriteBatch.DrawString(
                DefaultFont,
                text,
                ToXnaVector(Transformation * origin),
                color);
        }

        private Microsoft.Xna.Framework.Vector2 ToXnaVector(Vector2 position)
        {
            return new Microsoft.Xna.Framework.Vector2(position.X, position.Y);
        }
    }
}
