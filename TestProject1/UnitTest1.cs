using System.Text.Json;
using ConsoleApp1;
using ConsoleApp1.Core;
using TestProject1.Fakes;

namespace TestProject1;

public class UnitTest1
{
    private ReadOnlySpan<char> GetProjectFolder()
    {
        var bin = System.Reflection.Assembly.GetExecutingAssembly().Location.AsSpan();
        return bin[..(bin.IndexOf("TestProject1") + "TestProject1".Length)];
    }

    private XUnitLogger<MyQApp> GetLogger => new XUnitLogger<MyQApp>(new XUnitLoggerWrapper());
    
    private string GetInput(string name)
    {
        return Path.Combine(GetProjectFolder().ToString(), "TestInput", name);
    }
    
    private string GetOutput(string name)
    {
        return Path.Combine(GetProjectFolder().ToString(), "TestOutput", name);
    }

    private void RunTestInternal(string input, string output)
    {
        var result = "res.json";
        var service = new FakeFileService();
        var myQ = new MyQApp(GetLogger, service);
        myQ.Run(new[] { GetInput(input), result });
        
        var expectedRes = File.ReadAllText(GetOutput(output));
        
        var obj = JsonSerializer.Deserialize<OutputData>(expectedRes, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var serObj = JsonSerializer.Serialize(obj, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var generatedRes = service.ReadAllText(result);
        Assert.Equal(generatedRes, serObj);
    }
    
    [Fact]
    public void ArgumentsFail()
    {
        var myQ = new MyQApp(GetLogger, new FakeFileService());
        Assert.Throws<ArgumentOutOfRangeException>(() =>myQ.Run(new [] {"aaaa"}));
    }
    
    [Fact]
    public void RealTest1()
    {
        RunTestInternal("test1.json", "test1_result.json");
    }
    
    [Fact]
    public void RealTest2()
    {
        RunTestInternal("test2.json", "test2_result.json");
    }
}