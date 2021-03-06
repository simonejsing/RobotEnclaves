﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace ActionPlatformer
{
    class MonoKeyboardInput : IMonoInput
    {
        private Keys[] pressedKeys;

        public MonoKeyboardInput()
        {
            pressedKeys = new Keys[0];
        }

        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            pressedKeys = kbState.GetPressedKeys();

        }

        public bool MoveLeft()
        {
            return pressedKeys.Contains(Keys.A);
        }

        public bool MoveRight()
        {
            return pressedKeys.Contains(Keys.D);
        }

        public bool Jump()
        {
            return pressedKeys.Contains(Keys.W);
        }
    }
}
