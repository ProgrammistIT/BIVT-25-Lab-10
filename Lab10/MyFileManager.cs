namespace Lab10
{
    public abstract class MyFileManager : IFileManager, IFileLifeController
    {
        public string Name { get; private set; }
        public string FolderPath { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        public string FullPath => Path.Combine(FolderPath, $"{FileName}.{FileExtension}");

        public MyFileManager(string name)
        {
            Name = name;
            FolderPath = "";
            FileName = "";
            FileExtension = "txt";
        }

        public MyFileManager(string name, string folder, string fileName, string extension = "txt")
        {
            Name = name;
            FolderPath = folder;
            FileName = fileName;
            FileExtension = extension;
        }

        public virtual void ChangeFileExtension(string fileExtension)
        {
            if (File.Exists(FullPath))
            {
                string content = File.ReadAllText(FullPath);
                File.Delete(FullPath);
                FileExtension = fileExtension;
                File.WriteAllText(FullPath, content);
            }
            else
            {
                FileExtension = fileExtension;
            }
        }

        public virtual void CreateFile()
        {
            if (!string.IsNullOrEmpty(FolderPath) && !Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            if (!File.Exists(FullPath))
            {
                File.Create(FullPath).Close();
            }
        }

        public virtual void DeleteFile()
        {
            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
        }

        public virtual void EditFile(string content)
        {
            if (!File.Exists(FullPath)) return;
            File.WriteAllText(FullPath, content);
        }

        public void ChangeFileFormat(string fileFormat)
        {
            FileExtension = fileFormat;
            if (!string.IsNullOrEmpty(FolderPath) && !string.IsNullOrEmpty(FileName))
            {
                if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
                if (!File.Exists(FullPath)) File.Create(FullPath).Close();
            }
        }

        public void ChangeFileName(string fileName)
        {
            FileName = fileName;
        }

        public void SelectFolder(string path)
        {
            FolderPath = path;
        }
    }
}