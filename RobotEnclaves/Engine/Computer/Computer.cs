using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Computer.Programs;
    using Engine.Items;
    using Engine.Robotics;

    public class Computer : IComputer
    {
        private readonly IProgrammableComponent DefaultComponent;
        private readonly List<IProgrammableComponent> ProxyComponents = new List<IProgrammableComponent>();
        private readonly List<IProgram> programs = new List<IProgram>();

        private readonly List<IComputerUpgrade> pendingUpgrades = new List<IComputerUpgrade>(); 
        public string Name { get; private set; }

        public IObject Object { get; private set; }

        private readonly MemoryBank memoryBank;

        public IMemoryBank MemoryBank
        {
            get
            {
                return memoryBank;
            }
        }

        public IEnumerable<IProgram> Programs
        {
            get
            {
                return programs;
            }
        }

        public IProgram CurrentProgram { get; private set; }
        public ISensor Sensor { get; set; }

        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                return ProxyComponents.Concat(new [] {memoryBank});
            }
        }

        public IEnumerable<IComputerUpgrade> PendingUpgrades
        {
            get
            {
                return this.pendingUpgrades;
            }
        }

        public Computer(IObject obj, IProgrammableComponent defaultComponent, string name)
        {
            if(defaultComponent == null)
                throw new ArgumentNullException("defaultComponent");

            Object = obj;
            DefaultComponent = defaultComponent;
            Name = name;
            memoryBank = new MemoryBank(200);
            CurrentProgram = new NullProgram();
            Sensor = new NullSensor();
        }

        public void AddProxyComponents(IEnumerable<IProgrammableComponent> components)
        {
            ProxyComponents.AddRange(components);
        }

        public void SetCurrentProgram(IProgram program)
        {
            CurrentProgram = program;
        }

        public void AddProgram(IProgram program)
        {
            programs.Add(program);
        }

        public void InstallUpgrade(IComputerUpgrade upgrade)
        {
            this.pendingUpgrades.Add(upgrade);
        }

        public void ApplyUpgrades()
        {
            foreach (var upgrade in this.pendingUpgrades)
            {
                upgrade.Apply(this);
            }
        }

        public void ExecuteNextProgramStatement()
        {
            CurrentProgram.GetNextStatement().Execute(this);
        }

        public IComputerType EvaluateInstruction(string instruction)
        {
            var componentTokens = instruction.Split(new[] { '.' }, 2);
            var componentName = componentTokens[0];
            if (componentTokens.Length > 1 && ComponentExists(componentName))
            {
                var componentInstruction = componentTokens[1].Trim();
                var component = Components.First(c => c.Name.Equals(componentName, StringComparison.OrdinalIgnoreCase));

                return component.EvaluateInstruction(componentInstruction);
            }

            // Check if the instruction is a program
            if (instruction.EndsWith(")"))
            {
                var programTokens = instruction.Split(new[] { '(' }, 2);
                var programName = programTokens[0];

                var program = this.Programs.FirstOrDefault(p => p.Name.Equals(programName, StringComparison.OrdinalIgnoreCase));
                if (programTokens.Length > 1 && program != null)
                {
                    var argumentString = programTokens[1].Trim();
                    argumentString = argumentString.Substring(0, argumentString.Length - 1);
                    program.Execute(ComputerType.Parse(argumentString));
                    this.SetCurrentProgram(program);
                    return new ComputerTypeVoid();
                }
            }

            return DefaultComponent.EvaluateInstruction(instruction);
        }

        private bool ComponentExists(string name)
        {
            return Components.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
