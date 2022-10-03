using EazyTemplate.Evaluators.Config;
using System;
using Xunit;

namespace EazyTemplate.Test.Evaluators;

public class TextEvaluatorConfigBuilderTest
{
    [Fact]
    public void UseTypeResolver_AlreadyPassedType_AddsType()
    {
        //Arrange
        var builder = new TextEvaluatorConfigBuilder();
        builder.UseTypeResolver<int>(v => string.Empty);
        builder.UseTypeResolver<int>(v => string.Empty);

        //Assert
        Assert.Equal(2, builder.RegisteredTypes.Count);
    }

    [Fact]
    public void UseTypeResolver_NewType_AddsType()
    {
        //Arrange
        var builder = new TextEvaluatorConfigBuilder();
        builder.UseTypeResolver<DateTime>(v => string.Empty);
        builder.UseTypeResolver<int>(v => string.Empty);
        builder.UseTypeResolver<decimal>(v => string.Empty);

        //Assert
        var registeredTypes = builder.RegisteredTypes;
        Assert.Equal(3, registeredTypes.Count);
        Assert.Contains(typeof(DateTime), registeredTypes);
        Assert.Contains(typeof(int), registeredTypes);
        Assert.Contains(typeof(decimal), registeredTypes);
    }

    [Fact]
    public void Build_ReturnsCreatedConfig()
    {
        //Arrange
        var builder = new TextEvaluatorConfigBuilder();
        builder.UseTypeResolver<DateTime>(v => string.Empty);
        builder.UseTypeResolver<int>(v => string.Empty);
        builder.UseTypeResolver<decimal>(v => string.Empty);

        //Act
        var result = builder.Build();

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.GetForBuiltInType(typeof(int)));
        Assert.NotNull(result.GetForBuiltInType(typeof(DateTime)));
        Assert.NotNull(result.GetForBuiltInType(typeof(decimal)));
    }
}
