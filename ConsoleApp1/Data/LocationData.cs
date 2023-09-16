using System.Text.Json.Serialization;

namespace ConsoleApp1.Core;

public class LocationData
{
    [JsonPropertyName("X")]
    public int X { get; set; }
    [JsonPropertyName("Y")]
    public int Y { get; set; }
    public string Facing { get; set; }
}