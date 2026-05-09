namespace Lab10;

public abstract class MyFileManager : IFileManager, IFileLifeController
{
    public string Name { get; private set; }
    public string FolderPath { get; private set; }
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public string FullPath
    {
        get
        {
            if (string.IsNullOrEmpty(FileName))
                return "";
            
            if (string.IsNullOrEmpty(FileExtension))
                return Path.Combine(FolderPath, FileName);
            
            return Path.Combine(FolderPath, FileName + (FileExtension.StartsWith(".") ? "" : ".") + FileExtension);
        }
    }

    protected MyFileManager(string name)
    {
        Name = name;
        FolderPath = "";
        FileName = "";
        FileExtension = "";
    }
    protected MyFileManager(string name, string folderPath, string fileName, string fileExtension = "") : this(name)
    {
        FolderPath = folderPath;
        FileName = fileName;
        FileExtension = fileExtension;
    }
    
    // методы IFileManager
    public virtual void SelectFolder(string path) { FolderPath = path; }
    public virtual void ChangeFileName(string fileName) { FileName = fileName; }
    public virtual void ChangeFileFormat(string fileFormat) { FileExtension = fileFormat; }
    
    // методы IFileLifeController
    public virtual void CreateFile()
    {
        if (!Directory.Exists(FolderPath) && !string.IsNullOrEmpty(FolderPath))
            Directory.CreateDirectory(FolderPath);
        if (!string.IsNullOrEmpty(FullPath))
            File.Create(FullPath).Dispose();
    }
    public virtual void DeleteFile()
    {
        if (File.Exists(FullPath))
            File.Delete(FullPath);
    }
    public virtual void EditFile(string content)
    {
        if (File.Exists(FullPath))
        {
            File.WriteAllText(FullPath, content);
        }
    }

    public virtual void ChangeFileExtension(string newExtension)
    {
        if (File.Exists(FullPath))
        {
            string oldPath = FullPath;
            FileExtension = newExtension;
            if (oldPath != FullPath) File.Move(oldPath, FullPath);
        }
        else
            FileExtension = newExtension;
    }
}