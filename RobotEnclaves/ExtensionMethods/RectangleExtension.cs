using Microsoft.Xna.Framework;

namespace ExtensionMethods
{
    public static class RectangleExtension
    {
        public static Vector2 Position(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Top);
        }

        public static Vector2 Size(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }
    }
}
