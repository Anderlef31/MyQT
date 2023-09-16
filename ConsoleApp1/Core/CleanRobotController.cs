using ConsoleApp1.Core;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1;

public class CleanRobotController
{
    private readonly ILogger _logger;
    private readonly Map _map;
    private readonly CleaningRobot _robot;
    private readonly List<string> _commands;
    private readonly Dictionary<int, List<string>> _fallbackCommands = new Dictionary<int, List<string>>(); 
    
    private readonly List<Location> _visitedLocations  = new List<Location>();
    private readonly List<Location> _cleanedLocations = new List<Location>();
    
    public CleanRobotController(InputData inputData, ILogger logger)
    {
        _logger = logger;
        _map = new Map(inputData.Map);
        _robot = new CleaningRobot(
            inputData.Start.X,
            inputData.Start.Y,
            inputData.Battery,
            Enum.Parse<DirectionEnum>(inputData.Start.Facing),
            IsPositionAvailableWithTracking, AddCleanedLocation);

        _commands = inputData.Commands;
        
        _fallbackCommands.Add(0, new List<string>(new []{ "TR", "A", "TL" }));
        _fallbackCommands.Add(1, new List<string>(new []{ "TR", "A", "TR" }));
        _fallbackCommands.Add(2, new List<string>(new []{ "TR", "A", "TR" }));
        _fallbackCommands.Add(3, new List<string>(new []{ "TR", "B", "TR", "A" }));
        _fallbackCommands.Add(4, new List<string>(new []{ "TL", "TL", "A" }));
    }

    private bool IsPositionAvailableWithTracking(Location loc)
    {
        if (!_map.IsValidLocation(loc))
        {
            return false;
        }

        if (!_visitedLocations.Contains(loc))
        {
            _visitedLocations.Insert(0, loc);
        }
        return true;
    }
    
    private void AddCleanedLocation(Location loc)
    {
        if (!_cleanedLocations.Contains(loc))
        {
            _cleanedLocations.Insert(0, loc);
        }
    }

    public OutputData Run()
    {
        var res = CommandsExecutionInternal(_commands, 0);
        return new OutputData
        {
            Battery = _robot.Battery,
            Cleaned = _cleanedLocations,
            Visited = _visitedLocations,
            Final = new LocationData
            {
                Facing = _robot.Direction.ToString(),
                X = _robot.Location.X,
                Y = _robot.Location.Y
            }
        };
    }

    private ActionResultEnum CommandsExecutionInternal(List<string> commands, int fallback)
    {
        foreach (var command in commands)
        {
            var res = ExecuteCommand(command);
            if (res == ActionResultEnum.OutOfBattery)
            {
                return ActionResultEnum.OutOfBattery;
            }
            
            if (res == ActionResultEnum.NotAccessible)
            {
                if (!_fallbackCommands.ContainsKey(fallback))
                {
                    return ActionResultEnum.End;
                }
             
                _logger.LogInformation($"Back off strategy {fallback} will be executed.");
                
                var fallbackCommands = _fallbackCommands[fallback];
                var fallbackRes = CommandsExecutionInternal(fallbackCommands, fallback + 1);
                if (fallbackRes != ActionResultEnum.Ok)
                    return fallbackRes;
            }
        }

        return ActionResultEnum.Ok;
    }
    
    private ActionResultEnum ExecuteCommand(string command)
    {
        _logger.LogInformation($"Command {command} for execution.");
        switch (command)
        {
            case "TL":
                return _robot.TurnLeft();
            case "TR":
                return _robot.TurnRight();
            case "A":
                return _robot.Advance();
            case "B":
                return _robot.Back();
            case "C":
                return _robot.Clean();
            default:
                throw new ArgumentOutOfRangeException($"Command {command} not supported");
        }
    }
}