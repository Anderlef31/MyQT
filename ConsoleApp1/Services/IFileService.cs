namespace ConsoleApp1.Services;

public interface IFileService
{
    public string ReadAllText(string path);
    public void WriteAllText(string path, string content);
}