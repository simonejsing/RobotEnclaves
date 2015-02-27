using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public class RadarSensor : ISensor
    {
        public float Range
        {
            get
            {
                return 250f;
            }
        }
    }
}
