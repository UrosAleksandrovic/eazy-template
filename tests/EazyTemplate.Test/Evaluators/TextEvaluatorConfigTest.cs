using EazyTemplate.Core;
using EazyTemplate.Evaluators.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace EazyTemplate.Test.Evaluators;

public class TextEvaluatorConfigTest
{
    [Fact]
    public void GetForBuiltInType_HasRegisteredFunction_FindsProperFunction()
    {
        //Arrange
        var expectedFunction = (DateTime value) => value.ToString(CultureInfo.InvariantCulture);
        var listOfResolvers = new List<ITextEvaluatorWrapper>();
        listOfResolvers.Add(new TextEvaluatorWrapper<DateTime>(expectedFunction));
        var evaluatorConfig = new TextEvaluatorConfig(listOfResolvers);
        var testDate = DateTime.UtcNow;

        //Act
        var expectedResult = expectedFunction.Invoke(testDate);
        var foundWrapper = evaluatorConfig.GetForBuiltInType(typeof(DateTime));
        Assert.NotNull(foundWrapper);
        var result = foundWrapper.GetResolver().Invoke(testDate);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetForBuiltInType_DoesNotHaveRegisteredFunction_GetsDefaultAction()
    {
        //Arrange
        var expectedFunction = (DateTime value) => string.Empty;
        var listOfResolvers = new List<ITextEvaluatorWrapper>();

        var evaluatorConfig = new TextEvaluatorConfig(listOfResolvers);
        var testDate = DateTime.UtcNow;

        //Act
        var expectedResult = expectedFunction.Invoke(testDate);
        var foundWrapper = evaluatorConfig.GetForBuiltInType(typeof(DateTime));
        Assert.NotNull(foundWrapper);
        var result = foundWrapper.GetResolver().Invoke(testDate);

        //Assert
        Assert.NotNull(foundWrapper);
        Assert.NotEqual(expectedResult, result);
    }

    [Fact]
    public void Constructor_TypeIsNotSupported_Exception()
    {
        //Arrange
        var expectedFunction = (Type value) => value?.ToString();
        var listOfFunctions = new List<ITextEvaluatorWrapper>();
        listOfFunctions.Add(new TextEvaluatorWrapper<Type>(expectedFunction));

        //Act
        void a() => new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.Throws<EazyTemplateException>(a);
    }

    [Fact]
    public void Constructor_TypeIsAlreadyRegistered_Exception()
    {
        //Arrange
        var expectedFunction = (int value) => value.ToString();
        var listOfFunctions = new List<ITextEvaluatorWrapper>();
        listOfFunctions.Add(new TextEvaluatorWrapper<int>(expectedFunction));
        listOfFunctions.Add(new TextEvaluatorWrapper<int>(expectedFunction));

        //Act
        void a() => new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.Throws<EazyTemplateException>(a);
    }
}
