using System.Text.Json;
using ConsoleApp1.Services;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1;

public class MyQApp
{
    private readonly ILogger<MyQApp> _logger;
    private readonly IFileService _fileService;

    public MyQApp(ILogger<MyQApp> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    public void Run(string[] args)
    {
        if (args?.Length != 2)
            throw new ArgumentOutOfRangeException($"Missing proper parameret set: <input.json> <output.json>");

        var inputFilePath = args[0];
        var outputFilePath = args[1];
        
        var inputData = _fileService.ReadAllText(inputFilePath);
        var input = JsonSerializer.Deserialize<InputData>(inputData, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var crc = new CleanRobotController(input, _logger);
        var res =  crc.Run();
        
        var serializedRes = JsonSerializer.Serialize(res, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        
        _fileService.WriteAllText(outputFilePath, serializedRes);   
    }
}