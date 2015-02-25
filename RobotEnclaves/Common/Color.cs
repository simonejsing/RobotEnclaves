using System;

namespace Common
{
    public class Color
    {
        protected bool Equals(Color other)
        {
            return this._alpha == other._alpha && this.R == other.R && this.G == other.G && this.B == other.B;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Color)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this._alpha;
                hashCode = (hashCode*397) ^ this.R;
                hashCode = (hashCode*397) ^ this.G;
                hashCode = (hashCode*397) ^ this.B;
                return hashCode;
            }
        }

        public static bool operator ==(Color left, Color right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !Equals(left, right);
        }

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
        public static Color Sand {
            get
            {
                return new Color() { R = 198, G = 130, B = 89 };
            }
        }

        public static Color White {
            get
            {
                return new Color() {R = 255, G = 255, B = 255};
            }
        }

        public override string ToString()
        {
            return string.Format("R:{0} G:{1} B:{2} A:{3}", R, G, B, A);
        }
    }
}
