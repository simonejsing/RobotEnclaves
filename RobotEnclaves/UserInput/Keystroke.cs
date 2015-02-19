using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInput
{
    public class Keystroke
    {
        const char CharBackspace = (char)8;
        const char CharEnter = (char)13;

        public char Character { get; private set; }

        public bool IsBackspace
        {
            get
            {
                return Character == CharBackspace;
            }
        }

        public bool IsEnter
        {
            get
            {
                return Character == CharEnter;
            }
        }

        public bool IsAlphaNumeric
        {
            get
            {
                return (Character >= 'A' && Character <= 'Z') || (Character >= '0' && Character <= '9');
            }
        }

        public Keystroke(char character)
        {
            this.Character = character;
        }
    }
}
