namespace Advent.Of.Code;

using System.Linq;

public static class Aoc09 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Console.WriteLine(Follow(data, (0, 0), Seq1((0, 0))).Distinct().Length());
        Console.WriteLine(Follow(data, (0, 0), Enumerable.Repeat((0, 0), 9).ToSeq()).Distinct().Length());
    }

    private static Seq<(int, int)> Follow(
        Seq<(string, int)> walk,
        (int, int) head,
        Seq<(int, int)> knots
    ) => walk.Length() switch {
        0 => Seq1(knots.Last()),
        _ => walk.Head() switch {
            (_, 0) => Follow(walk.Tail().ToSeq(), head, knots),
            (var dir, var steps) => knots.Last().Cons(
                Follow(
                    (dir, steps - 1).Cons(walk.Tail()),
                    Move(head, dir),
                    MoveKnots(Move(head, dir).Cons(knots))
                )
            )
        }
    };

    private static Seq<(int, int)> MoveKnots(Seq<(int, int)> knots) => knots.ToArray() switch {
        [var head, var next, ..var tail] => MoveAfter(head, next).Cons(MoveKnots(MoveAfter(head, next).Cons(tail))),
        _ => Seq<(int, int)>(),
    };

    private static (int x, int y) Move((int x, int y) pos, string dir) => dir switch {
        "U" => (pos.x, pos.y - 1),
        "D" => (pos.x, pos.y + 1),
        "R" => (pos.x + 1, pos.y),
        "L" => (pos.x - 1, pos.y),
        _ => throw new Exception("bad direction")
    };

    private static (int x, int y) MoveAfter(
        (int x, int y) head,
        (int x, int y) tail
    ) => diff(head, tail) switch {
        (var dx, var dy, <= 1, <= 1) => tail,
        (var dx, var dy, _, _) => (tail.x + Clamp(dx), tail.y + Clamp(dy)),
    };

    private static (int dx, int dy, int ax, int ay) diff((int x, int y) head, (int x, int y) tail) =>
        (head.x - tail.x, head.y - tail.y, Math.Abs(head.x - tail.x), Math.Abs(head.y - tail.y));

    private static int Clamp(int i) => i switch {
        > 0 => 1,
        < 0 => -1,
        _ => 0
    };

    private static Seq<(string, int)> Parse(TextReader data) => data.ReadLine() switch {
        null or "" => Seq<(string, int)>(),
        var line => line.Split(" ") switch {
            [var dir, var steps] => (dir, int.Parse(steps)).Cons(Parse(data)),
            _ => throw new Exception("Parse error")
        }
    };
}

