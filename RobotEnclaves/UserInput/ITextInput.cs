using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInput
{
    public interface ITextInput
    {
        IEnumerable<Keystroke> GetNewKeystrokes();
    }
}
