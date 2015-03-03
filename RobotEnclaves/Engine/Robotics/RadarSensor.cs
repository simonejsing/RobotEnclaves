using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public class RadarSensor : ISensor
    {
        public bool Active { get; set; }

        public float Range
        {
            get
            {
                return 250f;
            }
        }

        public IEnumerable<string> Errors
        {
            get
            {
                return Enumerable.Empty<string>();
            }
        }

        public RadarSensor()
        {
            Active = false;
        }
    }
}
