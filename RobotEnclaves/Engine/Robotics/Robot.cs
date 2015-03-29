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
        public IComputer Computer { get; private set; }
        public IHull Hull { get; private set; }
        public IObject Object { get; private set; }

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
                return Hull.Components;
            }
        }

        private ComputerType ListComponents()
        {
            return new ComputerTypeList(Components.Select(c => new ComputerTypeString(c.Name)));
        }

        public override sealed string Name { get; protected set; }

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
            Hull = new CatarpillarHull(this);

            Computer = new Computer(this.Object, this, name);
            Computer.AddProxyComponents(Hull.Components);

            var massProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "mass",
                () => new ComputerTypeFloat(this.Object.Mass));
            this.RegisterProperty(massProperty);

            this.RegisterMethod(new ProgrammableMethod("reboot", ct => this.Reboot()));
            this.RegisterMethod(new ProgrammableMethod("components", ct => this.ListComponents()));
            this.RegisterMethod(new ProgrammableMethod("install", this.InstallItem));
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

        public bool ObjectInRange(IObject obj, float range)
        {
            return (this.Position - obj.Position).LengthSquared < range * range;
        }
    }

}
