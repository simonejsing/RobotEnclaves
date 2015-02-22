using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    using System.Diagnostics;

    public class LapStopwatch
    {
        private readonly Stopwatch stopwatch;
        private long lastMeasurement;

        public LapStopwatch()
        {
            stopwatch = new Stopwatch();
        }

        public long LapMilliseconds
        {
            get
            {
                return stopwatch.ElapsedMilliseconds - lastMeasurement;
            }
        }

        public long ElapsedMilliseconds
        {
            get
            {
                lastMeasurement = stopwatch.ElapsedMilliseconds;
                return lastMeasurement;
            }
        }

        public void Restart()
        {
            stopwatch.Restart();
            lastMeasurement = stopwatch.ElapsedMilliseconds;
        }
    }
}
