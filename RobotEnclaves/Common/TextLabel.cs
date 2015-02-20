using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TextLabel
    {
        private string label = "";

        public string Text
        {
            get
            {
                return label ?? "";
            }
            set
            {
                label = value;
            }
        }
    }
}
