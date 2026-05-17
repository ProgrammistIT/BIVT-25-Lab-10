using System.Text.Json;
using Lab9.Green;

namespace Lab10.Green;

public class GreenJsonFileManager : GreenFileManager
{
    public GreenJsonFileManager(string name) : base(name)
    {
        ChangeFileFormat("json");
    }

    public GreenJsonFileManager(string name, string folderPath, string fileName, string fileExtension = "json")
        : base(name, folderPath, fileName, fileExtension) { }

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
        
        var data = new Dictionary<string, string>
        {
            ["TypeName"] = obj.GetType().Name,
            ["Input"] = obj.Input ?? string.Empty
        };
        
        if (obj is Task3 task3)
            data["Pattern"] = task3.Pattern ?? string.Empty;
        
        File.WriteAllText(FullPath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
    }

    public override T Deserialize<T>()
    {
        if (!File.Exists(FullPath)) return default;
        try
        {
            var dto = JsonSerializer.Deserialize<GreenDto>(File.ReadAllText(FullPath));
            
            if (dto == null) return default;
            
            Lab9.Green.Green? result = dto.TypeName switch
            {
                nameof(Task1) => new Task1(dto.Input),
                nameof(Task2) => new Task2(dto.Input),
                nameof(Task3) => new Task3(dto.Input, dto.Pattern),
                nameof(Task4) => new Task4(dto.Input),
                _ => null
            };
            
            result?.Review();

            return result is T typed ? typed : default;
        }
        catch { return default; }
    }
}