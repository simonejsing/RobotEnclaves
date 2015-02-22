namespace Engine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class TimeCounter
    {
        private readonly float[] _measurements;
        private int _index = 0;

        public abstract override string ToString();

        protected IEnumerable<float> Measurements
        {
            get
            {
                return _measurements;
            }
        }

        public virtual void Update(long milliseconds)
        {
            _measurements[_index] = (float)(milliseconds);
            _index = (_index + 1) % _measurements.Length;
        }

        protected TimeCounter(int capacity)
        {
            _measurements = new float[capacity];
        }
    }
}