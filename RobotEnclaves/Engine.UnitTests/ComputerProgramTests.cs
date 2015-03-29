using System;
using Engine.Computer;
using Engine.Computer.Programs;
using Engine.Robotics;
using Engine.UnitTests.Stubs;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Engine.UnitTests
{
    [TestClass]
    public class ComputerProgramTests
    {
        [TestMethod]
        public void ComputerExecutesNextStatementOfCurrentProgramWhenRequestedTo()
        {
            var expected = new ComputerTypeFloat(12.5f);

            var computer = new Computer.Computer(null, new NullProgrammableComponent(), "computer");
            var statement = new Mock<IStatement>();
            var program = new Mock<IProgram>();
            program.Setup(p => p.GetNextStatement()).Returns(statement.Object);
            statement.Setup(s => s.Execute(It.IsAny<IComputer>())).Callback((IComputer c) => c.MemoryBank.Set(0, expected));

            computer.MemoryBank.Set(0, new ComputerTypeFloat(0f));
            computer.SetCurrentProgram(program.Object);
            computer.ExecuteNextProgramStatement();

            computer.MemoryBank.GetByte(0).Should().Be(expected);
        }

        [TestMethod]
        public void RobotCanRunAProgramWithArguments()
        {
            IComputerType arguments = null;
            var mockProgram = new Mock<IProgram>();
            mockProgram.Setup(m => m.Name).Returns("program");
            mockProgram.Setup(m => m.Execute(It.IsAny<IComputerType>())).Callback<IComputerType>(ct => arguments = ct);
            var robot = new TestableRobot("AZ15");
            robot.Computer.AddProgram(mockProgram.Object);

            var result = robot.Computer.EvaluateInstruction("program(1.15,\"horse\")");
            result.Should().BeOfType<ComputerTypeVoid>();
            arguments.Should().NotBeNull();
            var listArgs = (ComputerTypeList)arguments;
            var floatArg = listArgs.Value[0] as ComputerTypeFloat;
            floatArg.Value.Should().Be(1.15f);
            var stringArg = listArgs.Value[1] as ComputerTypeString;
            stringArg.Value.Should().Be("horse");
        }

        [TestMethod]
        public void ARunningProgramCanChangePropertiesOfDefaultComponent()
        {
            var engine = new ProgrammableEngine();
            var program = GenericProgram.FromCode("fullspeedahead", "throttle = 1.0");
            var computer = new Computer.Computer(null, engine, "computer");

            computer.SetCurrentProgram(program);
            computer.ExecuteNextProgramStatement();
            engine.Throttle.Should().Be(1.0f);
        }

        [TestMethod]
        public void ARunningProgramCanChangePropertiesOfProxyComponents()
        {
            var engine = new ProgrammableEngine();
            var instruction = string.Format("{0}.throttle = 1.0", engine.Name);
            var program = GenericProgram.FromCode("fullspeedahead", instruction);

            var computer = new Computer.Computer(null, new NullProgrammableComponent(), "computer");
            computer.AddProxyComponents(new [] {engine});

            computer.SetCurrentProgram(program);
            computer.ExecuteNextProgramStatement();
            engine.Throttle.Should().Be(1.0f);
        }

        [TestMethod]
        public void AProgramCanInspectRobotProperties()
        {
            var robot = new RepairBot("AZ15");
            var computer = new Computer.Computer(null, robot, "computer");
            var result = computer.EvaluateInstruction("mass") as ComputerTypeFloat;
            result.Value.Should().Be(robot.Mass);
        }
    }
}
