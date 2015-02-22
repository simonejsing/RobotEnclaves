using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using FluentAssertions;

    [TestClass]
    public class TimerTests
    {
        [TestMethod]
        public void FrequencyTimerReportsCountsPerSecond()
        {
            var freq = new FrequencyTimeCounter(10);
            for (int i = 0; i < 10; i++)
            {
                freq.Update(100);
            }

            freq.Frequency.Should().BeInRange(9.9f, 10.1f);
        }

        [TestMethod]
        public void AverageTimerReportsAverageMilliseconds()
        {
            var avg = new AverageTimeCounter(10);
            for (int i = 0; i < 10; i++)
            {
                avg.Update(10);
            }

            avg.Average.Should().BeInRange(9.9f, 10.1f);
        }
    }
}
