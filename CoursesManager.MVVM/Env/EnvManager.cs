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
            .WithEnvFiles(FindEnvFiles().ToArray())
            .Load();

        LoadValues(model);
        return model;
    });

    private static readonly Dictionary<string, string> _envData = new();

    public static T Values => _values.Value;

    

    private static void LoadValues(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var envFields = model.GetType().GetFields().Where(f => f.IsPublic);

        foreach (var field in envFields)
        {
            var envKey = field.Name;

            if (!EnvReader.HasValue(envKey)) throw new Exception($"Environment value with key: {envKey} not found.");

            object value = field.FieldType.Name switch
            {
                nameof(Int32) => EnvReader.GetIntValue(envKey),
                nameof(Boolean) => EnvReader.GetBooleanValue(envKey),
                nameof(Decimal) => EnvReader.GetDecimalValue(envKey),
                nameof(Double) => EnvReader.GetDoubleValue(envKey),
                nameof(String) => EnvReader.GetStringValue(envKey),
                _ => throw new Exception($"Invalid type in model: {field.FieldType}")
            };

            field.SetValue(model, value);

            
            _envData[envKey] = value.ToString()!;
        }
    }

    public T Load()
    {
        var model = new T();

        DotEnv.Fluent()
            .WithExceptions()
            .WithEnvFiles(FindEnvFiles().ToArray())
            .Load();

        LoadValues(model);
        return model;
    }

    public void Save(T config)
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        var envFields = config.GetType().GetFields().Where(f => f.IsPublic);

        foreach (var field in envFields)
        {
            var envKey = field.Name;
            var value = field.GetValue(config)?.ToString();

            if (!string.IsNullOrWhiteSpace(value))
            {
                _envData[envKey] = value;
            }
        }

        SaveToFile();
    }



    public static void UpdateValue(string key, string value)
    {
        if (!_envData.ContainsKey(key))
        {
            throw new Exception($"Key '{key}' bestaat niet in de .env-data.");
        }

        _envData[key] = value;
    }

    public static void SaveToFile()
    {
        var startDirectory = Directory.GetCurrentDirectory();
        var envFilePath = Path.Combine(startDirectory, ".env");

        try
        {
            using var writer = new StreamWriter(envFilePath);
            foreach (var kvp in _envData)
            {
                writer.WriteLine($"{kvp.Key}={kvp.Value}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while saving .env file: {ex.Message}");
        }
    }



    private static List<string> FindEnvFiles()
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

        return envFiles;
    }
}