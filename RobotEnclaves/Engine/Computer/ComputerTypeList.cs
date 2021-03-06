﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;

    public sealed class ComputerTypeList : ComputerType
    {
        public List<ComputerType> Value { get; private set; }

        public ComputerTypeList(params ComputerType[] values)
        {
            this.Value = new List<ComputerType>();
            this.Value.AddRange(values);
        }

        public ComputerTypeList(IEnumerable<ComputerType> values)
        {
            this.Value = new List<ComputerType>();
            this.Value.AddRange(values.ToArray());
        }

        public override string TypeName
        {
            get
            {
                return "List";
            }
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, Value.Select(v => v.ToString()));
        }
    }
}
