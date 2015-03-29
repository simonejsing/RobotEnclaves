using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class GenericStatement : IStatement
    {
        private readonly Action<IComputer> body;

        public GenericStatement(Action<IComputer> statementBody)
        {
            body = statementBody;
        }

        public void Execute(IComputer computer)
        {
            body(computer);
        }

        public static IStatement FromCode(string code)
        {
            return new GenericStatement(c => c.EvaluateInstruction(code));
        }
    }
}
