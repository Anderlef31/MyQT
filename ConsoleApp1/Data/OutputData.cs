namespace ConsoleApp1.Core;


public class OutputData
{
    public List<Location> Visited { get; set; }
    public List<Location> Cleaned { get; set; }
    public LocationData Final { get; set; }
    public int Battery { get; set; }
}