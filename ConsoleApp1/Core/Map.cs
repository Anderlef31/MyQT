using ConsoleApp1.Core;

namespace ConsoleApp1;

public class Map
{
    private readonly HashSet<string> _hashSet = new HashSet<string>();

    public Map(List<List<string>> definitionData)
    {
        for (var y = 0; y < definitionData.Count; y++)
        {
            var yList = definitionData[y];
            for (var x = 0; x < yList.Count; x++)
            {
                var loc = yList[x];
                if (!string.IsNullOrEmpty(loc) && loc.Equals("S", StringComparison.Ordinal))
                {
                    _hashSet.Add((new Location(x, y)).ToString());
                }
            }
        }
    }

    public bool IsValidLocation(Location loc)
    {
        return _hashSet.Contains(loc.ToString());
    }
}