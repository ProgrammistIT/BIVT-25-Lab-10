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
        var greens = Tasks;
        Array.Resize(ref greens, greens.Length + 1);
        greens[^1] = task;
        Tasks = greens;
    }

    public void Add(Lab9.Green.Green[] tasks)
    {
        var valid = tasks?.Where(t => t != null).ToArray();
        if (valid == null || valid.Length == 0) return;

        var newTasks = Tasks;
        int oldSize = newTasks.Length;
        Array.Resize(ref newTasks, oldSize + valid.Length);
        Array.Copy(valid, 0, newTasks, oldSize, valid.Length);
        Tasks = newTasks;
    }

    public void Remove(Lab9.Green.Green task)
    {
        if (Tasks == null || Tasks.Length == 0) return;

        int idx = Array.IndexOf(Tasks, task);
        if (idx == -1) return;

        var newTasks = new Lab9.Green.Green[Tasks.Length - 1];
        Array.Copy(Tasks, newTasks, idx);
        Array.Copy(Tasks, idx + 1, newTasks, idx, Tasks.Length - idx - 1);
        Tasks = newTasks;
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
            Manager.ChangeFileName($"task_{i}");
            Manager.Serialize(Tasks[i]);
        }
    }

    public void LoadTasks()
    {
        if (Manager == null || string.IsNullOrEmpty(Manager.FolderPath)) return;
        if (!Directory.Exists(Manager.FolderPath)) return;

        // Нормализуем расширение для поиска (без точки)
        string ext = Manager.FileExtension?.TrimStart('.') ?? "";
        string pattern = string.IsNullOrEmpty(ext) ? "task_*" : $"task_*.{ext}";

        var files = Directory.GetFiles(Manager.FolderPath, pattern);
        var loadedTasks = new Lab9.Green.Green[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            Manager.ChangeFileName($"task_{i}");
            loadedTasks[i] = Manager.Deserialize<Lab9.Green.Green>();
        }

        Tasks = loadedTasks;
    }
    
    public void ChangeManager(GreenFileManager newManager)
    {
        if (newManager == null) return;

        Manager = newManager;
        
        string folder = newManager.Name;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        Manager.SelectFolder(folder);
    }
}