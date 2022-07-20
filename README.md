# Eazy Template

Reflection based library for populating any text template

## What is eazy template for?

If there is some text to be populated based on the instance of a class and it's public properties, then eazy template is a library that helps to accomplish that goal.
The project aims to take care of reflection for the user. All that the user needs to do is to configure the TextBuidler.

## Technologies

.NET 6

## How to use it?

In order to populate a template, you need to instantiate TextBuilder. TextBuilder is configurable with some basic options. Hopefully, with more to come in the future. :blush:
```
    var textBuilder = new TextBuilder();

    textBuilder.UseTextEvaluatorConfig(cfg =>
    {
        cfg.UseTypeResolver(value => ((bool)value) ? "Yes" : "No");
    });

    textBuilder.UseParametersConfiguration(cfg =>
    {
        cfg.UseOpeningAndClosingRegex(@"\(\(\(",@"\)\)\)");
    });
```
In the code example above, textBuilder is configured to handle boolean type of properties in a certain way. By default, ToString() is called any property that is in a text template.

Another configuration is for opening and closing regex, used to find parameters in the text. By default, parameters are opened with **[[[** and closed with **]]]** but as the example shows, this is configurable to anything really. ex. **(((** and **)))**

```
    textBuilder.HasTemplate(textTemplate);

    textBuilder.Build(object);
```
After configuration, all that is needed is to pass the text template and object that populates it. This is why eazy template is called **eazy**. Nothing more to have in the code. No Regex, no reflection to handle. :blush:

## Preparing template

When populating a template, well, the first thing needed is a template. What is referred as template is any text (string) that does or does not have parameters inside. Parameters need to have **opening**, **closing**, **owner name** and **property name**. Additionally, some parameters may have their own respective template (these are called complex parameters).

Here is some of the most important information for you:

- By default, parameter opening and closing are declared as follows *[[[* *]]]*.
- Every parameter has to have a parent.
- The object passed to Build function is referred as a root in the template. Every parameter that comes from it has to follow '[[[root.**propName**]]]'

#### Simple parameters

Simple parameters are properties that are of C# built in types. Essentially leaves of object trees. You declare them as *[[[parentName.propName]]]*

#### Complex parameters

Complex parameters are parameters that have a template of their own. You can imagine nested objects or enumerables. If interaction through the list of some objects is needed, then creating complex parameters with templates is the solution. 

>*[[[parentName.propName as complexParamName: complexParamTemplate :parentName.propName]]]*

>*[[[root.Items as item: item.Name, item.Price, item.Qty :root.Items]]]*

