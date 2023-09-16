using System.Text.Json.Serialization;

namespace ConsoleApp1.Core;

public struct Location
{
    [JsonPropertyName("X")]
    public int X { get; set; }
    [JsonPropertyName("Y")]
    public int Y { get; set; }

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return X + ";" + Y;
    }
}