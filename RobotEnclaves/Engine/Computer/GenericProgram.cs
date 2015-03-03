using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class GenericProgram : IProgram
    {
        private readonly List<IStatement> statements = new List<IStatement>();
        private int currentLineNumber = 0;
 
        public string Name { get; private set; }

        public bool Finished
        {
            get
            {
                return currentLineNumber == statements.Count;
            }
        }

        public void AddStatement(IStatement statement)
        {
            statements.Add(statement);
        }

        public GenericProgram(string name)
        {
            this.Name = name;
        }

        public void Execute(IComputerType arguments)
        {
            throw new NotImplementedException();
        }

        public IStatement GetNextStatement()
        {
            var line = statements[currentLineNumber];
            currentLineNumber++;
            return line;
        }
    }
}
