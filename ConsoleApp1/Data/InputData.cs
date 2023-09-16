using System.Text.Json.Serialization;
using ConsoleApp1.Core;

namespace ConsoleApp1;
public class InputData
{
    public List<List<string>> Map { get; set; }
    public LocationData Start { get; set; }
    public List<string> Commands { get; set; }
    public int Battery { get; set; }
}