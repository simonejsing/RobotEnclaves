using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Items;
    using VectorMath;

    public class Robot : ProgrammableComponentBase, IRobot, IObject
    {
        public IComputer Computer { get; private set; }
        public IHull Hull { get; private set; }
        public IObject Object { get; private set; }

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
            Computer = new Computer(name);
            Hull = new CatarpillarHull(this);
            Object = new RobotObject(this);

            var massProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "mass",
                () => new ComputerTypeFloat(this.Object.Mass));
            this.RegisterProperty(massProperty);

            this.RegisterMethod(new ProgrammableMethod("components", ct => this.ListComponents()));
            this.RegisterMethod(new ProgrammableMethod("install", ct => this.InstallItem(ct)));
            Position = Vector2.Zero;
            BaseMass = 100.0f;

            Direction = UnitVector2.GetInstance(1f, 0f);
            Name = name;
        }

        private IComputerType InstallItem(IComputerType ct)
        {
            var arguments = (ComputerTypeList) ct;
            var itemName = (ComputerTypeString) arguments.Value[0];
            var targetName = (ComputerTypeString) arguments.Value[1];

            var target = World.FindComputerByName(targetName.Value);
            var item = Hull.CargoBay.FindItemByName(itemName.Value);

            if (item is IComputerUpgrade)
            {
                var upgrade = item as IComputerUpgrade;
                target.InstallUpgrade(upgrade);
                Hull.CargoBay.Items.Remove(item);
                return new ComputerTypeBoolean(true);
            }

            return new ComputerTypeBoolean(false);
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
            Computer.CurrentProgram.GetNextStatement().Execute(this.Computer);
        }

        public bool ObjectInRange(IObject obj, float range)
        {
            return (this.Position - obj.Position).LengthSquared < range * range;
        }
    }

}
