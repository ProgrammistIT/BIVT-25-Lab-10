namespace Lab10;

public interface ISerializer
{
    T Deserialize<T>() where T : Lab9.Green.Green;
    void Serialize<T>(T obj) where T : Lab9.Green.Green;
}