using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public interface ISensor
    {
        bool Active { get; set; }
        float Range { get; }

        IEnumerable<string> Errors { get; }
    }
}
