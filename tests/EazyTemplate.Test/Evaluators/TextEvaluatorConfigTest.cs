using EazyTemplate.Core;
using EazyTemplate.Evaluators.Config;
using System;
using System.Collections.Generic;
using Xunit;

namespace EazyTemplate.Test.Evaluators;

public class TextEvaluatorConfigTest
{
    [Fact]
    public void GetForBuiltInType_HasRegisteredFunction_FindsProperFunction()
    {
        //Arrange
        var expectedFunction = (object value) => value?.ToString();
        var listOfFunctions = new List<(Type, Func<object, string>)>();
        listOfFunctions.Add((typeof(int), expectedFunction));

        var evaluatorConfig = new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.StrictEqual(expectedFunction, evaluatorConfig.GetForBuiltInType(typeof(int)));
    }

    [Fact]
    public void GetForBuiltInType_DoesNotHaveRegisteredFunction_GetsDefaultAction()
    {
        //Arrange
        var expectedFunction = (object value) => value?.ToString();
        var listOfFunctions = new List<(Type, Func<object, string>)>();

        var evaluatorConfig = new TextEvaluatorConfig(listOfFunctions);

        //Act
        var foundFunction = evaluatorConfig.GetForBuiltInType(typeof(string));

        //Assert
        Assert.NotNull(foundFunction);
        Assert.NotEqual(expectedFunction, foundFunction);
    }

    [Fact]
    public void Constructor_TypeIsNotSupported_Exception()
    {
        //Arrange
        var expectedFunction = (object value) => value?.ToString();
        var listOfFunctions = new List<(Type, Func<object, string>)>();
        listOfFunctions.Add((typeof(Type), expectedFunction));

        //Act
        void a() => new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.Throws<EazyTemplateException>(a);
    }

    [Fact]
    public void Constructor_TypeIsAlreadyRegistered_Exception()
    {
        //Arrange
        var expectedFunction = (object value) => value?.ToString();
        var listOfFunctions = new List<(Type, Func<object, string>)>();
        listOfFunctions.Add((typeof(int), expectedFunction));
        listOfFunctions.Add((typeof(int), expectedFunction));

        //Act
        void a() => new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.Throws<EazyTemplateException>(a);
    }

    [Fact]
    public void Constructor_NewTypeIsPassed_FunctionRegistered()
    {
        //Arrange
        var expectedFunction = (object value) => value?.ToString();
        var listOfFunctions = new List<(Type, Func<object, string>)>();
        listOfFunctions.Add((typeof(int), expectedFunction));
        listOfFunctions.Add((typeof(DateTime), expectedFunction));

        //Act
        var result = new TextEvaluatorConfig(listOfFunctions);

        //Assert
        Assert.NotNull(result.GetForBuiltInType(typeof(int)));
        Assert.NotNull(result.GetForBuiltInType(typeof(DateTime)));
    }
}
