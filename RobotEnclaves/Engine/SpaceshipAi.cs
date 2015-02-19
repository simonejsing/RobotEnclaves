using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Common;
    using UserInput;

    public class SpaceshipAi
    {
        public TextBuffer Console { get; private set; }
        public string Input { get; private set; }

        public SpaceshipAi()
        {
            this.Console = new TextBuffer();
            this.Input = "";
            this.InitializeConsoleBuffer();
        }

        private void InitializeConsoleBuffer()
        {
            const string consoleInitialContent =
@"Hard-Core OS booting...
BIOS check... OK
RAM check... FATAL ERROR
Peripheral sensor calibration... FATAL ERROR

FATAL ERRORS ENCOUNTERED!

Entering recovery mode...

Available RAM: 0 MB

Systems online";
            this.Console.AddRange(
                consoleInitialContent.Split(new[] { Environment.NewLine },
                StringSplitOptions.None));
        }

        public void ProcessKeystrokes(IEnumerable<Keystroke> keystrokes)
        {
            foreach (var keystroke in keystrokes)
            {
                if (keystroke.IsAlphaNumeric)
                    this.Input += keystroke.Character;
                else if (keystroke.IsBackspace && this.Input.Length > 0)
                    this.Input = this.Input.Substring(0, this.Input.Length - 1);
                else if (keystroke.IsEnter)
                {
                    Console.Add(this.Input);
                    this.Input = "";
                }
            }
        }
    }
}
