namespace Advent.Of.Code;

using System.Linq;

public static class Aoc04 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Seq(Solve, SolveBonus).Map(f => f(data)).ToList().ForEach(Console.WriteLine);
    }

    public static int Solve(Seq<((int, int), (int, int))> data) => data.Map(SolveLine).Sum();

    public static int SolveLine(((int, int), (int, int)) line) => line switch {
        ((var a, var b), (var c, var d)) when a >= c && b <= d => 1,
        ((var a, var b), (var c, var d)) when a <= c && b >= d => 1,
        _ => 0
    };

    public static int SolveBonus(Seq<((int, int), (int, int))> data) => data.Map(SolveBonusLine).Sum();

    public static int SolveBonusLine(((int, int), (int, int)) line) => line switch {
        ((var a, var b), (var c, var d)) when b < c || a > d=> 0,
        _ => 1
    };

    public static Seq<((int, int), (int, int))> Parse(TextReader stdin) => stdin.ReadLine() switch {
        null or "" => Seq<((int, int), (int, int))>(),
        var line => ParseLine(line).Cons(Parse(stdin))
    };

    public static ((int, int), (int, int)) ParseLine(string line) => line
        .Split(',')
        .Bind(s => s.Split('-'))
        .Map(int.Parse)
        .ToArray() switch {
            [var a, var b, var c, var d] => ((a, b), (c, d)),
            _ => throw new Exception("Parse error")
    };
}
