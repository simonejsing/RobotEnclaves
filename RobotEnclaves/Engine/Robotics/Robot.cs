using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using VectorMath;

    public class Robot : IComputer, IObject
    {
        public IMemoryBank MemoryBank { get; private set; }
        public IProgram CurrentProgram { get; set; }
        public ProgrammableEngine Engine { get; private set; }
        public ProgrammableCrane Crane { get; private set; }
        public IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                yield return Engine;
                yield return Crane;
            }
        }

        public string Name { get; private set; }
        public Vector2 Position { get; set; }
        public UnitVector2 Direction { get; set; }
        public World World { get; private set; }


        public Robot(string name)
        {
            Position = Vector2.Zero;
            Direction = UnitVector2.GetInstance(1f, 0f);
            Name = name;
            MemoryBank = new MemoryBank(200);
            Engine = new ProgrammableEngine();
            Crane = new ProgrammableCrane(this, 40f);
        }

        public void SetCurrentWorld(World world)
        {
            this.World = world;
        }

        public void ExecuteStatement(string statement)
        {
            var tokens = statement.Split(new[] { '.' }, 2);
            if (tokens.Length > 1)
            {
                var componentName = tokens[0];
                var instruction = tokens[1].Trim();

                var component = Components.First(c => c.Name.Equals(componentName));

                if (instruction.EndsWith(")"))
                {
                    component.EvaluateMethodInvocation(instruction);
                }
                else
                {
                    component.EvaluatePropertyInstruction(instruction);
                }
            }
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
