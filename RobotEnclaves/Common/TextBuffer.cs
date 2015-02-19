using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TextBuffer
    {
        private readonly List<string> ConsoleBuffer = new List<string>();

        public IEnumerable<string> Lines {
            get
            {
                return ConsoleBuffer;
            }
        }

        public void Add(string line)
        {
            ConsoleBuffer.Add(line);
        }

        public void AddRange(IEnumerable<string> lines)
        {
            ConsoleBuffer.AddRange(lines);
        }
    }
}
