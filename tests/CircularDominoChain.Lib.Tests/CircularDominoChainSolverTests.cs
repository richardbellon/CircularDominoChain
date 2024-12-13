using FluentAssertions;

namespace CircularDominoChain.Lib.Tests;

public class CircularDominoChainSolverTests
{
    [Fact]
    public async Task SetIsEmpty_SolveAsync_ReturnsEmptyChain()
    {
        var sut = CreateSut();
        var dominos = new List<Domino>();

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeTrue();
        solveResult.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task SetContainsOneWithSameSides_SolveAsync_ReturnsChainWithTheElement()
    {
        var sut = CreateSut();
        var dominos = new List<Domino> {
            new (1, 1)
        };

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeTrue();
        var chain = solveResult.Value.ToList();
        chain.Count.Should().Be(1);
        chain[0].Should().Be(dominos[0]);
    }

    [Fact]
    public async Task SetContainsOneWithDifferentSides_SolveAsync_ReturnsFailure()
    {
        var sut = CreateSut();
        var dominos = new List<Domino> {
            new (1, 2)
        };

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task SetIsExerciseExample1_SolveAsync_ReturnsChainInOrder()
    {
        var sut = CreateSut();
        var dominos = new List<Domino> { new(2, 1), new(2, 3), new(1, 3), };

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeTrue();
        IsCircularChain(solveResult.Value).Should().BeTrue();
    }

    [Fact]
    public async Task SetIsExerciseExample2_SolveAsync_ReturnsFailure()
    {
        var sut = CreateSut();
        var dominos = new List<Domino> { new(1, 2), new(4, 1), new(2, 3), };

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(SetsWithNoSolution))]
    public async Task SetHasNoCircularChain_SolveAsync_ReturnsFailure(IEnumerable<Domino> dominos)
    {
        var sut = CreateSut();

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(1, 1000)]
    [InlineData(2, 10000)]
    [InlineData(3, 100000)]
    [InlineData(4, 1000000)]
    [InlineData(5, 9999999)]
    public async Task SetHasCircularChain_SolveAsync_ReturnsChainInOrder(int randomSeed, long cardinality)
    {
        var sut = CreateSut();
        var dominos = CircularDominoChainGenerator.Generate(randomSeed, cardinality).ToArray();
        var random = new Random(randomSeed);
        random.Shuffle(dominos);

        var solveResult = await sut.SolveAsync(dominos);

        solveResult.IsSuccess.Should().BeTrue();
        var chain = solveResult.Value.ToList();
        chain.Count.Should().Be(dominos.Length);
        IsCircularChain(solveResult.Value).Should().BeTrue();
    }

    public static IEnumerable<object[]> SetsWithNoSolution =>
       [
            [new Domino[] { new(1,2), new(2,3), new(3,4), }],
            [new Domino[] { new(1,2), new(2,3), new(5,6), new(6,1), }],
            [new Domino[] { new(1,2), new(2,3), new(2,3), new(3,1), }],
       ];

    private static bool IsCircularChain(IEnumerable<Domino> dominoChain)
    {
        using var enumerator = dominoChain.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return true;
        }

        var firstDomino = enumerator.Current;
        var prevDomino = firstDomino;
        while (enumerator.MoveNext())
        {
            if (!prevDomino.CanLinkTo(enumerator.Current))
            {
                return false;
            }

            prevDomino = enumerator.Current;
        }

        return prevDomino.CanLinkTo(firstDomino);
    }

    private static CircularDominoChainSolver CreateSut()
    {
        return new CircularDominoChainSolver();
    }
}
