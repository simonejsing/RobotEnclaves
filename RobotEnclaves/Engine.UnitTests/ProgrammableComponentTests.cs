using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Robotics;
    using FluentAssertions;

    [TestClass]
    public class ProgrammableComponentTests
    {
        [TestMethod]
        public void CraneReportsRangeCorrectly()
        {
            const float range = 15.0f;
            var crane = new ProgrammableCrane(new Robot("AZ15"), range);

            (crane.EvaluateInstruction("range") as ComputerTypeFloat).Value.Should().Be(range);
        }

        [TestMethod]
        public void EngineReportsThrottleCorrectly()
        {
            const float throttleValue = 0.8f;
            var engine = new ProgrammableEngine();

            engine.EvaluateInstruction(string.Format("throttle = {0}", throttleValue));
            (engine.EvaluateInstruction("throttle") as ComputerTypeFloat).Value.Should().Be(throttleValue);
        }

        [TestMethod]
        public void ChangingEngineThrottleAboveHundredPercentThrowsException()
        {
            const float throttleValue = 1.1f;
            string instruction = string.Format("throttle = {0}", throttleValue);

            var engine = new ProgrammableEngine();

            Action action = () =>
                            {
                                engine.EvaluateInstruction(instruction);
                            };
            action.ShouldThrow<RobotException>();
        }

        [TestMethod]
        public void ChangingCraneRangeThrowsReadOnlyPropertyException()
        {
            const float range = 15.0f;
            var crane = new ProgrammableCrane(new Robot("AZ15"), range);

            Action action = () => crane.EvaluateInstruction("range = 1.0");
            action.ShouldThrow<SettingReadOnlyPropertyException>();
        }
    }
}
