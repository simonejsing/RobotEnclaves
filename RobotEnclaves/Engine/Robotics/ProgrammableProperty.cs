using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;

    public enum ProgrammablePropertyType { ReadWrite, ReadOnly }

    public class ProgrammableProperty<T> : IProgrammableProperty where T : ComputerType
    {
        private readonly ProgrammablePropertyType Type;
        private readonly Func<T> Getter;
        private readonly Action<T> Setter;

        public ProgrammableProperty(string name, Func<T> getter, Action<T> setter, ProgrammablePropertyType type = ProgrammablePropertyType.ReadWrite)
        {
            this.Name = name;
            this.Getter = getter;
            this.Setter = setter;
            this.Type = type;
        }

        public string Name { get; private set; }

        public bool IsReadOnly
        {
            get
            {
                return Type == ProgrammablePropertyType.ReadOnly;
            }
        }

        public ComputerType Get()
        {
            return Getter();
        }

        public void Set(ComputerType value)
        {
            Setter(value as T);
        }
    }
}
