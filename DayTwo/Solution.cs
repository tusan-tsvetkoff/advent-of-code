using System.Text.RegularExpressions;

namespace DayTwo;

public partial class Solution(string[] input)
{
    private const int MaxRed = 12;
    private const int MaxBlue = 14;
    private const int MaxGreen = 13;

    private record Game(List<Set> Sets)
    {
        public int RedColorCount => Sets.Max(set => set.RedColorCount);
        public int BlueColorCount => Sets.Max(set => set.BlueColorCount);
        public int GreenColorCount => Sets.Max(set => set.GreenColorCount);

        private int[] ColorCounts => [RedColorCount, BlueColorCount, GreenColorCount];

        public int CalculateProduct => ColorCounts.Where(x => x != 0).Aggregate((a, b) => a * b);
    }

    private class Set
    {
        public Dictionary<string, int> ColorCountMap { get; set; } = [];

        public int RedColorCount => ColorCountMap.TryGetValue("red", out int value) ? value : 0;
        public int GreenColorCount => ColorCountMap.TryGetValue("green", out int value) ? value : 0;
        public int BlueColorCount => ColorCountMap.TryGetValue("blue", out int value) ? value : 0;

        private Set(Dictionary<string, int> colorCountMap)
        {
            ColorCountMap = colorCountMap;
        }

        public static Set Create(Dictionary<string, int> colorCountMap)
        {
            return new Set(colorCountMap);
        }
    }

    public int SolvePartTwo()
    {
        return input
            .Select(line => ExtractSets(line)!)
            .Select(
                sets =>
                    sets.Select(
                            set =>
                                set.Trim()
                                    .Split(',')
                                    .Select(pair => GetColorAndCount(pair))
                                    .GroupBy(pair => pair.color)
                                    .ToDictionary(
                                        group => group.Key,
                                        group => group.Sum(entry => entry.count)
                                    )
                        )
                        .Select(pairMap => Set.Create(pairMap))
                        .ToList()
            )
            .Select(setCollection => new Game(setCollection).CalculateProduct)
            .Sum();
    }

    private (string color, int count) GetColorAndCount(string pair)
    {
        var color = pair.Trim().Split(' ')[1].Trim();
        var count = int.Parse(pair.Trim().Split(' ')[0].Trim());
        return (color, count);
    }

    private string[]? ExtractSets(string line)
    {
        return line[(line.IndexOf(':') + 1)..].Trim().Split(';');
    }

    private int ExtractGameId(string line)
    {
        var match = GameIdReg().Match(line);
        return match.Success
            ? int.Parse(match.Groups[1].Value)
            : throw new Exception("No match found: ");
    }

    public int Solve()
    {
        List<int> possibleGames = [];
        var setPairsMap = new List<Dictionary<string, int>>();
        foreach (string line in input)
        {
            var sets = ExtractSets(line);

            var setLists = new List<List<string>>();

            var gameId = ExtractGameId(line);

            for (int i = 0; i < sets.Length; i++)
            {
                var pairs = sets[i].Trim().Split(',');
                var pairMap = new Dictionary<string, int>();
                foreach (var pair in pairs)
                {
                    var (color, count) = GetColorAndCount(pair);
                    if (pairMap.ContainsKey(color))
                    {
                        pairMap[color] += count;
                    }
                    else
                    {
                        pairMap[color] = count;
                    }
                }
                setPairsMap.Add(pairMap);
            }
            if (setPairsMap.Any(pm => !CheckSetValidity(pm)))
            {
                setPairsMap.Clear();
                continue;
            }
            else
            {
                possibleGames.Add(gameId);
                setPairsMap.Clear();
            }
        }

        return possibleGames.Sum();
    }

    public static bool CheckSetValidity(Dictionary<string, int> colorCount)
    {
        foreach (var color in colorCount.Keys)
        {
            switch (color)
            {
                case "red" when colorCount[color] > MaxRed:
                    return false;
                case "blue" when colorCount[color] > MaxBlue:
                    return false;
                case "green" when colorCount[color] > MaxGreen:
                    return false;
            }
        }
        return true;
    }

    [GeneratedRegex(@"Game (\d+):")]
    private static partial Regex GameIdReg();
}

