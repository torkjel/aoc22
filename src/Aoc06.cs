namespace Advent.Of.Code;

using System.Linq;
using System.Collections.Generic;

public static class Aoc06 {

    public static void Main(string[] args) {
        var data = Console.In.ReadLine().ToArray();
        Console.WriteLine(Solve(data, 4, 4));
        Console.WriteLine(Solve(data, 14, 14));
    }

    public static int Solve(IEnumerable<char> data, int pos, int windowLength) {
        var window = data.Take(windowLength);
        if (window.Length() < windowLength)
            throw new Exception("EoS");
        else if (window.Distinct().Length() == windowLength)
            return pos;
        else
            return Solve(data.Tail(), pos + 1, windowLength);
    }
}
