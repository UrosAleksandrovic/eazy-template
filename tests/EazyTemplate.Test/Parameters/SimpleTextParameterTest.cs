using EazyTemplate.Core;
using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters;
using EazyTemplate.Parameters.Config;
using System;
using Xunit;

namespace EazyTemplate.Test.Parameters;

public class SimpleTextParameterTest
{
    private readonly ParametersConfig _paramConfig;
    private readonly TextEvaluatorConfig _evaluatorConfig;

    public SimpleTextParameterTest()
    {
        _paramConfig = new ParametersConfig();
        _evaluatorConfig = new TextEvaluatorConfig();
    }

    [Fact]
    public void FindSimpleProperty_RootIsNull_ArgumentNullException()
    {
        //Arrange
        var path = "root.Id";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);

        //Act
        void a() => parameter.FindSimpleProperty(null, typeof(TestEntity));

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void FindSimpleProperty_TypeIsNull_ArgumentNullException()
    {
        //Arrange
        var path = "root.Id";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");

        //Act
        void a() => parameter.FindSimpleProperty(entity, null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void FindSimpleProperty_ParameterIsRoot_InvalidPropertyPathException()
    {
        //Arrange
        var path = "root";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");

        //Act
        void a() => parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        var exception = Assert.Throws<InvalidPropertyPathException>(a);
        Assert.Equal("Property path must have root attribute.", exception.Message);
    }

    [Fact]
    public void FindSimpleProperty_PropertyIsUnsupportedEnumerable_InvalidPropertyTypeException()
    {
        //Arrange
        var path = "root.EnumerableTest";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");
        entity.EnumerableTest = new string[] { "Test1" };

        //Act
        void a() => parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Throws<InvalidPropertyTypeException>(a);
    }

    [Fact]
    public void FindSimpleProperty_PropertyOnPathIsEnumerable_InvalidPropertyTypeException()
    {
        //Arrange
        var path = "root.EnumerableTest.Something";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");
        entity.EnumerableTest = new string[] { "Test1" };

        //Act
        void a() => parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        var exception = Assert.Throws<InvalidPropertyTypeException>(a);
        Assert.Equal("SimpleTextResolver does not support enumerable properties on path.", exception.Message);
    }

    [Fact]
    public void FindSimpleProperty_PropertyDoesNotExist_ReturnsBothNulls()
    {
        //Arrange
        var path = "root.ThisDoesNotExist";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");

        //Act
        var result = parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Null(result.Item1);
        Assert.Null(result.Item2);
    }

    [Fact]
    public void FindSimpleProperty_PropertyOnThePathDoesNotExist_ReturnsBothNulls()
    {
        //Arrange
        var path = "root.NestedExample.ThisDoesNotExist";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");
        entity.NestedExample = new TestEntity("id2");

        //Act
        var result = parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Null(result.Item1);
        Assert.Null(result.Item2);
    }

    [Fact]
    public void FindSimpleProperty_PropertyIsCustomType_InvalidPropertyTypeException()
    {
        //Arrange
        var path = "root.NestedExample";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");
        entity.NestedExample = new TestEntity("someId");

        //Act
        void a() => parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Throws<InvalidPropertyTypeException>(a);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void FindSimpleProperty_ParameterPathIsEmpty_InvalidPropertyPathException(string invalidInput)
    {
        //Arrange
        var parameter = new SimpleTextParameter(invalidInput, 0, invalidInput != null ? invalidInput.Length + 6 : 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");

        //Act
        void a() => parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        var exception = Assert.Throws<InvalidPropertyPathException>(a);
        Assert.Equal("Property path must have root attribute.", exception.Message);
    }

    [Fact]
    public void FindSimpleProperty_ValueOnPathIsNull_ReturnsNullValueWithType()
    {
        //Arrange
        var path = "root.NestedExample.Id";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");

        //Act
        var result = parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Null(result.Item1);
        Assert.NotNull(result.Item2);
        Assert.Equal(typeof(TestEntity), result.Item2);
    }

    [Fact]
    public void FindSimpleProperty_ValueOfThePropertyIsNull_ReturnsNullValueWithType()
    {
        //Arrange
        var path = "root.NestedExample.NullTestProp";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id");
        entity.NestedExample = new TestEntity("new id");

        //Act
        var result = parameter.FindSimpleProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Null(result.Item1);
        Assert.NotNull(result.Item2);
        Assert.Equal(typeof(string), result.Item2);
    }

    [Fact]
    public void Evaluate_RootIsNull_ReturnsParameterText()
    {
        //Arrange
        var path = "root.EnumerableTest";
        var parameter = new SimpleTextParameter(path, 0, 24, _paramConfig, _evaluatorConfig);

        //Act
        var result = parameter.Evaluate(null, typeof(TestEntity));

        //Assert
        Assert.Equal("[[[root.EnumerableTest]]]", result);
    }

    [Fact]
    public void Evaluate_PropertyIsEnumerableSimpleValue_ReturnsResultForThatValue()
    {
        //Arrange
        var path = "item.Value";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("id")
        {
            EnumerableTest = new string[] { "Test1", "Test2" }
        };

        //Act
        var result = parameter.Evaluate(entity.EnumerableTest[1], typeof(string));

        //Assert
        Assert.Equal(entity.EnumerableTest[1], result);
    }

    [Theory]
    [InlineData("root.ThisDoesNotExist")]
    [InlineData("root.NestedExample.ThisDeosNotExist")]
    public void Evaluate_PropertyNotFound_PopulateUnknownParameters(string testInput)
    {
        //Arrange
        var paramConfig = new ParametersConfig();
        paramConfig.HandleUnknownParameters();
        var parameter = new SimpleTextParameter(testInput, 0, testInput.Length + 6, paramConfig, _evaluatorConfig);
        var entity = new TestEntity("SomeId");
        entity.NestedExample = new TestEntity("SomeId2");

        //Act
        var result = parameter.Evaluate(entity, typeof(TestEntity));

        //Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("root.ThisDoesNotExist")]
    [InlineData("root.NestedExample.ThisDeosNotExist")]
    public void Evaluate_PropertyNotFound_ReturnParameterText(string testInput)
    {
        //Arrange
        var parameter = new SimpleTextParameter(testInput, 0, testInput.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("SomeId");
        entity.NestedExample = new TestEntity("SomeId2");

        //Act
        var result = parameter.Evaluate(entity, typeof(TestEntity));

        //Assert
        Assert.Equal($"[[[{testInput}]]]", result);

    }

    [Fact]
    public void Evaluate_ValidPropertyPath_ReturnResultEvaluatedByEvaluatorConfig()
    {
        //Arrange
        var path = "root.Id";
        var parameter = new SimpleTextParameter(path, 0, path.Length + 6, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("SomeId");
        var expectedResult = _evaluatorConfig.GetForBuiltInType(entity.Id.GetType()).GetResolver().Invoke(entity.Id);

        //Act
        var result = parameter.Evaluate(entity, typeof(TestEntity));

        //Assert
        Assert.Equal(expectedResult, result);
    }
}