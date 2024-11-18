using System.IO;
using dotenv.net;
using dotenv.net.Utilities;

namespace CoursesManager.MVVM.Env;

public class EnvManager<T>
    where T : new()
{
    private static readonly Lazy<T> _values = new(() =>
    {
        var model = new T();

        DotEnv.Fluent()
            .WithExceptions()
            .WithEnvFiles(FindEnvFiles())
            .Load();

        LoadValues(model);
        return model;
    });

    public static T Values => _values.Value;

    private EnvManager()
    { }

    private static void LoadValues(T model)
    {
        var envFields = model.GetType().GetFields().Where(f => f.IsPublic);

        foreach (var field in envFields)
        {
            var envKey = field.Name;

            if (!EnvReader.HasValue(envKey)) throw new Exception($"Environment value with key: {envKey} not found.");

            if (field.FieldType == typeof(int) && EnvReader.GetIntValue(envKey) is var intValue)
            {
                field.SetValue(model, intValue);
            }
            else if (field.FieldType == typeof(bool) && EnvReader.GetBooleanValue(envKey) is var boolValue)
            {
                field.SetValue(model, boolValue);
            }
            else if (field.FieldType == typeof(decimal) && EnvReader.GetDecimalValue(envKey) is var decimalValue)
            {
                field.SetValue(model, decimalValue);
            }
            else if (field.FieldType == typeof(double) && EnvReader.GetDoubleValue(envKey) is var doubleValue)
            {
                field.SetValue(model, doubleValue);
            }
            else if (field.FieldType == typeof(string) && EnvReader.GetStringValue(envKey) is var stringValue)
            {
                field.SetValue(model, stringValue);
            }
            else
            {
                throw new Exception($"Invalid type in model: {field.FieldType}");
            }
        }
    }

    private static string[] FindEnvFiles()
    {
        var envFiles = new List<string>();
        var startDirectory = Directory.GetCurrentDirectory();

        try
        {
            var files = Directory.GetFiles(startDirectory, "*.env", SearchOption.AllDirectories);

            envFiles.AddRange(files);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while searching for .env files: {ex.Message}");
        }

        return envFiles.ToArray();
    }
}