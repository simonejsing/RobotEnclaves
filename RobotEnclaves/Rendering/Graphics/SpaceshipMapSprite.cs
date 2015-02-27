using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Common;
    using Engine;
    using Engine.Spaceship;
    using VectorMath;

    class SpaceshipMapSprite : ObjectMapSprite
    {
        private Spaceship Spaceship;
        private float cosAndSinOfPiQuarter;

        const float Radius = 50f;

        private Vector2 crossTopRight;
        private Vector2 crossTopLeft;

        public SpaceshipMapSprite(Spaceship spaceship) : base(spaceship.Position)
        {
            this.cosAndSinOfPiQuarter = (float)(Math.Sqrt(2.0) / 2.0);
            Spaceship = spaceship;

            this.crossTopRight = new Vector2(cosAndSinOfPiQuarter * Radius, cosAndSinOfPiQuarter * Radius);
            this.crossTopLeft = new Vector2(-cosAndSinOfPiQuarter * Radius, cosAndSinOfPiQuarter * Radius);
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(Spaceship.Position, Radius, Color.Blue, 5f);

            // When crashed the spaceship is rendered as a crossed out circle
            if (Spaceship.Crashed)
            {
                renderEngine.DrawVector(
                    Spaceship.Position + crossTopRight,
                    -crossTopRight * 2, 
                    Color.Blue,
                    5f);

                renderEngine.DrawVector(
                    Spaceship.Position + crossTopLeft,
                    -crossTopLeft * 2,
                    Color.Blue,
                    5f);
            }
        }
    }
}
