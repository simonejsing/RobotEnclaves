using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.World;
    using VectorMath;

    public class Robot : IComputer, IObject
    {
        public IMemoryBank MemoryBank { get; private set; }
        public IProgram CurrentProgram { get; set; }
        public RobotEngine Engine { get; private set; }
        public IEnumerable<IRobotComponent> Components
        {
            get
            {
                yield return Engine;
            }
        }

        public string Name { get; private set; }
        public Vector2 Position { get; set; }
        public UnitVector2 Direction { get; set; }

        public Robot(string name)
        {
            Position = Vector2.Zero;
            Direction = UnitVector2.GetInstance(1f, 0f);
            Name = name;
            MemoryBank = new MemoryBank(200);
            Engine = new RobotEngine();
        }

        public void ExecuteStatement(string statement)
        {
            var tokens = statement.Split(new[] { '.' }, 2);
            if (tokens.Length > 1)
            {
                var componentName = tokens[0];
                var instruction = tokens[1];

                var component = Components.First(c => c.Name.Equals(componentName));

                var propertyTokens = instruction.Split(new char[] {'='}, 2);
                if (propertyTokens.Length > 1)
                {
                    var propertyName = propertyTokens[0].Trim();
                    var propertyValue = float.Parse(propertyTokens[1].Trim());

                    component[propertyName] = propertyValue;
                }
            }
        }

        public void ExecuteNextProgramStatement()
        {
            CurrentProgram.GetNextStatement().Execute(this);
        }
    }
}
