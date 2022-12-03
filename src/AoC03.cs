namespace Advent.Of.Code;

using System.Linq;

public static class Aoc03 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Seq(Solve, SolveBonus).Map(f => f(data)).ToList().ForEach(Console.WriteLine);
    }

    public static int Solve(this Seq<Seq<int>> data) => data.Map(SolveLine).Sum();

    public static int SolveLine(this Seq<int> line) => line
        .Zip(Range(0, 10000))
        .GroupBy(e => e.First, e => e.Second)
        .Map(g => g.Sort<TInt, int>().ToArray() switch {
            [var h, .., var t] when h < line.Length / 2 && t >= line.Length / 2 => g.Key,
            _ => 0
        }).Sum();

    public static int SolveBonus(this Seq<Seq<int>> data) => data.Length switch {
        > 0 => SolveBonusGroup(data.Take(3)) + SolveBonus(data.Skip(3)),
        _    => 0
    };

    public static int SolveBonusGroup(this Seq<Seq<int>> data) => data
        .Bind(elf => elf.Distinct())
        .GroupBy(i => i)
        .Find(g => g.Length() == 3)
        .Match(
            Some: g  => g.Key,
            None: () => throw new Exception("No badge")
        );

    public static Seq<Seq<int>> Parse(TextReader stdin) => stdin.ReadLine() switch {
        null     => Seq<Seq<int>>(),
        var line => ParseLine(line.ToArray()).Cons(Parse(stdin))
    };

    public static Seq<int> ParseLine(char[] line) => line switch {
        []                             => Seq<int>(),
        [var h, ..var t] when h >= 'a' => (h - 'a' + 1).Cons(ParseLine(t)),
        [var h, ..var t] when h >= 'A' => (h - 'A' + 27).Cons(ParseLine(t)),
        _                              => throw new Exception("Parse error"),
    };
}
