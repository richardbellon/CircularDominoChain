using System.CommandLine;
using CircularDominoChain.Lib;

var inFileOption = new Option<FileInfo>(
        name: "--infile",
        description: "File containing the domino set to work on.",
        isDefault: true,
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                return new FileInfo("example-success-input.txt");
            }

            string filePath = result.Tokens.Single().Value;
            if (!File.Exists(filePath))
            {
                result.ErrorMessage = "The provided file does not exist";
                return null!;
            }
            else
            {
                return new FileInfo(filePath);
            }
        });

var outFileOption = new Option<FileInfo>(
        name: "--outfile",
        description: "File path where the output should be written to.",
        isDefault: true,
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                return new FileInfo("output.txt");
            }

            string filePath = result.Tokens.Single().Value;
            return new FileInfo(filePath);
        });

var rootCommand = new RootCommand("Sample app to calculate a cicular domino chain based on a set of dominos");
rootCommand.AddOption(inFileOption);
rootCommand.AddOption(outFileOption);

rootCommand.SetHandler(
    SolveAsync,
    inFileOption,
    outFileOption);

return await rootCommand.InvokeAsync(args);

static async Task SolveAsync(FileInfo inFile, FileInfo outFile)
{
    var dominos = await ParseFileAsync(inFile);
    var solver = new CircularDominoChainSolver();
    var result = await solver.SolveAsync(dominos);

    if (result.IsFailed)
    {
        Console.WriteLine(result.Errors.First().Message);
        return;
    }

    await WriteOutputAsync(outFile, result.Value);
}

static async Task<List<Domino>> ParseFileAsync(FileInfo inFile)
{
    var dominos = new List<Domino>();
    var lines = await File.ReadAllLinesAsync(inFile.FullName);
    for (int i = 0; i < lines.Length; ++i)
    {
        var sides = lines[i].Trim().Split(' ');
        if (sides.Length != 2)
        {
            throw new InvalidOperationException(
                $"Error reading file '{inFile.Name}' at line {i + 1}; expected the two sides of the domino got {lines[i]}");
        }
        if (!int.TryParse(sides[0], out var side1))
        {
            throw new InvalidOperationException(
                $"Error reading file '{inFile.Name}' at line {i + 1}; cannot parse first side of domino: {sides[0]}");
        }
        if (!int.TryParse(sides[1], out var side2))
        {
            throw new InvalidOperationException(
                $"Error reading file '{inFile.Name}' at line {i + 1}; cannot parse second side of domino: {sides[1]}");
        }

        try
        {
            var domino = new Domino(int.Parse(sides[0]), int.Parse(sides[1]));
            dominos.Add(domino);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error reading file '{inFile.Name}' at line {i + 1}", e);
        }
    }

    return dominos;
}

static Task WriteOutputAsync(FileInfo outFile, IEnumerable<Domino> dominos)
{
    return File.WriteAllLinesAsync(outFile.FullName, dominos.Select(d => $"{d.Side1} {d.Side2}"));
}
