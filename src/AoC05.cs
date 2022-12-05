namespace Advent.Of.Code;

using System.Linq;

public static class Aoc05 {

    private static readonly bool CM9000 = false;
    private static readonly bool CM9001 = true;

    public static void Main(string[] args) {
        var input = Console.In;
        var stacks = ParseStacks(input);
        var _ = input.ReadLine();
        var moves = ParseMoves(input);

        var solver = curry<Seq<Seq<char>>, Seq<(int, int, int)>, bool, char[]>(Solve)(stacks)(moves);
        Seq(CM9000, CM9001).Map(solver).ToList().ForEach(Console.WriteLine);
    }

    public static char[] Solve(Seq<Seq<char>> stacks, Seq<(int n, int src, int dst)> moves, bool keepOrder) =>
        Moves(stacks, moves, keepOrder).Map(s => s.Head()).ToArray();

    public static Seq<Seq<char>> MoveN(Seq<Seq<char>> stacks, (int n, int src, int dst) move, bool keepOrder) {
        var stacksArr = stacks.ToArray();
        var srcStack = stacksArr[move.src];
        var moved = srcStack.Take(move.n);
        stacksArr[move.src] = srcStack.Skip(move.n).ToSeq();
        stacksArr[move.dst] = (keepOrder ? moved : moved.Rev()).Concat(stacksArr[move.dst]);
        return stacksArr.ToSeq();
    }

    public static Seq<Seq<char>> Moves(
        Seq<Seq<char>> stacks,
        Seq<(int n, int src, int dst)> moves,
        bool keepOrder
    ) => moves.ToArray() switch {
        [var move, ..var t]  => Moves(MoveN(stacks, move, keepOrder), t.ToSeq(), keepOrder),
        [] => stacks
    };

    public static Seq<(int n, int src, int dst)> ParseMoves(TextReader stdin) => stdin.ReadLine() switch {
        null or "" => Seq<(int n, int src, int dst)>(),
        var line   => line.Split(' ') switch {
            [_, var n, _, var src, _, var dst] => (int.Parse(n), int.Parse(src) - 1, int.Parse(dst) - 1).Cons(ParseMoves(stdin)),
            var l                              => throw new Exception("Parse error: " + l)
        }
    };

    public static Seq<Seq<char>> ParseStacks(TextReader stdin) => stdin.ReadLine() switch {
        var line when line.Contains('[') => AppendLine(ParseStackLine(line), ParseStacks(stdin)),
        _ => Seq<Seq<char>>()
    };

    public static Seq<Seq<char>> AppendLine(Seq<Option<char>> line, Seq<Seq<char>> stacks) =>
        (line.ToArray(), stacks.ToArray()) switch {
            ([var oh, ..var t], [var sh, ..var st]) => oh.Match(
                Some: h  => h.Cons(sh).Cons(AppendLine(t.ToSeq(), st.ToSeq())),
                None: () => sh.Cons(AppendLine(t.ToSeq(), st.ToSeq()))
            ),
            ([], var s)             => s.ToSeq(),
            ([var oh, ..var t], []) => oh.Match(
                Some: h  => Seq1(h).Cons(AppendLine(t.ToSeq(), Seq<Seq<char>>())),
                None: () => Seq<char>().Cons(AppendLine(t.ToSeq(), Seq<Seq<char>>()))
            )
        };

    public static Seq<Option<char>> ParseStackLine(string line) => line
        .Chunk(4)
        .Map(chunk => chunk[1])
        .Map(a => a switch {
            ' '   => Option<char>.None,
            var c => Option<char>.Some(c)
        })
        .ToSeq();
}
