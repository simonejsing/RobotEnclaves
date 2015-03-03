using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class GameTimer
    {
        public float TotalSeconds { get; private set; }
        public float DeltaSeconds { get; private set; }
        public long Frame { get; private set; }

        public GameTimer(float startTime = 0.0f)
        {
            this.TotalSeconds = startTime;
            this.DeltaSeconds = 0.0f;
            this.Frame = 0;
        }

        public void Progress(float deltaT)
        {
            this.DeltaSeconds = deltaT;
            this.TotalSeconds += deltaT;
            this.Frame++;
        }
    }
}
