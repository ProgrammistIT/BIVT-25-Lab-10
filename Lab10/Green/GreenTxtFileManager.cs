using System.Reflection;

namespace Lab10.Green
{
    public class GreenTxtFileManager : GreenFileManager
    {
        public GreenTxtFileManager(string name) : base(name) { }

        public GreenTxtFileManager(string name, string folder, string fileName, string extension = "txt")
            : base(name, folder, fileName, extension) { }

        public override void EditFile(string input)
        {
            if (File.Exists(FullPath))
            {
                var obj = Deserialize<Lab9.Green.Green>();
                if (obj != null)
                {
                    obj.ChangeText(input);
                    Serialize(obj);
                }
            }
        }

        public override void ChangeFileExtension(string input)
        {
            ChangeFileFormat("txt");
        }

        public override void Serialize<T>(T obj)
{
    if (obj == null || string.IsNullOrEmpty(FullPath)) return;

    var lines = new System.Collections.Generic.List<string>();
    lines.Add($"TypeName={obj.GetType().FullName}");

    Type type = obj.GetType();
    while (type != null && type != typeof(object))
    {
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            lines.Add($"{field.Name}={field.GetValue(obj)}");
        type = type.BaseType;
    }

    File.WriteAllText(FullPath, string.Join(Environment.NewLine, lines));
}

        public override T Deserialize<T>()
        {
            if (!File.Exists(FullPath)) return null;
            try
            {
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string line in File.ReadAllLines(FullPath))
                {
                    int sep = line.IndexOf('=');
                    if (sep < 0) continue;
                    dict[line.Substring(0, sep)] = line.Substring(sep + 1);
                }

                Type actualType = typeof(T);
                if (dict.TryGetValue("TypeName", out string typeName))
                {
                    var found = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(a => a.GetType(typeName)).FirstOrDefault(t => t != null);
                    if (found != null) actualType = found;
                }

                // Пробуем все конструкторы, включая непубличные
                object obj = null;
                var ctors = actualType
                    .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .OrderByDescending(c => c.GetParameters().Length);
                foreach (var ctor in ctors)
                {
                    var pInfos = ctor.GetParameters();
                    var args = new object[pInfos.Length];
                    for (int i = 0; i < pInfos.Length; i++)
                    {
                        // Ищем по имени параметра среди полей
                        string match = dict.Keys.FirstOrDefault(k =>
                            k.ToLower() == pInfos[i].Name.ToLower() ||
                            k.ToLower() == "_" + pInfos[i].Name.ToLower() ||
                            k.ToLower().Contains(pInfos[i].Name.ToLower()));
                        args[i] = match != null ? dict[match] : string.Empty;
                    }

                    try
                    {
                        obj = ctor.Invoke(args);
                        break;
                    }
                    catch
                    {
                    }
                }

                if (obj == null) return null;
                
                Type t = actualType;
                while (t != null && t != typeof(object))
                {
                    foreach (var field in t.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                                      BindingFlags.NonPublic))
                    {
                        if (dict.TryGetValue(field.Name, out string val))
                        {
                            try
                            {
                                if (field.FieldType == typeof(string)) field.SetValue(obj, val);
                                else if (field.FieldType == typeof(int)) field.SetValue(obj, int.Parse(val));
                                else if (field.FieldType == typeof(double)) field.SetValue(obj, double.Parse(val));
                            }
                            catch
                            {
                            }
                        }
                    }

                    t = t.BaseType;
                }

                var result = (T)obj;
                result?.Review();
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}