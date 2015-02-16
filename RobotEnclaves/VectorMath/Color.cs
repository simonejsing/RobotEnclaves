using System;

namespace VectorMath
{
    public class Color
    {
        private static readonly Random _random = new Random(1);
        private int _alpha = 255;

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int A
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public static Color Random
        {
            get
            {
                return new Color() {R = _random.Next(256), G = _random.Next(256), B = _random.Next(256)};
            }
        }

        public static Color Red { get { return new Color() { R = 255 }; } }
        public static Color Green { get { return new Color() { G = 128 }; } }
        public static Color Blue { get { return new Color() { B = 255 }; } }
        public static Color Black { get { return new Color() { R = 0, G = 0, B = 0 }; } }
        public static Color Gray { get { return new Color() { R = 128, G = 128, B = 128 }; } }
        public static Color Cyan { get { return new Color() { R = 0, G = 255, B = 255} ;} }
    }
}
