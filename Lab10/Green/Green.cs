namespace Lab10.Green;

public class Green
{
    public GreenFileManager Manager { get; private set; }
    public Lab9.Green.Green[] Tasks { get; private set; }
    
    public Green()
    {
        Tasks = [];
    }

    public Green(Lab9.Green.Green[] tasks) : this(tasks, null) { }
    
    public Green(GreenFileManager manager, Lab9.Green.Green[] tasks = null) : this(tasks, manager) { }

    public Green(Lab9.Green.Green[] tasks, GreenFileManager manager)
    {
        Manager = manager;
        Tasks = tasks?.Where(t => t != null).ToArray() ?? [];
    }

    public void Add(Lab9.Green.Green task)
    {
        if (task == null) return;
        
        var arr = Tasks;
        Array.Resize(ref arr, arr.Length + 1);
        arr[^1] = task;
        
        Tasks = arr;
    }

    public void Add(Lab9.Green.Green[] tasks)
    {
        var valid = tasks?.Where(t => t != null).ToArray();
        
        if (valid == null || valid.Length == 0) return;
        
        var arr = Tasks;
        int old = arr.Length;
        
        Array.Resize(ref arr, old + valid.Length);
        Array.Copy(valid, 0, arr, old, valid.Length);

        Tasks = arr;
    }

    public void Remove(Lab9.Green.Green task)
    {
        if (Tasks == null || Tasks.Length == 0) return;
        
        int idx = Array.IndexOf(Tasks, task);
        
        if (idx == -1) return;
        var arr = new Lab9.Green.Green[Tasks.Length - 1];
        
        Array.Copy(Tasks, arr, idx);
        Array.Copy(Tasks, idx + 1, arr, idx, Tasks.Length - 1 - idx);
        
        Tasks = arr;
    }

    public void Clear()
    {
        Tasks = [];
        
        if (Manager != null && !string.IsNullOrEmpty(Manager.FolderPath) && Directory.Exists(Manager.FolderPath))
            Directory.Delete(Manager.FolderPath, true);
    }

    public void SaveTasks()
    {
        if (Manager == null || Tasks == null) return;
        
        for (int i = 0; i < Tasks.Length; i++)
        {
            Manager.ChangeFileName($"task{i}");
            
            Manager.Serialize(Tasks[i]);
        }
    }

    public void LoadTasks()
    {
        if (Manager == null || string.IsNullOrEmpty(Manager.FolderPath)) return;
        
        if (!Directory.Exists(Manager.FolderPath)) return;
        
        var files = Directory.GetFiles(Manager.FolderPath);

        var loaded = new List<Lab9.Green.Green>();

        foreach (var file in files)
        {
            Manager.ChangeFileName(Path.GetFileNameWithoutExtension(file));
            var task = Manager.Deserialize<Lab9.Green.Green>();
            if (task != null) loaded.Add(task);
        }

        Tasks = loaded.ToArray();
    }

    public void ChangeManager(GreenFileManager newManager)
    {
        if (newManager == null) return;
        
        Manager = newManager;
        
        string folder = Path.Combine(Directory.GetCurrentDirectory(), newManager.Name);
        
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        
        Manager.SelectFolder(folder);
    }
}