using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Computer;
    using FluentAssertions;
    using Moq;

    [TestClass]
    public class RobotTests
    {
        [TestMethod]
        public void RobotExecutesNextStatementOfCurrentProgramWhenRequestedTo()
        {
            const byte expected = 0x7F;

            var robot = new Robot();
            var statement = new Mock<IStatement>();
            var program = new Mock<IProgram>();
            program.Setup(p => p.GetNextStatement()).Returns(statement.Object);
            statement.Setup(s => s.Execute(It.IsAny<IMemoryBank>())).Callback((IMemoryBank mb) => mb.Set(0, expected));

            robot.CurrentProgram = program.Object;
            robot.ExecuteNextProgramStatement();

            robot.MemoryBank.Get(0).Should().Be(expected);
        }
    }
}
