using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TextList
    {
        private readonly List<string> Buffer = new List<string>();

        public IEnumerable<string> Lines {
            get
            {
                return this.Buffer;
            }
        }

        public void Add(string line)
        {
            this.Buffer.Add(line);
        }

        public void AddRange(IEnumerable<string> lines)
        {
            this.Buffer.AddRange(lines);
        }
    }
}
