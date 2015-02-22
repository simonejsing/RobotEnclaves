using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class FrequencyTimeCounter : TimeCounter
    {
        private float _lastValue;
        private int _cyclesSinceLastUpdate = 0;

        public FrequencyTimeCounter(int capacity)
            : base(capacity)
        {
        }

        public float Frequency
        {
            get
            {
                var measurements = Measurements.ToArray();

                if (_cyclesSinceLastUpdate < measurements.Length)
                    return _lastValue;
                
                _cyclesSinceLastUpdate = 0;
                _lastValue = measurements.Length / (measurements.Sum() / 1000);
                return _lastValue;
            }
        }

        public override string ToString()
        {
            return String.Format("{0:0.0}", Frequency);
        }

        public override void Update(long milliseconds)
        {
            base.Update(milliseconds);
            _cyclesSinceLastUpdate++;
        }
    }
}
