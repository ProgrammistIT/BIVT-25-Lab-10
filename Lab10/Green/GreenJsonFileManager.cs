using System.Reflection;
using System.Text.Json;

namespace Lab10.Green;

public class GreenJsonFileManager : GreenFileManager
{
    public GreenJsonFileManager(string name) : base(name)
    { }
    public GreenJsonFileManager(string name, string folderPath, string fileName, string fileExtension = "")
        : base(name, folderPath, fileName, fileExtension)
    { }
    public override void EditFile(string content)
    {
        if (!File.Exists(FullPath)) return;
        var obj = Deserialize<Lab9.Green.Green>();
        if (obj == null) return;
        obj.ChangeText(content);
        Serialize(obj);
    }

    public override void ChangeFileExtension(string newExtension) => ChangeFileFormat("json");

    public override void Serialize<T>(T obj)
    {
        if (obj == null || string.IsNullOrEmpty(FullPath)) return;

        var data = new Dictionary<string, object?> { ["TypeName"] = obj.GetType().FullName, ["Input"] = obj.Input };

        for (var type = obj.GetType(); type != null && type != typeof(object); type = type.BaseType)
            foreach (var f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                data.TryAdd(f.Name, f.GetValue(obj));

        File.WriteAllText(FullPath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
    }

    public override T Deserialize<T>()
    {
        if (!File.Exists(FullPath)) return null;
        try
        {
            var raw = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(FullPath),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (raw == null) return null;

            var dict = raw.ToDictionary(kv => kv.Key, kv => kv.Value.ToString(), StringComparer.OrdinalIgnoreCase);

            var actualType = typeof(T);
            if (dict.TryGetValue("TypeName", out var typeName))
                actualType = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetType(typeName)).FirstOrDefault(t => t != null) ?? actualType;

            object? obj = null;
            foreach (var ctor in actualType
                         .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .OrderByDescending(c => c.GetParameters().Length))
            {
                try
                {
                    var args = ctor.GetParameters().Select(p =>
                        dict.Keys.FirstOrDefault(k => k.Equals(p.Name, StringComparison.OrdinalIgnoreCase)
                                                      || k.Equals("_" + p.Name, StringComparison.OrdinalIgnoreCase)) is
                            { } match
                            ? ConvertValue(dict[match], p.ParameterType)
                            : p.HasDefaultValue
                                ? p.DefaultValue
                                : string.Empty).ToArray();
                    obj = ctor.Invoke(args);
                    break;
                }
                catch
                {
                }
            }

            if (obj == null) return null;

            for (var t = actualType; t != null && t != typeof(object); t = t.BaseType)
                foreach (var f in t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    if (dict.TryGetValue(f.Name, out var val))
                        try
                        {
                            f.SetValue(obj, ConvertValue(val, f.FieldType));
                        }
                        catch
                        {
                        }

            var result = (T)obj;
            result?.Review();
            return result;
        }
        catch { return null; }
    }

    private static object? ConvertValue(string val, Type t)
    {
        if (t == typeof(string)) return val;
        if (t == typeof(int) && int.TryParse(val, out var i)) return i;
        if (t == typeof(double) && double.TryParse(val, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var d)) return d;
        if (t == typeof(bool) && bool.TryParse(val, out var b)) return b;
        return null;
    }
}