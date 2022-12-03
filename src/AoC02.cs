namespace Advent.Of.Code;

using System.Linq;

public static class Aoc02 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Seq(Solve, SolveBonus).Map(f => f(data)).ToList().ForEach(Console.WriteLine);
    }

    public static int Solve(this Seq<(int, int)> data) =>
        data.Map(Score).Sum();

    public static int Score((int, int) val) => val switch {
        (var x, var y) when x == y => 3,
        (0, 1) or (1, 2) or (2, 0) => 6,
        _ => 0
    } + val.Item2 + 1;

    public static int SolveBonus(this Seq<(int, int)> data) =>
        data.Map(ScoreBonus).Sum();

    public static int ScoreBonus((int, int) val) => val switch {
        (var a, 0) => (a + 2) % 3,
        (var a, 1) => a + 3,
        (var a, _) => ((a + 1) % 3) + 6
    } + 1;

    public static Seq<(int, int)> Parse(TextReader stdin) => stdin.ReadLine() switch {
        null => Seq<(int, int)>(),
        var l => l.ToArray() switch {
            [var a, _, var x] => (a - 'A', x - 'X').Cons(Parse(stdin)),
            _ => throw new Exception("Parse error")
        }
    };
}
