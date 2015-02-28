using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMath;

namespace Rendering.Animation
{
    public class MapHighlightAnimation : IAnimation
    {
        private const int length = 60 * 3;
        private float progress = 0;
        private float angle = 0;
        private long startFrame;
        private Vector2 animationCenter;

        public bool Completed { get; private set; }

        public MapHighlightAnimation(GameTimer startTime, Vector2 center)
        {
            animationCenter = center;
            startFrame = startTime.Frame;
        }

        public void Update(GameTimer gameTimer)
        {
            Completed = gameTimer.Frame >= startFrame + length;
            progress = (float)(gameTimer.Frame - startFrame) / length;
            angle += 0.03f;
        }

        public void Render(IRenderEngine renderEngine)
        {
            var sizeVector = new Vector2(50, 50);
            if (progress < 0.5f)
            {
                sizeVector += new Vector2(500, 500) * (0.5f - progress);
            }

            renderEngine.DrawTexture("Circular-highlight", animationCenter, sizeVector, angle, TextureDrawMode.Centered);
        }
    }
}
