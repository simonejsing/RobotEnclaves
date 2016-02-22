using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameWorld.Inputs;

namespace UnitTest.Common
{
    public static class MockInput
    {
        public static ControllerInput Idle
        {
            get { return WorldPlayerInput(false, false, false, false); }
        }

        public static ControllerInput Left
        {
            get { return WorldPlayerInput(true, false, false, false); }
        }

        public static ControllerInput Right
        {
            get { return WorldPlayerInput(false, true, false, false); }
        }

        public static ControllerInput Fire
        {
            get { return WorldPlayerInput(false, false, true, false); }
        }

        public static ControllerInput Jump
        {
            get { return WorldPlayerInput(false, false, false, true); }
        }

        public static ControllerInput JumpLeft
        {
            get { return WorldPlayerInput(true, false, false, true); }
        }

        public static ControllerInput WorldPlayerInput(bool left, bool right, bool fire, bool jump)
        {
            return new ControllerInput()
            {
                RequestedMoveLeft = left,
                RequestedMoveRight = right,
                RequestedShoot = fire,
                RequestedMoveUp = jump
            };
        }
    }
}
