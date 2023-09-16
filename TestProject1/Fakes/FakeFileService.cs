using ConsoleApp1.Services;

namespace TestProject1.Fakes;

public class FakeFileService
: IFileService
{
    private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    
    public string ReadAllText(string path)
    {
        return _dictionary.ContainsKey(path) ? _dictionary[path] : File.ReadAllText(path);
    }

    public void WriteAllText(string path, string content)
    {
        _dictionary[path] = content;
    }
}