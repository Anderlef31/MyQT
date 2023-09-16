using ConsoleApp1;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TestProject1.Fakes;

public class XUnitLoggerWrapper
{
    public XUnitLoggerWrapper()
    {
        var ee = 12;
    }
    public ITestOutputHelper OutputLogger;
}
    
public class XUnitLogger<T> : ILogger<T>, IDisposable
{
    private XUnitLoggerWrapper _wrapper;
    
    public XUnitLogger(XUnitLoggerWrapper wrapper)
    {
        _wrapper = wrapper;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (_wrapper.OutputLogger == null)
        {
            //logging before test run, initialization
            Console.WriteLine(state.ToString());
        }
        else
        {
            _wrapper.OutputLogger.WriteLine(state.ToString());
        }
    }
    
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }
    
    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }
    
    public void Dispose()
    {
    }
}