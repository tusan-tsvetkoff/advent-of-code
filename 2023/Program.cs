using PartOne;

var input = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
var result = new DayOne(input).Run();

Console.WriteLine(result);

