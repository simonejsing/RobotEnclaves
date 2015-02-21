using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Spaceship;
    using FluentAssertions;

    [TestClass]
    public class AiTests
    {
        [TestMethod]
        public void AiReturnsUnsuccessfulResultWhenTheSpecifiedRobotDoesNotExist()
        {
            var result = new Ai().InterpretCommand("robot.component.property = 1.0");
            result.Success.Should().BeFalse();
        }
    }
}
