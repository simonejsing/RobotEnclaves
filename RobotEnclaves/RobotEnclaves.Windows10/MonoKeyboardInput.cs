using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotEnclaves.Windows10
{
    using Microsoft.Xna.Framework.Input;
    using UserInput;

    class MonoKeyboardInput : ITextInput
    {
        private Keys[] lastPressedKeys;

        private List<Keystroke> keystrokeBuffer = new List<Keystroke>(); 

        public MonoKeyboardInput()
        {
            lastPressedKeys = new Keys[0];
        }

        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            var shiftPressed = kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift);

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key, shiftPressed);
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyDown(Keys key, bool shift)
        {
            char c;

            switch(key)
            {
                case Keys.Enter:
                    keystrokeBuffer.Add(Keystroke.SpecialKeystroke(Keystroke.KeystrokeType.Enter));
                    return;
                case Keys.Back:
                    keystrokeBuffer.Add(Keystroke.SpecialKeystroke(Keystroke.KeystrokeType.Backspace));
                    return;
                case Keys.Up:
                    keystrokeBuffer.Add(Keystroke.SpecialKeystroke(Keystroke.KeystrokeType.Up));
                    return;
                case Keys.Down:
                    keystrokeBuffer.Add(Keystroke.SpecialKeystroke(Keystroke.KeystrokeType.Down));
                    return;

                case Keys.OemMinus:
                    c = '-';
                    break;
                case Keys.Space:
                    c = ' ';
                    break;
                case Keys.OemPeriod:
                    c = '.';
                    break;
                case Keys.OemComma:
                    c = ',';
                    break;
                case Keys.OemPlus:
                    c = '=';
                    break;
                case Keys.OemQuotes:
                    c = '"';
                    break;
                default:
                    if (shift)
                    {
                        switch (key)
                        {
                            case Keys.D9:
                                c = '(';
                                break;
                            case Keys.D0:
                                c = ')';
                                break;
                            default:
                                c = (char)key;
                                break;
                        }
                    }
                    else
                        c = char.ToLower((char)key);
                    break;
            }

            keystrokeBuffer.Add(Keystroke.LiteralKeystroke(c));
        }

        private void OnKeyUp(Keys key)
        {
            //do stuff
        }

        public IEnumerable<Keystroke> GetNewKeystrokes()
        {
            var returnValue = keystrokeBuffer.ToArray();
            keystrokeBuffer.Clear();
            return returnValue;
        }
    }
}
