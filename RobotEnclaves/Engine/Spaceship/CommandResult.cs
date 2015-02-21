using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Spaceship
{
    public class CommandResult
    {
        private readonly List<string> messages = new List<string>();
 
        public bool Success { get; private set; }

        public IEnumerable<string> Messages
        {
            get
            {
                return messages;
            }
        }

        public CommandResult(bool success)
        {
            this.Success = success;
        }

        public void AddMessages(IEnumerable<string> lines)
        {
            messages.AddRange(lines);
        }

        public void AddMessages(params string[] lines)
        {
            messages.AddRange(lines);
        }
    }
}
