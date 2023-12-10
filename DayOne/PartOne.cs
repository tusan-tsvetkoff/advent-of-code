using System.Text.RegularExpressions;

namespace PartOne;

public class DayOne(string[] lines)
{
    public int Run()
    {
        int result = 0;
        foreach (var line in lines)
        {
            var ordered = GetStringDigits(line)
                ?.Concat(GetRealDigits(line))
                .OrderBy(v => v.Index)
                .ToArray();

            var first = ordered?.FirstOrDefault();
            var last = ordered?.LastOrDefault();
            result += ((first?.IntValue ?? 0) * 10) + (last?.IntValue ?? 0);
        }
        return result;
    }

    private static string MapCharDigit(char c)
    {
        return c switch
        {
            '1' => "one",
            '2' => "two",
            '3' => "three",
            '4' => "four",
            '5' => "five",
            '6' => "six",
            '7' => "seven",
            '8' => "eight",
            '9' => "nine",
            _ => ""
        };
    }

    private static Substring[]? GetRealDigits(string line)
    {
        var result = new List<Substring>();
        var offset = 0;
        foreach (var c in line.Where(char.IsNumber))
        {
            var index = Array.IndexOf([.. line], c, offset);
            result.Add(new Substring(MapCharDigit(c), index));
            offset = index + 1;
        }
        return [.. result];
    }

    private static Substring[]? GetStringDigits(string line)
    {
        var result = new List<Substring>();
        string[] substrings =
        [
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine"
        ];

        var foundSubstrings = substrings
            .SelectMany(
                sub =>
                    Regex
                        .Matches(line, sub, RegexOptions.IgnoreCase)
                        .Cast<Match>()
                        .Select(match => new Substring(match.Value, match.Index))
            )
            .OrderBy(result => result.Index)
            .ToArray();
        return foundSubstrings;
    }
}

public record Substring(string Value, int Index)
{
    public int IntValue =>
        Value.ToLowerInvariant() switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => 0
        };
}

