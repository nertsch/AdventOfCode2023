Part1();
Part2();

static void Part1()
{
    var calibrationValueSum = (
        from line in File.ReadLines("PuzzleInput.txt")
        let firstDigit = line.First(char.IsDigit)
        let lastDigit = line.Last(char.IsDigit)
        select int.Parse(stackalloc[] { firstDigit, lastDigit })
    ).Sum();

    Console.WriteLine(calibrationValueSum);
}

static void Part2()
{
    var replacements = new [] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }
        .Select((n,i) => (OldValue: n, NewValue: (i+1).ToString()[0]))
        .ToArray();

    var calibrationValueSum = (
        from line in File.ReadLines("PuzzleInput.txt")
        let fixedLine = FixLine(line.AsSpan())
        let firstDigit = fixedLine.First(char.IsDigit)
        let lastDigit = fixedLine.Last(char.IsDigit)
        select int.Parse(stackalloc[] { firstDigit, lastDigit })
    ).Sum();

    Console.WriteLine(calibrationValueSum);

    string FixLine(ReadOnlySpan<char> line)
    {
        Span<char> fixedLine = stackalloc char[line.Length];
        var fixedLineLength = 0;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (char.IsDigit(c))
            {
                fixedLine[fixedLineLength++] = c;
            }
            else
            {
                foreach (var replacement in replacements)
                {
                    if (line[i..].StartsWith(replacement.OldValue))
                        fixedLine[fixedLineLength++] = replacement.NewValue;
                }
            }
        }

        return new string(fixedLine[..fixedLineLength]);
    }
}
