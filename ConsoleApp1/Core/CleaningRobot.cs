using ConsoleApp1.Core;

namespace ConsoleApp1;

public class CleaningRobot
{
    private const int TurnBatteryConsumption = 1;
    private const int AdvanceBatteryConsumption = 2;
    private const int BackBatteryConsumption = 3;
    private const int CleanBatteryConsumption = 5;
    
    public int Battery { get; set; }
    public Location Location { get; set; }
    public DirectionEnum Direction { get; set; }

    private readonly Func<Location, bool> _isPositionAvailableAndTrackPosition;
    private readonly Action<Location> _cleaningPosition;
    
    public CleaningRobot(int x, int y, int battery, DirectionEnum directionEnum, 
        Func<Location, bool> isPositionAvailableAndTrackPosition, Action<Location> cleaningPosition)
    {
        _isPositionAvailableAndTrackPosition = isPositionAvailableAndTrackPosition;
        _cleaningPosition = cleaningPosition;
        
        Location = new Location(x, y);
        Battery = battery;
        Direction = directionEnum;

        if (!_isPositionAvailableAndTrackPosition(Location))
        {
            throw new ArgumentOutOfRangeException("Not accessible.");
        }
    }

    public ActionResultEnum TurnLeft()
    {
        if(Battery < TurnBatteryConsumption)
            return ActionResultEnum.OutOfBattery;
            
        Battery -= TurnBatteryConsumption;
        var d = (Direction - 1);
        Direction = d < 0 ? DirectionEnum.W : d;
        return ActionResultEnum.Ok;
    }
    
    public ActionResultEnum TurnRight()
    {
        if(Battery < TurnBatteryConsumption)
            return ActionResultEnum.OutOfBattery;
        
        Battery -= TurnBatteryConsumption;
        var d = ((int)Direction + 1) % 4;
        Direction = (DirectionEnum)d;
        return ActionResultEnum.Ok;
    }

    public ActionResultEnum Advance()
    {
        if(Battery < AdvanceBatteryConsumption)
            return ActionResultEnum.OutOfBattery;
        
        Battery -= AdvanceBatteryConsumption;
        var loc = Location;
        switch (Direction)
        {
            case DirectionEnum.E:
                loc.X++;
                break;
            
            case DirectionEnum.W:
                loc.X--;
                break;
            
            case DirectionEnum.N:
                loc.Y--;
                break;
            
            case DirectionEnum.S:
                loc.Y++;
                break;
        }

        if (!_isPositionAvailableAndTrackPosition(loc)) 
            return ActionResultEnum.NotAccessible;
        
        Location = loc;
        return ActionResultEnum.Ok;

    }
    
    public ActionResultEnum Back()
    {
        if (Battery < BackBatteryConsumption)
            return ActionResultEnum.OutOfBattery;
        
        Battery -= BackBatteryConsumption;
        var loc = Location;
        switch (Direction)
        {
            case DirectionEnum.E:
                loc.X--;
                break;
            
            case DirectionEnum.W:
                loc.X++;
                break;
            
            case DirectionEnum.N:
                loc.Y++;
                break;
            
            case DirectionEnum.S:
                loc.Y--;
                break;
        }
        
        if (_isPositionAvailableAndTrackPosition(loc))
        {
            Location = loc;
            return ActionResultEnum.Ok;
        }

        return ActionResultEnum.NotAccessible;
    }

    public ActionResultEnum Clean()
    {
        if (Battery < CleanBatteryConsumption)
            return ActionResultEnum.OutOfBattery;
        
        Battery -= CleanBatteryConsumption;
        _cleaningPosition(Location);
        return ActionResultEnum.Ok;
    }
}