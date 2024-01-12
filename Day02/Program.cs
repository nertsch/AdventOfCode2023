using System.Text.RegularExpressions;

Part1();

void Part1()
{
    var maximumCubes = new CubeSubSet(12, 13, 14);

    var result = File.ReadLines("PuzzleInput.txt")
        .Select(Game.Parse)
        .Where(g => g.SubSets.All(s => maximumCubes.Contains(s)))
        .Select(g => g.Id)
        .Sum();

    Console.WriteLine(result);
}

record Game(int Id, IReadOnlyCollection<CubeSubSet> SubSets)
{
    public static Game Parse(string gameAsString)
    {
        var match = Regex.Match(gameAsString, @"Game\s+(?<gameId>\d+):((?<cubeSubSet>.+?)(;|$))*");
        if(!match.Success)
            throw new Exception($"Invalid game:'{gameAsString}'");

        var gameId = int.Parse(match.Groups["gameId"].Value);
        var subSets = match.Groups["cubeSubSet"].Captures.Select(c => CubeSubSet.Parse(c.Value)).ToArray();
        
        return new Game(gameId, subSets);
    }
}

record CubeSubSet(int Red, int Green, int Blue)
{
    public bool Contains(CubeSubSet other) => 
        Red >= other.Red && 
        Green >= other.Green && 
        Blue >= other.Blue;

    public static CubeSubSet Parse(string subSetAsString)
    {
        var red = 0;
        var green = 0;
        var blue = 0;

        foreach (var colorPart in subSetAsString.Split(',').Select(p => p.Trim()))
        {
            if(!(colorPart.Split(' ') is [var amountAsString, var colorName] && int.TryParse(amountAsString, out var amount)))
                throw new Exception($"Invalid Subset:'{subSetAsString}'");

            switch (colorName)
            {
                case "red":
                    red = amount;
                    break;
                case "green":
                    green = amount;
                    break;
                case "blue":
                    blue = amount;
                    break;
                default:
                    throw new Exception("Invalid color: '{colorName}'");
            }
        }

        return new CubeSubSet(red, green, blue);
    }

}
