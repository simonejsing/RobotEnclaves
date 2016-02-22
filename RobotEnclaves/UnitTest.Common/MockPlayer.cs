using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameWorld;
using GameWorld.Inputs;
using GameWorld.Objects;
using GameWorld.Objects.ConcreteObjects;
using GameWorld.Objects.Interfaces;
using Moq;

namespace UnitTest.Common
{
    public static class MockPlayer
    {
        public static IPlayer MockedPlayerOnGround
        {
            get
            {
                var mockPlayer = MockObject;
                mockPlayer.SetupGet(p => p.OnGround).Returns(true);
                return mockPlayer.Object;
            }
        }

        public static IPlayer MockedPlayerInAir
        {
            get
            {
                var mockPlayer = MockObject;
                mockPlayer.SetupGet(p => p.OnGround).Returns(false);
                return mockPlayer.Object;
            }
        }

        public static Mock<IPlayer> MockObject
        {
            get
            {
                return new Mock<IPlayer>();
            }
        }

        public static Player GetInstance()
        {
            return new Player(new Color());
        }
    }
}
