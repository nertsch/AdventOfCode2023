Part1();
Part2();

static void Part1()
{
    var calibrationValueSum = (
        from line in File.ReadLines("PuzzleInput.txt")
        let firstDigit = line.First(char.IsDigit)
        let lastDigit = line.Last(char.IsDigit)
        select int.Parse([firstDigit, lastDigit])
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
        let firstDigit = GetFirstDigit(line)
        let lastDigit = GetLastDigit(line)
        select int.Parse([firstDigit, lastDigit])
    ).Sum();

    Console.WriteLine(calibrationValueSum);

    char GetFirstDigit(ReadOnlySpan<char> line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            if (GetDigit(line[i..]) is { } digit)
                return digit;
        }

        throw new ArgumentException("Line must contain at least one digit");
    }

    char GetLastDigit(ReadOnlySpan<char> line)
    {
        for (var i = line.Length - 1; i >= 0; i--)
        {
            if (GetDigit(line[i..]) is { } digit)
                return digit;
        }
        
        throw new ArgumentException("Line must contain at least one digit");
    }
   
    char? GetDigit(ReadOnlySpan<char> input)
    {
        if (char.IsDigit(input[0]))
            return input[0];

        foreach (var replacement in replacements)
        {
            if (input.StartsWith(replacement.OldValue))
                return replacement.NewValue;
        }

        return null;
    }
}
