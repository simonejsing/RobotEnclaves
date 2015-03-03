using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;
    using Engine.Robotics;

    public class MemoryBank : ProgrammableComponentBase, IMemoryBank
    {
        private IComputerType[] storage;

        public override string Name
        {
            get
            {
                return "ram";
            }
            protected set
            {
            }
        }

        public MemoryBank(int size)
        {
            this.RegisterProperty(
                new ProgrammableProperty<ComputerTypeInt>(
                    "size",
                    () => new ComputerTypeInt(this.SizeMB)));

            this.RegisterMethod(new ProgrammableMethod("get", this.GetWrapper));
            this.RegisterMethod(new ProgrammableMethod("set", this.SetWrapper));

            this.storage = new IComputerType[size];
        }

        private IComputerType SetWrapper(IComputerType ct)
        {
            ComputerTypeList ctList = (ComputerTypeList)ct;
            ComputerTypeInt indexCt = new ComputerTypeInt(ctList.Value[0]);
            var valueCt = ctList.Value[1];
            
            if(valueCt is ComputerTypeList)
                throw new RobotException(string.Format("Cannot store {0} directly in memory.", valueCt.TypeName));
            
            this.Set(indexCt.Value, valueCt);
            return new ComputerTypeVoid();
        }

        private IComputerType GetWrapper(IComputerType ct)
        {
            var ctInt = new ComputerTypeInt(ct);
            return this.GetByte(ctInt.Value);
        }

        public int SizeMB
        {
            get
            {
                return this.storage.Length / 1024;
            }
        }

        public void Set(int address, IComputerType value)
        {
            this.storage[address] = value;
        }

        public IComputerType GetByte(int address)
        {
            return this.storage[address];
        }

        public void Upgrade(int extraMb)
        {
            this.storage = new IComputerType[this.storage.Length + extraMb * 1024];
        }
    }
}
