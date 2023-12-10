using DayTwo;

// string startupPath = Path.Combine(Directory.GetCurrentDirectory(), "testInputDayTwo.txt");
var input = File.ReadAllText("daytwoinput.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);

/* string[] input =
[
    "Game 1: 7 blue, 9 red, 1 green; 8 green; 10 green, 5 blue, 3 red; 11 blue, 5 red, 1 green"
]; */
var result = new Solution(input).SolvePartTwo();

Console.WriteLine(result);


