using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using System.Linq;
    using Engine.Computer;
    using Engine.Items;
    using Engine.Robotics;
    using FluentAssertions;

    [TestClass]
    public class RepairBotTests
    {
        [TestMethod]
        public void RepairBotHasARepairProgramLoaded()
        {
            IRobot robot = new RepairBot("az15");

            robot.Computer.Programs.Should().Contain(p => p.Name.Equals("repair", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void RepairProgramRepairsADamagedItemCompletely()
        {
            IRobot robot = new RepairBot("az15");
            var item = new CollectableItem("item", "damaged");
            item.ObjectHealth.TakeDamage(20);
            robot.Hull.CargoBay.LoadItem(item);

            var repairProgram = robot.Computer.Programs.First(p => p.Name.Equals("repair", StringComparison.OrdinalIgnoreCase));

            repairProgram.Execute(new ComputerTypeString(item.Name));
            while (!repairProgram.Finished)
            {
                repairProgram.GetNextStatement().Execute(robot.Computer);
            }

            item.ObjectHealth.FullHealth.Should().BeTrue();
        }

        [TestMethod]
        public void RepairBotCanInspectStatusOfItemsInCargoBay()
        {
            Robot robot = new RepairBot("az15");
            var item = new CollectableItem("item", "damaged");
            item.ObjectHealth.TakeDamage(20);
            robot.Hull.CargoBay.LoadItem(item);

            var ct = robot.EvaluateInstruction("inspect(\"item\")");
            ct.ToString().Should().Be("Health: 80/100");
        }
    }
}
