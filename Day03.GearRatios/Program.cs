Part1();
Part2();
Console.ReadKey();

void Part1()
{
    string previousLine = null!;
    string? lineToEvaluate = null;
    string nextLine = null!;

    var partNumberSum = 0;
    char[] currentNumber = null!;
    var currentNumberLength = 0;
    var hasCurrentNumberAdjacentSymbol = false;
    var isNumberReadInProgress = false;

    foreach (var readLine in File.ReadLines("PuzzleInput.txt").Append(null))
    {
        if (lineToEvaluate != null)
        {
            previousLine = lineToEvaluate;
            lineToEvaluate = nextLine;
            nextLine = readLine ?? new string('.', nextLine.Length);
        }
        else
        {
            previousLine = lineToEvaluate = new string('.', readLine!.Length);
            nextLine = readLine;
            currentNumber = new char[readLine.Length];
        }

        for (var i = 0; i < lineToEvaluate.Length; i++)
        {
            if (char.IsDigit(lineToEvaluate[i]))
            {
                isNumberReadInProgress = true;
                currentNumber[currentNumberLength++] = lineToEvaluate[i];
            }
            else
            {
                if (isNumberReadInProgress)
                {
                    if (hasCurrentNumberAdjacentSymbol)
                        partNumberSum += int.Parse(currentNumber.AsSpan()[..currentNumberLength]);
                    currentNumberLength = 0;
                    isNumberReadInProgress = false;
                    hasCurrentNumberAdjacentSymbol = false;
                }
            }

            if (isNumberReadInProgress && !hasCurrentNumberAdjacentSymbol)
            {
                var beforeI = i - 1;
                var afterI = i + 1;
                hasCurrentNumberAdjacentSymbol =
                    (beforeI >= 0 && (IsSymbol(previousLine[beforeI]) || IsSymbol(lineToEvaluate[beforeI]) || IsSymbol(nextLine[beforeI]))) ||
                     IsSymbol(previousLine[i]) || IsSymbol(nextLine[i]) ||
                    (afterI < lineToEvaluate.Length && (IsSymbol(previousLine[afterI]) || IsSymbol(lineToEvaluate[afterI]) || IsSymbol(nextLine[afterI])));
            }
        }

    }

    bool IsSymbol(char c) => c != '.' && !char.IsDigit(c);

    Console.WriteLine(partNumberSum);
}

void Part2()
{
    string[]? lineBuffer = null;

    var gearRatioSum = 0;

    foreach (var readLine in File.ReadLines("PuzzleInput.txt").Append(null))
    {
        if (lineBuffer != null)
        {
            lineBuffer[0] = lineBuffer[1];
            lineBuffer[1] = lineBuffer[2];
            lineBuffer[2] = readLine ?? new string('.', lineBuffer[2].Length);
        }
        else
        {
            lineBuffer = new string[3];
            lineBuffer[0] = lineBuffer[1] = new string('.', readLine!.Length);
            lineBuffer[2] = readLine;
        }


        for (var gearIndex = lineBuffer[1].IndexOf('*', 0);
             gearIndex > -1 && gearIndex < lineBuffer[1].Length - 1;
             gearIndex = lineBuffer[1].IndexOf('*', gearIndex + 1))
        {
            int? firstNumber = null;
            int? secondNumber = null;

            for (var y = 0; y < 3; y++)
            {
                var xMaxExclusive = Math.Min(gearIndex + 2, lineBuffer[1].Length - 1);
                for (var x = Math.Max(0, gearIndex - 1); x < xMaxExclusive; x++)
                {
                    if (char.IsDigit(lineBuffer[y][x]))
                    {
                        var number = ReadNumber(lineBuffer[y], x, out var numberEndIndexExclusive);
                        x = numberEndIndexExclusive;
                        if (!firstNumber.HasValue)
                            firstNumber = number;
                        else if (!secondNumber.HasValue)
                            secondNumber = number;
                        else
                            throw new Exception("Gear must connect exactly two numbers");

                    }
                }
            }

            if (firstNumber != null && secondNumber != null)
                gearRatioSum += firstNumber.Value * secondNumber.Value;
        }
    }

    int ReadNumber(string line, int position, out int numberEndIndexInclusive)
    {
        var lineAsSpan = line.AsSpan();
        var start = position;
        while (start > 0 && char.IsDigit(lineAsSpan[start - 1])) start--;
        var end = position;
        while (end < lineAsSpan.Length-1 && char.IsDigit(lineAsSpan[end + 1])) end++;
        numberEndIndexInclusive = end;
        return int.Parse(lineAsSpan[start..(numberEndIndexInclusive + 1)]);
    }

    Console.WriteLine(gearRatioSum);
}
