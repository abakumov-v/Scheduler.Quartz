using System;
using System.Diagnostics.CodeAnalysis;
using Core.Models.Infrastructure;
using Xunit;

namespace Core.Models.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class ConsoleArgumentTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_ArgumentIsNullOrEmpty_ThrowValidException(string argument)
        {
            // Arrange
            Action act = () => new ConsoleArgument(argument);
            // Act
            var ex = Record.Exception(act);
            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        #region Case 1: argument has not separator and value

        [Fact]
        public void Ctor_ArgumentHasNotValue_ValueIsTrue()
        {
            // Arrange
            var argument = "--some-argument";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var value = consoleArgument.Value;
            var valueAsBoolean = Convert.ToBoolean(value);
            // Assert
            Assert.NotEmpty(value);
            Assert.True(valueAsBoolean);
        }
        [Fact]
        public void Ctor_ArgumentHasNotValue_NameIsValid()
        {
            // Arrange
            var argument = "--some-argument";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var name = consoleArgument.Name;
            // Assert
            Assert.Equal(argument, name);
        }
        [Fact]
        public void Ctor_ArgumentHasNotValue_ValueSeparatorPositionLessThat0()
        {
            // Arrange
            var argument = "--some-argument";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var separatorPosition = consoleArgument.ValueSeparatorPosition;
            // Assert
            Assert.True(separatorPosition < 0);
        }

        #endregion

        #region Case 2: Argument has separator, but has not value

        [Fact]
        public void Ctor_ArgumentHasSeparatorCharButHasNotValue_ThrowValidException()
        {
            // Arrange
            var argumentName = "--some-argument";
            var argument = $"{argumentName}=";
            Action act = () => new ConsoleArgument(argument);
            // Act
            var ex = Record.Exception(act);
            // Assert
            Assert.IsType<ArgumentException>(ex);
            Assert.Contains($"The argument \"{argumentName}\" has no value", ex.Message);
        }

        #endregion

        #region Case 3: Argument has separator and value

        [Theory]
        [InlineData("some-value1")]
        [InlineData("123")]
        [InlineData("123,456,789")]
        public void Ctor_ArgumentHasValue_ValueIsTrue(string argumentValue)
        {
            // Arrange
            var argumentName = "--some-argument";
            var argument = $"{argumentName}={argumentValue}";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var value = consoleArgument.Value;
            // Assert
            Assert.Equal(argumentValue, value);
        }
        [Fact]
        public void Ctor_ArgumentHasValue_NameIsValid()
        {
            // Arrange
            var argumentName = "--some-argument";
            var argumentValue = "some-value";
            var argument = $"{argumentName}={argumentValue}";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var name = consoleArgument.Name;
            // Assert
            Assert.Equal(argumentName, name);
        }
        [Fact]
        public void Ctor_ArgumentHasValue_ValueSeparatorPositionIsValid()
        {
            // Arrange
            var argumentName = "--some-argument";
            var argumentValue = "some-value";
            var argument = $"{argumentName}={argumentValue}";
            var consoleArgument = new ConsoleArgument(argument);
            // Act
            var separatorPosition = consoleArgument.ValueSeparatorPosition;
            // Assert
            var validSeparatorPosition = argument.IndexOf('=');
            Assert.Equal(validSeparatorPosition, separatorPosition);
        }

        #endregion
    }
}