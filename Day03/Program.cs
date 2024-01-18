Part1();
Console.ReadKey();

void Part1()
{
    string previousLine = null!;
    string lineToEvaluate = null!;
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
            previousLine = lineToEvaluate = new string('.', readLine.Length);
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

