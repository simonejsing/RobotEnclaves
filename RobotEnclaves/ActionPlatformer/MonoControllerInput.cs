using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionPlatformer
{
    internal class MonoControllerInput : IMonoInput
    {
        private PlayerIndex index;
        private GamePadState state;

        public MonoControllerInput(PlayerIndex playerIndex)
        {
            index = playerIndex;
        }

        public void Update()
        {
            state = GamePad.GetState(index);
        }

        public bool MoveLeft()
        {
            return state.ThumbSticks.Left.X < 0.0;
        }

        public bool MoveRight()
        {
            return state.ThumbSticks.Left.X > 0.0;
        }

        public bool Jump()
        {
            return state.Buttons.A == ButtonState.Pressed;
        }
    }
}