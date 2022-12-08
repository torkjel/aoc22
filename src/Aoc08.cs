namespace Advent.Of.Code;

using System.Linq;

public static class Aoc08 {

    public static void Main(string[] args) {
        var data = Parse(Console.In);
        Seq(FindVisible, FindBestView).Map(f => f(data)).ToList().ForEach(Console.WriteLine);
    }

    public static int FindVisible(char[][] data) {
        var coords = Range(0, data.Length).Bind(y => Range(0, data[0].Length).Map(x => (x, y)));
        return coords.Fold(Set<(int, int)>(), (v, c) => IsVisible(data, c.Item1, c.Item2) ? v.AddOrUpdate(c) : v).Length;
    }

    public static bool IsVisible(char[][] map, int x, int y) =>
        (x, y) switch {
            (0, _) or (_, 0) => true,
            _ when x >= map[y].Length - 1 || y >= map.Length - 1 => true,
            _ => FreeSigth(map, map[y][x], x - 1, y, -1,  0) ||
                 FreeSigth(map, map[y][x], x, y - 1,  0, -1) ||
                 FreeSigth(map, map[y][x], x + 1, y,  1,  0) ||
                 FreeSigth(map, map[y][x], x, y + 1,  0,  1)
        };

    public static bool FreeSigth(char[][] map, int height, int x, int y, int dx, int dy) =>
        (x, y) switch {
            (-1, _) or (_, -1) => true,
            _ when y >= map.Length || x >= map[0].Length => true,
            _ => map[y][x] < height && FreeSigth(map, height, x + dx, y + dy, dx, dy)
        };

    public static int FindBestView(char[][] data) => Range(0, data.Length)
        .Bind(y => Range(0, data[0].Length).Map(x => (x, y)))
        .Map(c => ViewScore(data, c.Item1, c.Item2))
        .Max();

    public static int ViewScore(char[][] data, int x, int y) =>
        FreeView(data, data[y][x], x - 1, y, -1,  0) *
        FreeView(data, data[y][x], x, y - 1,  0, -1) *
        FreeView(data, data[y][x], x + 1, y,  1,  0) *
        FreeView(data, data[y][x], x, y + 1,  0,  1);

    public static int FreeView(char[][] map, int height, int x, int y, int dx, int dy) =>
        (x, y) switch {
            (-1, _) or (_, -1) => 0,
            _ when y >= map.Length || x >= map[0].Length => 0,
            _ when map[y][x] >= height => 1,
            _ => 1 + FreeView(map, height, x + dx, y + dy, dx, dy)
        };

    public static char[][] Parse(TextReader data) => data.ReadLine() switch {
        null or "" => new char[][] {},
        var line => line.ToArray().Cons(Parse(data)).ToArray()
    };
}

