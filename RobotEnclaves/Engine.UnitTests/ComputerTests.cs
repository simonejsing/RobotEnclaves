using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.UnitTests
{
    using Engine.Computer;
    using Engine.Exceptions;
    using FluentAssertions;

    [TestClass]
    public class ComputerTests
    {
        [TestMethod]
        public void TypeSystemFloatConvertsToString()
        {
            const float value = 1.12f;
            ComputerType floatType = new ComputerTypeFloat(value);
            floatType.ToString().Should().Be(value.ToString());
        }

        [TestMethod]
        public void TypeSystemCanParseFloatValue()
        {
            const string value = "1.12";
            var floatType = ComputerType.Parse(value);
            floatType.Should().BeOfType<ComputerTypeFloat>();
        }

        [TestMethod]
        public void TypeSystemCanParseStringValue()
        {
            const string value = "\"test\"";
            var stringType = ComputerType.Parse(value);
            stringType.Should().BeOfType<ComputerTypeString>();
            stringType.ToString().Should().Be("test");
        }

        [TestMethod]
        public void TypeSystemCanParseBooleanTrueValue()
        {
            const string value = "true";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeBoolean>();
            (type as ComputerTypeBoolean).Value.Should().BeTrue();
        }

        [TestMethod]
        public void TypeSystemCanParseBooleanFalseValue()
        {
            const string value = "false";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeBoolean>();
            (type as ComputerTypeBoolean).Value.Should().BeFalse();
        }

        [TestMethod]
        public void TypeSystemCanParseListOfFloats()
        {
            const string value = "1.12,2.43";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeList>();
            var listType = type as ComputerTypeList;
            listType.Value.Count.Should().Be(2);
            listType.Value[0].Should().BeOfType<ComputerTypeFloat>();
            (listType.Value[0] as ComputerTypeFloat).Value.Should().Be(1.12f);
            listType.Value[1].Should().BeOfType<ComputerTypeFloat>();
            (listType.Value[1] as ComputerTypeFloat).Value.Should().Be(2.43f);
        }

        [TestMethod]
        public void TypeSystemCanParseListOfStrings()
        {
            const string value = "\"abc\",\"def\"";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeList>();
            var listType = type as ComputerTypeList;
            listType.Value.Count.Should().Be(2);
            listType.Value[0].Should().BeOfType<ComputerTypeString>();
            (listType.Value[0] as ComputerTypeString).Value.Should().Be("abc");
            listType.Value[1].Should().BeOfType<ComputerTypeString>();
            (listType.Value[1] as ComputerTypeString).Value.Should().Be("def");
        }

        [TestMethod]
        public void TypeSystemCanParseListOfFloatAndString()
        {
            const string value = "1.12,\"def\"";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeList>();
            var listType = type as ComputerTypeList;
            listType.Value.Count.Should().Be(2);
            listType.Value[0].Should().BeOfType<ComputerTypeFloat>();
            (listType.Value[0] as ComputerTypeFloat).Value.Should().Be(1.12f);
            listType.Value[1].Should().BeOfType<ComputerTypeString>();
            (listType.Value[1] as ComputerTypeString).Value.Should().Be("def");
        }

        [TestMethod]
        public void TypeSystemCanParseInteger()
        {
            const string value = "12";
            var type = ComputerType.Parse(value);
            type.Should().BeOfType<ComputerTypeInt>();
            var intType = type as ComputerTypeInt;
            intType.Value.Should().Be(12);
        }

        [TestMethod]
        public void TypeSystemCanCastIntegerToFloat()
        {
            IComputerType intType = new ComputerTypeInt(12);
            ComputerTypeFloat floatType = intType.Cast<ComputerTypeFloat>() as ComputerTypeFloat;

            floatType.Value.Should().Be(12f);
        }

        [TestMethod]
        public void CastingIntegerToStringThrowsInvalidCastException()
        {
            IComputerType intType = new ComputerTypeInt(12);
            Action action = () => intType.Cast<ComputerTypeString>();

            action.ShouldThrow<ComputerInvalidCastException>();
        }
    }
}
