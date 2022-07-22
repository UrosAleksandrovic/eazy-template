using EazyTemplate.Parameters.Config;
using Xunit;

namespace EazyTemplate.Test.Parameters.Config;

public class ParametersConfigBuilderTest
{
    [Fact]
    public void Build_BothRegexAreSet_ReturnsCustomConfig()
    {
        //Arrange
        var customBuilder = new ParametersConfigBuilder();
        customBuilder.UseOpeninAndClosingRegex(@"\(\(\(", @"\)\)\)");

        //Act
        var result = customBuilder.Build();

        //Assert
        Assert.Equal("(((", result.OpeningPattern);
        Assert.Equal(")))", result.ClosingPattern);
    }

    [Fact]
    public void Build_NoneRegexAreSet_ReturnsDefaultConfig()
    {
        //Arrange
        var customBuilder = new ParametersConfigBuilder();
        var defaultBuilder = new ParametersConfigBuilder();
        customBuilder.UseOpeninAndClosingRegex(null, null);

        //Act
        var expectedResult = defaultBuilder.Build();
        var result = customBuilder.Build();

        //Assert
        Assert.Equal(expectedResult.OpeningPattern, result.OpeningPattern);
        Assert.Equal(expectedResult.ClosingPattern, result.ClosingPattern);
    }

    [Fact]
    public void Build_OnlyOneIsSet_ReturnsDefaultConfig()
    {
        //Arrange
        var customBuilder = new ParametersConfigBuilder();
        var defaultBuilder = new ParametersConfigBuilder();
        customBuilder.UseOpeninAndClosingRegex("SomeRegex", null);

        //Act
        var expectedResult = defaultBuilder.Build();
        var result = customBuilder.Build();

        //Assert
        Assert.Equal(expectedResult.OpeningPattern, result.OpeningPattern);
        Assert.Equal(expectedResult.ClosingPattern, result.ClosingPattern);
    }

    [Fact]
    public void Build_HandleForUnknownPropertiesIsSet_BuildWithBoolPropSet()
    {
        //Arrange
        var defaultBuilder = new ParametersConfigBuilder();
        defaultBuilder.UseEmptyStringForUnKnownProperties();

        //Act
        var expectedResult = defaultBuilder.Build();

        //Assert
        Assert.True(expectedResult.PopulateUnknownParameters);
    }

    [Fact]
    public void Build_DefaultConfig_BuildWithUnknownPropNotSet()
    {
        //Arrange
        var defaultBuilder = new ParametersConfigBuilder();

        //Act
        var expectedResult = defaultBuilder.Build();

        //Assert
        Assert.False(expectedResult.PopulateUnknownParameters);
    }
}
