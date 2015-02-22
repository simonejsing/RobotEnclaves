using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class AverageTimeCounter : TimeCounter
    {
        private float _lastValue;
        private int _cyclesSinceLastUpdate = 0;

        public AverageTimeCounter(int capacity)
            : base(capacity)
        {
        }

        public float Average
        {
            get
            {
                var measurements = Measurements.ToArray();

                if (_cyclesSinceLastUpdate < measurements.Length)
                    return _lastValue;

                _cyclesSinceLastUpdate = 0;
                _lastValue = measurements.Average();
                return _lastValue;
            }
        }

        public override string ToString()
        {
            return Average.ToString();
        }

        public override void Update(long milliseconds)
        {
            base.Update(milliseconds);
            _cyclesSinceLastUpdate++;
        }
    }
}
