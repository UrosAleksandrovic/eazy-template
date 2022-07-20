using EazyTemplate.Core;
using EazyTemplate.Evaluators.Config;
using EazyTemplate.Parameters;
using EazyTemplate.Parameters.Config;
using System;
using Xunit;

namespace EazyTemplate.Test.Parameters;

public class ComplexTextParameterTest
{
    private readonly ParametersConfig _paramConfig;
    private readonly TextEvaluatorConfig _evaluatorConfig;

    public ComplexTextParameterTest()
    {
        _paramConfig = new ParametersConfig();
        _evaluatorConfig = new TextEvaluatorConfig();
    }

    [Theory]
    [InlineData("")]
    [InlineDataAttribute(" ")]
    [InlineData(null)]
    public void GetOrderedChildParameters_EmptyTemplate_EazyTempalteException(string testInput)
    {
        //Arrange
        var complexParam = new ComplexTextParameter(testInput, _paramConfig, _evaluatorConfig);

        //Act
        void a() => complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Throws<EazyTemplateException>(a);
    }

    [Fact]
    public void GetOrderedChildParameters_NoParameters_EmptyList()
    {
        //Arrange
        var testTemplate = "Text without parameters";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetOrderedChildParameters_HasOnlySimpleParameters_OnlySimpleParams()
    {
        //Arrange
        var testTemplate = "[[[root.Test1]]], [[[root.Test2]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(2, result.Count);

        foreach (var param in result)
            Assert.IsType<SimpleTextParameter>(param);
    }

    [Fact]
    public void GetOrderedChildParameters_HasOnlyComplexParameters_OnlyComplexParams()
    {
        //Arrange
        var testTemplate = "[[[root.Test1 as test: newTemplate :root.Test1]]], [[[root.Test2 as test: newTemplate2 :root.Test2]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(2, result.Count);

        foreach (var param in result)
            Assert.IsType<ComplexTextParameter>(param);
    }

    [Fact]
    public void GetOrderedChildParameters_HasShuffledParameters_CorrectOrderOfParams()
    {
        //Arrange
        var testTemplate = "[[[root.Test1 as test: newTemplate :root.Test1]]], [[[root.Test3]]], [[[root.Test2 as test: newTemplate2 :root.Test2]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(3, result.Count);
        Assert.IsType<ComplexTextParameter>(result[0]);
        Assert.IsType<SimpleTextParameter>(result[1]);
        Assert.IsType<ComplexTextParameter>(result[2]);
    }

    [Fact]
    public void GetOrderedChildParameters_HasFirstAllSimpleParameters_CorrectOrderOfParams()
    {
        //Arrange
        var testTemplate = "[[[root.Test1]]], [[[root.Test3]]], [[[root.Test2 as test: newTemplate2 :root.Test2]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(3, result.Count);
        Assert.IsType<SimpleTextParameter>(result[0]);
        Assert.IsType<SimpleTextParameter>(result[1]);
        Assert.IsType<ComplexTextParameter>(result[2]);
    }

    [Fact]
    public void GetOrderedChildParameters_HasFirstAllComplexParameters_CorrectOrderOfParams()
    {
        var testTemplate = "[[[root.Test1 as test: newTemplate :root.Test1]]],  [[[root.Test2 as test: newTemplate2 :root.Test2]]], [[[root.Test3]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(3, result.Count);
        Assert.IsType<ComplexTextParameter>(result[0]);
        Assert.IsType<ComplexTextParameter>(result[1]);
        Assert.IsType<SimpleTextParameter>(result[2]);
    }

    [Fact]
    public void GetOrderedChildParameters_HasParamsFromAnotherRoot_IgnoresParams()
    {
        //Arrange
        var testTemplate = "[[[someOtherRoot.Test1]]],  [[[root.Test2 as test: newTemplate2 :root.Test2]]], [[[root.Test3]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        var result = complexParam.GetOrderedChildParameters();

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void FindComplexProperty_RootIsNull_ArgumentNullException()
    {
        //Arrange
        var testTemplate = "[[[root.NestedExample as test: [[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]] :root.ComplexTest]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);

        //Act
        void a() => complexParam.FindComplexProperty(null, typeof(TestEntity));

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void FindComplexProperty_RootTypeIsNull_ArgumentNullException()
    {
        //Arrange
        var testTemplate = "[[[root.NestedExample as test: [[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]] :root.ComplexTest]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("TestId");

        //Act
        void a() => complexParam.FindComplexProperty(entity, null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void FindComplexProperty_PathForParameterIsEmpty_InvalidParameterPathException(string testInput)
    {
        //Arrange
        var testTemplate = "[[[root.NestedExample as test: [[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]] :root.ComplexTest]]]";
        var complexParam = new ComplexTextParameter(testInput, testInput, testTemplate, 0, 0, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("TestId");

        //Act
        void a() => complexParam.FindComplexProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Throws<InvalidPropertyPathException>(a);
    }

    [Fact]
    public void FindComplexProperty_ParameterIsRoot_ReturnRootObject()
    {
        //Arrange
        var testTemplate = "[[[root.NestedExample as test: [[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]] :root.ComplexTest]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("TestId");

        //Act
        var result = complexParam.FindComplexProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Equal(result.Item1, entity);
        Assert.Equal(typeof(TestEntity), result.Item2);
    }

    [Fact]
    public void FindComplexProperty_PropertyDoesNotExist_InvalidPropertyPathException()
    {
        //Arrange
        var testTemplate = "[[[root.ThisDoesNotExist as test: [[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]] :root.ThisDoesNotExist]]]";
        var complexParam = new ComplexTextParameter(testTemplate, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("TestId");

        //Act
        var result = complexParam.FindComplexProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Equal(result.Item1, entity);
        Assert.Equal(typeof(TestEntity), result.Item2);
    }

    [Fact]
    public void FindComplexProperty_PropertyHasNullValue_NullValueReturned()
    {
        //Arrange
        var testTemplate = "[[[test.Id]]], [[[test.CreatedOn as test2: test2.Month :test.ComplexTest]]]";
        var complexParam = new ComplexTextParameter("root.NullTestProp", "test", testTemplate, 0, 0, _paramConfig, _evaluatorConfig);
        var entity = new TestEntity("TestId");
        entity.NestedExample = new TestEntity("2");

        //Act
        var result = complexParam.FindComplexProperty(entity, typeof(TestEntity));

        //Assert
        Assert.Null(result.Item1);
        Assert.Equal(typeof(string), result.Item2);
    }
}
