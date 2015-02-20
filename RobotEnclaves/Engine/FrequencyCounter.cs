using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class FrequencyCounter
    {
        private readonly float[] _measurements;
        private int _index = 0;
        private long _lastUpdate;

        private int _cyclesSinceLastUpdate = 0;
        private float _lastValue;

        public float Frequency
        {
            get
            {
                if (_cyclesSinceLastUpdate < _measurements.Length)
                    return _lastValue;
                
                _cyclesSinceLastUpdate = 0;
                _lastValue = _measurements.Length / _measurements.Sum();
                return _lastValue;
            }
        }

        public FrequencyCounter(int capacity)
        {
            _lastUpdate = DateTime.Now.Ticks;
            _measurements = new float[capacity];
        }

        public void Update()
        {
            var ticks = DateTime.Now.Ticks;
            _measurements[_index] = (float)(ticks - _lastUpdate) / 10000 / 1000;
            _lastUpdate = ticks;
            _index = (_index + 1) % _measurements.Length;
            _cyclesSinceLastUpdate++;
        }
    }
}
