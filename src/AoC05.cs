namespace Advent.Of.Code;

using System.Linq;

public static class Aoc05 {

    public static void Main(string[] args) {
        var input = Console.In;
        var stacks = ParseStacks(input);
        var _ = input.ReadLine();
        var moves = ParseMoves(input);

        Console.WriteLine(Solve(stacks, moves));
    }

    public static char[] Solve(Seq<Seq<char>> stacks, Seq<(int n, int src, int dst)> moves) =>
        Moves(stacks, moves).Map(s => s.Head()).ToArray();

    public static Seq<Seq<char>> Move(Seq<Seq<char>> stacks, (int, int src, int dst) move) {
        var stacksArr = stacks.ToArray();
        var srcStack = stacksArr[move.src];
        stacksArr[move.src] = srcStack.Tail().ToSeq();
        stacksArr[move.dst] = srcStack.Head().Cons(stacksArr[move.dst]);
        return stacksArr.ToSeq();
    }

    public static Seq<Seq<char>> Moves(
        Seq<Seq<char>> stacks,
        Seq<(int n, int src, int dst)> moves
    ) => moves.ToArray() switch {
        [(0, _, _), ..var t] => Moves(stacks, t.ToSeq()),
        [var move, ..var t]  => Moves(Move(stacks, move), (move.n - 1, move.src, move.dst).Cons(t)),
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
