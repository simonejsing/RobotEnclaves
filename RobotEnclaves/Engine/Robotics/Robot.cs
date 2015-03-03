using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Items;
    using VectorMath;

    public class Robot : ProgrammableComponentBase, IRobot
    {
        private readonly List<IProgram> programs = new List<IProgram>();
 
        public IComputer Computer { get; private set; }
        public IHull Hull { get; private set; }
        public IObject Object { get; private set; }

        public IEnumerable<IProgram> Programs
        {
            get
            {
                return programs;
            }
        }

        public void Progress(GameTimer timer)
        {
            if (!Computer.CurrentProgram.Finished)
            {
                Computer.CurrentProgram.GetNextStatement().Execute(Computer);
            }
        }

        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                return Hull.Components.Concat(Computer.Components);
            }
        }

        private ComputerType ListComponents()
        {
            return new ComputerTypeList(Components.Select(c => new ComputerTypeString(c.Name)));
        }

        public override string Name { get; protected set; }

        public Vector2 Position
        {
            get
            {
                return Object.Position;
            }
            set
            {
                Object.Position = value;
            }
        }

        public UnitVector2 Direction
        {
            get
            {
                return Object.Direction;
            }
            set
            {
                Object.Direction = value;
            }
        }

        public IObjectHealth ObjectHealth
        {
            get
            {
                return Object.ObjectHealth;
            }
        }

        public float Mass
        {
            get
            {
                return Object.Mass;
            }
        }

        public World World { get; private set; }
        public float BaseMass { get; set; }

        public Robot(string name)
        {
            Object = new RobotObject(this);
            Computer = new Computer(this.Object, name);
            Hull = new CatarpillarHull(this);

            var massProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "mass",
                () => new ComputerTypeFloat(this.Object.Mass));
            this.RegisterProperty(massProperty);

            this.RegisterMethod(new ProgrammableMethod("reboot", ct => this.Reboot()));
            this.RegisterMethod(new ProgrammableMethod("components", ct => this.ListComponents()));
            this.RegisterMethod(new ProgrammableMethod("install", ct => this.InstallItem(ct)));
            this.RegisterMethod(new ProgrammableMethod("diagnostics", ct => this.RunDiagnosticsReport()));

            Position = Vector2.Zero;
            BaseMass = 100.0f;

            Direction = UnitVector2.GetInstance(1f, 0f);
            Name = name;
        }

        private IComputerType Reboot()
        {
            Computer.ApplyUpgrades();
            return new ComputerTypeVoid();
        }

        protected void RegisterProgram(IProgram program)
        {
            programs.Add(program);
        }

        private IComputerType RunDiagnosticsReport()
        {
            var result = new ComputerTypeList();

            result.Value.Add(new ComputerTypeString(string.Format("Diagnostics report ({0}):", Name)));
            result.Value.AddRange(Components.Select(c => new ComputerTypeString(string.Format("  {0}: {1}", c.Name, FormatComponentStatus(c)))));

            return result;
        }

        private string FormatComponentStatus(IProgrammableComponent component)
        {
            if (component.Errors.Any())
            {
                return string.Format("<error: {0}>", string.Join(", ", component.Errors));
            }

            return "OK";
        }

        private IComputerType InstallItem(IComputerType ct)
        {
            var arguments = (ComputerTypeList) ct;
            var itemName = (ComputerTypeString) arguments.Value[0];
            var targetName = (ComputerTypeString) arguments.Value[1];

            var item = Hull.CargoBay.FindItemByName(itemName.Value);
            var upgrade = (IComputerUpgrade)item;
            if (upgrade == null)
                throw new RobotException("Item is not an upgrade.");

            var target = World.FindComputerByName(targetName.Value);
            if (!Hull.Crane.ItemInRange(target.Object))
            {
                throw new RobotException("Target is out of range.");
            }

            target.InstallUpgrade(upgrade);
            Hull.CargoBay.Items.Remove(item);
            return new ComputerTypeBoolean(true);
        }

        public void SetCurrentWorld(World world)
        {
            this.World = world;
        }

        public override IComputerType EvaluateInstruction(string instruction)
        {
            var componentTokens = instruction.Split(new[] { '.' }, 2);
            var componentName = componentTokens[0];
            if (componentTokens.Length > 1 && ComponentExists(componentName))
            {
                var componentInstruction = componentTokens[1].Trim();
                var component = Components.First(c => c.Name.Equals(componentName));

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
                    this.Computer.SetCurrentProgram(program);
                    return new ComputerTypeVoid();
                }
            }

            return base.EvaluateInstruction(instruction);
        }

        private bool ComponentExists(string name)
        {
            return Components.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void ExecuteNextProgramStatement()
        {
            Computer.CurrentProgram.GetNextStatement().Execute(this.Computer);
        }

        public bool ObjectInRange(IObject obj, float range)
        {
            return (this.Position - obj.Position).LengthSquared < range * range;
        }
    }

}
