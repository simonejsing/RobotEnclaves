using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInput
{
    public class Keystroke
    {
        public enum KeystrokeType { Literal, Backspace, Enter, Up, Down }

        public KeystrokeType Type { get; private set; }
        public char? Literal { get; private set; }

        private readonly char[] validSigns =
        {
            '(', ')', '.', '=', ' '
        };

        public bool IsValid
        {
            get
            {
                if (Literal == null)
                    return false;

                return
                    (this.Literal >= 'A' && this.Literal <= 'Z') ||
                    (this.Literal >= 'a' && this.Literal <= 'z') ||
                    (this.Literal >= '0' && this.Literal <= '9') ||
                    validSigns.Contains(this.Literal.Value);
            }
        }

        private Keystroke(KeystrokeType type, char? literal)
        {
            this.Type = type;
            this.Literal = literal;
        }

        public static Keystroke SpecialKeystroke(KeystrokeType type)
        {
            return new Keystroke(type, null);
        }

        public static Keystroke LiteralKeystroke(char c)
        {
            return new Keystroke(KeystrokeType.Literal, c);
        }

        public static Keystroke[] FromString(string s)
        {
            List<Keystroke> keys = new List<Keystroke>();

            for (int i = 0; i < s.Length; i++)
            {
                keys.Add(Keystroke.LiteralKeystroke(s[i]));
            }

            return keys.ToArray();
        }
    }
}
