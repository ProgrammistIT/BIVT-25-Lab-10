namespace Lab10.Green;

public abstract class GreenFileManager : MyFileManager, ISerializer
{
    public GreenFileManager(string name) : base(name)
    { }

    public GreenFileManager(string name, string folderPath, string fileName, string fileExtension = "") : base(name,
        folderPath, fileName, fileExtension)
    { }

    public override void EditFile(string content)
    {
        if (string.IsNullOrEmpty(FullPath)) return;
        base.EditFile(content);
    }

    public override void ChangeFileExtension(string newExtension)
    {
        if (string.IsNullOrEmpty(FullPath)) return;
        base.ChangeFileExtension(newExtension);
    }
    public abstract T Deserialize<T>() where T : Lab9.Green.Green;
    public abstract void Serialize<T>(T obj) where T : Lab9.Green.Green;
}