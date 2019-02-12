using System;
using System.Diagnostics.CodeAnalysis;
using Core.Models.Infrastructure;
using Xunit;

namespace Core.Models.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class ConsoleArgumentsModelTests
    {
        #region Property: Args

        [Fact]
        public void Args_ArgsIsNull_ThrowValidException()
        {
            // Arrange
            Action act = () => new ConsoleArgumentsModel(null);
            // Act
            var ex = Record.Exception(act);
            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        #endregion

        #region Property: IsShowHelp

        [Fact]
        public void IsShowHelp_ArgsIsEmpty_ReturnTrue()
        {
            // Arrange
            var args = new string[] { };
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.True(model.IsShowHelp);
        }

        [Theory]
        [InlineData("--service")]
        [InlineData("--start")]
        public void IsShowHelp_ArgsContainsIsService_ReturnTrue(string oneOfRequiredArgument)
        {
            // Arrange
            var args = new [] {oneOfRequiredArgument};
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.False(model.IsShowHelp);
        }

        #endregion

        #region Property: IsNeedStartImmediatelly

        [Fact]
        public void IsNeedStartImmediately_ArgsDoesNotContainIsService_ReturnFalse()
        {
            // Arrange
            var args = new string[] { };
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.False(model.IsNeedStartImmediately);
        }

        [Fact]
        public void IsNeedStartImmediately_ArgsContainsIsService_ReturnTrue()
        {
            // Arrange
            var args = new [] { "--start" };
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.True(model.IsNeedStartImmediately);
        }

        #endregion

        #region Property: IsService

        [Fact]
        public void IsService_ArgsDoesNotContainIsService_ReturnFalse()
        {
            // Arrange
            var args = new string[] { };
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.False(model.IsService);
        }

        [Fact]
        public void IsService_ArgsContainsIsService_ReturnTrue()
        {
            // Arrange
            var args = new [] {"--service"};
            // Act
            var model = new ConsoleArgumentsModel(args);
            // Assert
            Assert.True(model.IsService);
        }

        #endregion
        
    }
}