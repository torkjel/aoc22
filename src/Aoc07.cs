namespace Advent.Of.Code;

using System.Linq;

public record State(Seq<int> stack, int total);

public static class Aoc07 {

    public static void Main(string[] args) {
        var sizes = ListSizes(Console.In, new Seq<string>());
        Seq(Summarize, SummarizeBonus).Map(f => f(sizes)).ToList().ForEach(Console.WriteLine);
    }

    public static int Summarize(Seq<(Seq<string>, int)> listing) => listing
        .GroupBy(a => a.Item1, b => b.Item2)
        .Map(g => g.Sum())
        .Filter(v => v <= 100000)
        .Sum();

    public static int SummarizeBonus(Seq<(Seq<string>, int)> listing) {
        var sizes = listing
            .GroupBy(a => a.Item1, b => b.Item2)
            .Map(g => g.Sum());
        var root = sizes.Max();
        return sizes
            .Sort<TInt, int>()
            .Find(n => 70000000 - root + n > 30000000)
            .IfNoneUnsafe(() => throw new Exception("bah"));
    }

    public static Seq<(Seq<string>, int)> ListSizes(TextReader data, Seq<string> path) =>
        data.ReadLine() switch {
            null or "" => Seq<(Seq<string>, int)>(),
            var line => line.Split(' ') switch {
                [var size, _] when size.IsNum() => Trickle(path, int.Parse(size)).Concat(ListSizes(data ,path)),
                ["dir", _] => ListSizes(data, path) ,
                ["$", ..var cmd] => cmd switch {
                    ["ls"] => ListSizes(data, path),
                    ["cd", ".."] => ListSizes(data, path.Tail().ToSeq()),
                    ["cd", "/"] => ListSizes(data, Seq<string>()),
                    ["cd", var dir] => ListSizes(data, dir.Cons(path)),
                    _ => throw new Exception("unknown command")
                },
                _ => throw new Exception("unknown output")
            }
        };

    public static bool IsNum(this string str) => str.All(Char.IsDigit);

    public static Seq<(Seq<string>, int)> Trickle(Seq<string> path, int size) => path.Length switch {
        0 => Seq1((Seq<string>(), size)),
        _ => (path, size).Cons(Trickle(path.Tail().ToSeq(), size))
    };
}
