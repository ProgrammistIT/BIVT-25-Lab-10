using Lab9.Green;

namespace Lab10.Green;

public class GreenTxtFileManager : GreenFileManager
{
    public GreenTxtFileManager(string name) : base(name)
    {
        ChangeFileFormat("txt");
    }

    public GreenTxtFileManager(string name, string folderPath, string fileName, string fileExtension = "txt")
        : base(name, folderPath, fileName, fileExtension) { }

    public override void EditFile(string content)
    {
        if (!File.Exists(FullPath)) return;
        var obj = Deserialize<Lab9.Green.Green>();
        if (obj == null) return;
        obj.ChangeText(content);
        Serialize(obj);
    }

    public override void ChangeFileExtension(string newExtension) => ChangeFileFormat("txt");

    public override void Serialize<T>(T obj)
    {
        if (obj == null || string.IsNullOrEmpty(FullPath)) return;
        
        var lines = new List<string>
        {
            $"TypeName={obj.GetType().Name}",
            $"Input={obj.Input ?? string.Empty}"
        };
        
        if (obj is Task3 task3)
            lines.Add($"Pattern={task3.Pattern ?? string.Empty}");
        
        File.WriteAllText(FullPath, string.Join(Environment.NewLine, lines));
    }

    public override T Deserialize<T>()
    {
        if (!File.Exists(FullPath)) return default;
            var data = File.ReadAllLines(FullPath)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(['='], 2))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0], p => p[1]);
            
            if (!data.TryGetValue("TypeName", out var typeName)) return default;

            string input = data.GetValueOrDefault("Input", string.Empty);
            string pattern = data.GetValueOrDefault("Pattern", string.Empty);
            
            Lab9.Green.Green? result = typeName switch
            {
                nameof(Task1) => new Task1(input),
                nameof(Task2) => new Task2(input),
                nameof(Task3) => new Task3(input, pattern),
                nameof(Task4) => new Task4(input),
                _ => null
            };
            
            result?.Review();
            return result is T typed ? typed : default;
    }
}