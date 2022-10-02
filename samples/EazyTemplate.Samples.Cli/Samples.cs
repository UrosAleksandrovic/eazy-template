using System.Runtime.ExceptionServices;
using System.Text;

namespace EazyTemplate.Samples.Cli;

public static class Samples
{
    public static void SimpleParameterExample()
    {
        var textTemplate = "This is our simple parameter [[[root.SimpleParameter]]]";

        var testObject = new { SimpleParameter = "Hello world, I am simple property!" };

        var textBuilder = new TextBuilder();
        textBuilder.HasTemplate(textTemplate);
        var result = textBuilder.BuildBody(testObject);

        WriteTemplateAndResult(textTemplate, result);
    }

    public static void ComplexParameterExample()
    {
        var templateBuilder = new StringBuilder();
        templateBuilder.AppendLine("This is our complex parameter [[[root.Person as persona:");
        templateBuilder.AppendLine("First name: [[[persona.FirstName]]]");
        templateBuilder.Append("Last name: [[[persona.FirstName]]]");
        templateBuilder.Append(":root.Person]]]");
        var textTemplate = templateBuilder.ToString();

        var testObject = new
        {
            Person = new
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };

        var textBuilder = new TextBuilder();
        textBuilder.HasTemplate(textTemplate);
        var result = textBuilder.BuildBody(testObject);

        WriteTemplateAndResult(textTemplate, result);
    }

    public static void EnumerableParameterExample()
    {
        var templateBuilder = new StringBuilder();
        templateBuilder.AppendLine("These are all days in a week: [[[root.DaysOfWeek as day:");
        templateBuilder.Append("[[[day.value]]]");
        templateBuilder.Append(":root.DaysOfWeek]]]");
        var textTemplate = templateBuilder.ToString();

        var testObject = new
        {
            DaysOfWeek = Enum.GetNames(typeof(DayOfWeek))
        };

        var textBuilder = new TextBuilder();
        textBuilder.HasTemplate(textTemplate);
        var result = textBuilder.BuildBody(testObject);

        WriteTemplateAndResult(textTemplate, result);
    }

    public static void ComplexEnumerableParameterExample()
    {
        var templateBuilder = new StringBuilder();
        templateBuilder.AppendLine("These are all employees: [[[root.Employees as employee:");
        templateBuilder.AppendLine("Id: [[[employee.Id]]]");
        templateBuilder.AppendLine("First name: [[[employee.FirstName]]]");
        templateBuilder.Append("Last name: [[[employee.FirstName]]]");
        templateBuilder.Append(":root.Employees]]]");
        var textTemplate = templateBuilder.ToString();

        var testObject = new
        {
            Employees = new[]
            {
                new
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe"
                },
                new
                {
                    Id = 2,
                    FirstName = "James",
                    LastName = "Smith",
                }
            }
        };

        var textBuilder = new TextBuilder();
        textBuilder.HasTemplate(textTemplate);
        var result = textBuilder.BuildBody(testObject);

        WriteTemplateAndResult(textTemplate, result);
    }

    public static void ConfigurationExample()
    {
        var templateBuilder = new StringBuilder();
        templateBuilder.AppendLine("Configured DateTime: (((root.ExampleDate###");
        templateBuilder.AppendLine("Configured Boolean True: (((root.ExampleBoolOne###");
        templateBuilder.AppendLine("Configured Boolean False: (((root.ExampleBoolTwo###");
        templateBuilder.Append("Unknown example is empty string: \"(((root.NonExistant###\"");
        var textTemplate = templateBuilder.ToString();

        var testObject = new
        {
            ExampleDate = DateTime.Now,
            ExampleBoolOne = true,
            ExampleBoolTwo = false,
        };
        
        var textBuilder = new TextBuilder();
        textBuilder.UseParametersConfiguration(cfg =>
        {
            cfg.UseOpeninAndClosingRegex(@"\(\(\(", @"###");
            cfg.UseEmptyStringForUnKnownProperties();
        });

        textBuilder.UseTextEvaluatorConfig(cfg =>
        {
            cfg.UseTypeResolver<DateTime>(dt => dt.ToString("dd/MMM/yy z"));
            cfg.UseTypeResolver<bool>(dt => dt ? "Awesome" : "Bad");
        });
        textBuilder.HasTemplate(textTemplate);
        var result = textBuilder.BuildBody(testObject);

        WriteTemplateAndResult(textTemplate, result);
    }

    private static void WriteTemplateAndResult(string template, string result)
    {
        var divider = "------------------------------";
        var outputBuilder = new StringBuilder();

        outputBuilder.AppendLine("Template"); outputBuilder.AppendLine(divider);
        outputBuilder.AppendLine(template); outputBuilder.AppendLine(divider);

        outputBuilder.AppendLine("Result"); outputBuilder.AppendLine(divider);
        outputBuilder.AppendLine(result); outputBuilder.AppendLine(divider);

        Console.WriteLine(outputBuilder.ToString());
    }
}
