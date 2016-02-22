using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhysicsEngine;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    [TestClass]
    public class TemporaryWorldObjectTranslationTests
    {
        [TestMethod]
        public void WorldObjectPositionIsModifiedInScopeOfTemporaryTransformation()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(1, 2) };

            using (new TemporaryObjectTransformation(transformation))
            {
                obj.Position.Should().Be(new Vector2(11, 22));
            }
        }

        [TestMethod]
        public void WorldObjectPositionIsRestoredOnDispose()
        {
            var obj = new WorldBox(new Vector2(10, 20), new Vector2(10, 10));
            var transformation = new ObjectTransformation(obj) { PrimaryTranslation = new Vector2(1, 2) };

            using (new TemporaryObjectTransformation(transformation))
            {
            }

            obj.Position.Should().Be(new Vector2(10, 20));
        }
    }
}
