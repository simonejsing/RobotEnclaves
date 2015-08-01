using System;
using System.Security.AccessControl;
using Engine.Exceptions;
using Engine.Network;
using Engine.Robotics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorMath;

namespace Engine.UnitTests
{
    [TestClass]
    public class NetworkTests
    {
        [TestMethod]
        public void RobotCanEstablishLinkToNearbyRobot()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            robot1.Comm.EstablishLink(robot2);

            network.Connections.Should().Contain(l => l.SourceRobot == robot1 && l.TargetRobot == robot2);
        }

        [TestMethod]
        public void RobotCannotEstablishLinkToFarAwayRobot()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            robot2.Object.Position = robot1.Object.Position + new Vector2(robot2.Comm.Range + 1.0f, 0.0f);
            Action action = () => robot1.Comm.EstablishLink(robot2);

            action.ShouldThrow<CommException>();
            network.Connections.Should().NotContain(l => l.SourceRobot == robot1 && l.TargetRobot == robot2);
        }

        [TestMethod]
        public void RobotCannotBeTargetOfMoreThanTwoLinks()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            IRobot robot3 = new CommBot("r3", network);
            IRobot robot4 = new CommBot("r4", network);
            robot2.Comm.EstablishLink(robot1);
            robot3.Comm.EstablishLink(robot1);
            Action action = () => robot4.Comm.EstablishLink(robot1);

            action.ShouldThrow<CommException>();
            network.Connections.Should().NotContain(l => l.SourceRobot == robot4 && l.TargetRobot == robot1);
        }

        [TestMethod]
        public void RobotCanEstablishOnlyTwoLinks()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            IRobot robot3 = new CommBot("r3", network);
            IRobot robot4 = new CommBot("r4", network);
            robot1.Comm.EstablishLink(robot2);
            robot1.Comm.EstablishLink(robot3);
            Action action = () => robot1.Comm.EstablishLink(robot4);

            action.ShouldThrow<CommException>();
            network.Connections.Should().NotContain(l => l.SourceRobot == robot1 && l.TargetRobot == robot4);
        }

        [TestMethod]
        public void RobotCanMessageSelf()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);

            var message = new NetworkMessagePayload();
            robot1.Comm.SendMessage(robot1, message);
            robot1.Comm.Messages.Should().Contain(m => m.Payload == message);
        }

        [TestMethod]
        public void RobotCanMessageAnotherRobotWhenTheyAreLinked()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            robot1.Comm.EstablishLink(robot2);

            var message = new NetworkMessagePayload();
            robot1.Comm.SendMessage(robot2, message);
            robot2.Comm.Messages.Should().Contain(m => m.Payload == message);
        }

        [TestMethod]
        public void RobotCanMessageAnotherRobotWhenTheyAreConnected()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            IRobot robot3 = new CommBot("r3", network);
            robot1.Comm.EstablishLink(robot2);
            robot2.Comm.EstablishLink(robot3);

            var message = new NetworkMessagePayload();
            robot1.Comm.SendMessage(robot3, message);
            robot3.Comm.Messages.Should().Contain(m => m.Payload == message);
        }

        [TestMethod]
        public void RobotCannotMessageAcrossUnconnectedNetworks()
        {
            var network = new NetworkTopology();

            // First splinter network
            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            IRobot robot3 = new CommBot("r3", network);
            robot1.Comm.EstablishLink(robot2);
            robot1.Comm.EstablishLink(robot3);

            // Second splinter network
            IRobot robot1b = new CommBot("r1b", network);
            IRobot robot2b = new CommBot("r2b", network);
            IRobot robot3b = new CommBot("r3b", network);
            robot1b.Comm.EstablishLink(robot2b);
            robot2b.Comm.EstablishLink(robot3b);

            var message = new NetworkMessagePayload();
            // Message internal in first network
            robot1.Comm.SendMessage(robot3, message);
            robot2.Comm.SendMessage(robot3, message);
            robot3.Comm.SendMessage(robot1, message);

            // Message internal in second network
            robot1b.Comm.SendMessage(robot3b, message);
            robot2b.Comm.SendMessage(robot3b, message);
            robot3b.Comm.SendMessage(robot1b, message);

            // Attempt cross network message
            Action action = () => robot3.Comm.SendMessage(robot2b, message);
            action.ShouldThrow<CommException>();
        }

        [TestMethod]
        public void RobotCannotMessageAnotherRobotWhenTheyAreNotLinked()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);

            var message = new NetworkMessagePayload();
            Action action = () => robot1.Comm.SendMessage(robot2, message);
            action.ShouldThrow<CommException>();

            robot2.Comm.Messages.Should().NotContain(message);
        }

        /*[TestMethod]
        public void ThreeRobotsConnectedCreatesAPowerGrid()
        {
            var network = new NetworkTopology();

            IRobot robot1 = new CommBot("r1", network);
            IRobot robot2 = new CommBot("r2", network);
            IRobot robot3 = new CommBot("r3", network);
            robot1.Comm.EstablishLink(robot2);
            robot1.Comm.EstablishLink(robot3);
            robot2.Comm.EstablishLink(robot3);

            network.PowerGrids.Should().Contain();
        }*/

    }
}
