using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Exceptions;

    public class ProgrammableProperty<T> : IProgrammableProperty where T : ComputerType
    {
        private readonly Func<T> Getter;
        private readonly Action<T> Setter;

        public ProgrammableProperty(string name, Func<T> getter, Action<T> setter = null)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (getter == null)
                throw new ArgumentNullException("getter");

            this.Name = name;
            this.Getter = getter;
            this.Setter = setter;
        }

        public string Name { get; private set; }

        public bool IsReadOnly
        {
            get
            {
                return Setter == null;
            }
        }

        public IComputerType Get()
        {
            return Getter();
        }

        public void Set(IComputerType value)
        {
            if (Setter == null)
            {
                throw new SettingReadOnlyPropertyException(Name);
            }
            Setter(value as T);
        }
    }
}
