using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using VectorMath;
    using ExtensionMethods;

    public class Robot : ProgrammableComponentBase, IComputer, IObject
    {
        public IMemoryBank MemoryBank { get; private set; }
        public IProgram CurrentProgram { get; set; }
        public ProgrammableEngine Engine { get; private set; }
        public ProgrammableCrane Crane { get; private set; }
        public ProgrammableCargoBay CargoBay { get; set; }
        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                yield return Engine;
                yield return Crane;
                yield return CargoBay;
            }
        }

        private ComputerType ListComponents()
        {
            return new ComputerTypeList(Components.Select(c => new ComputerTypeString(c.Name)));
        }

        public override string Name { get; protected set; }
        public Vector2 Position { get; set; }
        public float Mass {
            get
            {
                return BaseMass + CargoBay.TotalMass;
            }
        }
        public UnitVector2 Direction { get; set; }
        public World World { get; private set; }
        public float BaseMass { get; set; }

        public Robot(string name)
        {
            var massProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "mass",
                () => new ComputerTypeFloat(this.Mass));
            this.RegisterProperty(massProperty);

            this.RegisterMethod(new ProgrammableMethod("components", ct => this.ListComponents()));
            Position = Vector2.Zero;
            BaseMass = 100.0f;

            Direction = UnitVector2.GetInstance(1f, 0f);
            Name = name;
            MemoryBank = new MemoryBank(200);
            Engine = new ProgrammableEngine();
            Crane = new ProgrammableCrane(this, 40f);
            CargoBay = new ProgrammableCargoBay(100.0f);
        }

        public void SetCurrentWorld(World world)
        {
            this.World = world;
        }

        public override IComputerType EvaluateInstruction(string instruction)
        {
            var tokens = instruction.Split(new[] { '.' }, 2);
            var componentName = tokens[0];
            if (tokens.Length > 1 && ComponentExists(componentName))
            {
                var componentInstruction = tokens[1].Trim();
                var component = Components.First(c => c.Name.Equals(componentName));

                return component.EvaluateInstruction(componentInstruction);
            }

            return base.EvaluateInstruction(instruction);
        }

        private bool ComponentExists(string name)
        {
            return Components.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void ExecuteNextProgramStatement()
        {
            CurrentProgram.GetNextStatement().Execute(this);
        }

        public bool ObjectInRange(IObject obj, float range)
        {
            return (this.Position - obj.Position).LengthSquared < range * range;
        }
    }

}
