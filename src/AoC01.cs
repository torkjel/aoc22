namespace Advent.Of.Code;

public static class Aoc01 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Seq(Solve, SolveBonus).Map(f => f(data)).ToList().ForEach(Console.WriteLine);
    }

    public static int Solve(Seq<Seq<int>> data) =>
        data.Map(elf => elf.Sum()).Max();

    public static int SolveBonus(Seq<Seq<int>> data) =>
        data.Map(elf => elf.Sum()).Sort<OrdInt, int>().Rev().Take(3).Sum();

    public static Seq<Seq<int>> Parse(TextReader stdin) => stdin.ReadLine() switch {
        null     => Seq<Seq<int>>(),
        ""       => Seq<int>().Cons(Parse(stdin)),
        var line => Parse(stdin).ToArray() switch {
            [var h, ..var t] => int.Parse(line).Cons(h).Cons(t),
            var e            => Seq1(int.Parse(line)).Cons(e)
        }
    };
}
