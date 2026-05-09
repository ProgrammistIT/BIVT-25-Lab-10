namespace Lab10;

public interface IFileManager
{
    public string FolderPath { get; }
    public string FileName { get; }
    public string FileExtension { get; }
    public string FullPath { get; }

    public void SelectFolder(string path);
    public void ChangeFileName(string fileName);
    public void ChangeFileFormat(string fileFormat);
}