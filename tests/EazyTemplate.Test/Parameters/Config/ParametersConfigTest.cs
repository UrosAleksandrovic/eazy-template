using EazyTemplate.Parameters.Config;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace EazyTemplate.Test.Parameters.Config;

public class ParametersConfigTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void OpeningRegexAndClosingRegex_EmptyOpeningRegex_ArgumenNullException(string input)
    {
        //Arrange
        var customClosingRegex = @"\)\)\)";

        //Act
        void a() => new ParametersConfig(input, customClosingRegex);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void OpeningRegexAndClosingRegex_EmptyuClosingRegex_ArgumenNullException(string input)
    {
        //Arrange
        var customOpeningRegex = @"\(\(\(";

        //Act
        void a() => new ParametersConfig(customOpeningRegex, input);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void OpeningRegexAndClosingRegex_CustomConstructor_CustomRegex()
    {
        //Arrange
        var customOpeningRegex = @"\(\(\(";
        var customClosingRegex = @"\)\)\)";
        var paramConfig = new ParametersConfig(customOpeningRegex, customClosingRegex);

        //Assert
        Assert.Equal(customOpeningRegex, paramConfig.OpeningRegex);
        Assert.Equal(customClosingRegex, paramConfig.ClosingRegex);
    }

    [Fact]
    public void OpeningRegexAndClosingRegex_DefaultConstructor_DefaultRegex()
    {
        //Arrange
        var paramConfig = new ParametersConfig();

        //Assert
        Assert.Equal(@"\[\[\[", paramConfig.OpeningRegex);
        Assert.Equal(@"\]\]\]", paramConfig.ClosingRegex);
    }

    [Fact]
    public void OpeningAndClosingPattern_DefaultConstructor_DefaultPattern()
    {
        //Arrange
        var paramConfig = new ParametersConfig();

        //Assert
        Assert.Equal(Regex.Unescape(@"\[\[\["), paramConfig.OpeningPattern);
        Assert.Equal(Regex.Unescape(@"\]\]\]"), paramConfig.ClosingPattern);
    }

    [Fact]
    public void OpeningAndClosingPattern_CustomPattern_WithoutEscapeCharacters()
    {
        //Arrange
        var customOpeningRegex = @"\(\(\(";
        var customClosingRegex = @"\)\)\)";
        var paramConfig = new ParametersConfig(customOpeningRegex, customClosingRegex);

        //Assert
        Assert.Equal(Regex.Unescape(customOpeningRegex), paramConfig.OpeningPattern);
        Assert.Equal(Regex.Unescape(customClosingRegex), paramConfig.ClosingPattern);
    }

    [Fact]
    public void PopulateUnknownParameters_DefaultConstructor_SetToFalse()
    {
        //Arrange
        var paramConfig = new ParametersConfig();

        //Assert
        Assert.False(paramConfig.PopulateUnknownParameters);
    }

    [Fact]
    public void HandleUnknownParameters_SetsPropToTrue()
    {
        //Arrange
        var paramConfig = new ParametersConfig();

        //Act
        paramConfig.HandleUnknownParameters();

        //Assert
        Assert.True(paramConfig.PopulateUnknownParameters);
    }
}
