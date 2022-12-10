namespace Advent.Of.Code;

using System.Linq;

public static class Aoc10 {

    public static void Main(string[] args) {
        Console.WriteLine(Execute(Parse(Console.In), 1, 1, Seq1(0)).Sum());
    }

    private static Seq<int> Execute(Seq<string> data, int x, int cycle, Seq<int> samples) {

        if (data.IsEmpty)
            return samples;

        (var icycles, var n) = data.Head().Split(" ") switch {
            ["noop"] => (1, 0),
            ["addx", var s] => (2, int.Parse(s)),
            _ =>  throw new Exception("Fail")
        };

        while (icycles > 0) {
            var crt = ((cycle - 1) % 40) + 1;
            if (crt == 1)
                Console.WriteLine();

            if ((cycle + 20) % 40 == 0 && cycle <= 220)
                samples = (x * cycle).Cons(samples);
            cycle++;
            icycles--;

            if (icycles == 0)
                x += n;

            Console.Write((crt >= x - 1 && crt <= x + 1) ? "#": ".");
        }

        return Execute(data.Tail().ToSeq(), x, cycle, samples);
    }

    private static Seq<string> Parse(TextReader data) => data.ReadToEnd().Trim().Split("\n").ToSeq();
}

