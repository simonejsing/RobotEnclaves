using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using Engine.Spaceship;

    public class GameConsole : IGameConsole
    {
        private readonly List<KeyValuePair<string, bool>> lines = new List<KeyValuePair<string, bool>>();

        public IEnumerable<KeyValuePair<string, bool>> Lines
        {
            get
            {
                return lines;
            }
        }

        public void WriteResult(CommandResult result)
        {
            var messages = result.Messages.SelectMany(m => m.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            lines.AddRange(messages.Select(m => new KeyValuePair<string, bool>(m, result.Success)));
        }
    }
}
