using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public class NullSensor : ISensor
    {
        public bool Active
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public float Range
        {
            get
            {
                return 0f;
            }
        }

        public IEnumerable<string> Errors
        {
            get
            {
                yield return "Hardware error";
            }
        }
    }
}
