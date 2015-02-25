using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Storyline
{
    public class StoryEvent
    {
        public float Time { get; private set; }
        public Action Event { get; private set; }

        public StoryEvent(float time, Action e)
        {
            this.Time = time;
            this.Event = e;
        }
    }
}
