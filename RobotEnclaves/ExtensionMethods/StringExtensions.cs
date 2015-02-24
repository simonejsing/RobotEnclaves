using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool IsAlphaNumeric(this char c)
        {
            return
                (c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                (c >= '0' && c <= '9');
        }

        public static bool IsAlphaNumeric(this string str)
        {
            foreach (var c in str)
            {
                if (!c.IsAlphaNumeric())
                    return false;
            }

            return true;
        }
    }
}
