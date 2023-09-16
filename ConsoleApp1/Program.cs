using ConsoleApp1;
using ConsoleApp1.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Start {DateTime.Now}");
        
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IFileService, FileService>()
            .AddScoped<MyQApp>()
            .BuildServiceProvider();
        
        var myQApp = serviceProvider.GetService<MyQApp>();
        myQApp.Run(args);
        
        Console.WriteLine($"End {DateTime.Now}");
    }
}